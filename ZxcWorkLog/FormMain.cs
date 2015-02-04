using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using ZxcWorkLog.Data;
using ZxcWorkLog.Properties;
using ZxcWorkLog.Util;

namespace ZxcWorkLog
{
    public partial class FormMain : Form
    {
        private readonly KeyboardHook hook = new KeyboardHook();
        private bool canClose;
        private WorkItem itemInProgress;
        private int listItemId = -1;
        private DateTime timerStart;
        private ListViewItem hoveredItem;

        private DateTime lastScreenShotTaken;

        private string originalTitle;

        private enum NotifyIconBaloonAction
        {
            None,
            CheckForUpdates
        }

        private NotifyIconBaloonAction _baloonAction = NotifyIconBaloonAction.None;

        private readonly Updater _updater = Updater.GetInstance();

        public FormMain()
        {
            InitializeComponent();

            timerCheckForUpdates.Interval = (int) new TimeSpan(hours: 0, minutes: 30, seconds: 0).TotalMilliseconds;
            timerCheckForUpdates.Enabled = true;

            var currentVersion = Updater.GetInstance().GetCurrentVersion();
            if (currentVersion != null)
            {
                notifyIcon1.Text += string.Format(" v{0}", currentVersion);
            }
        }

        public bool Setup()
        {
            var success = true;
            hook.KeyPressed += hook_KeyPressed;
            success &= hook.RegisterHotKey(Util.ModifierKeys.Control | Util.ModifierKeys.Shift, Keys.Insert);
            success &= hook.RegisterHotKey(Util.ModifierKeys.Control | Util.ModifierKeys.Shift, Keys.Home);
            success &= hook.RegisterHotKey(Util.ModifierKeys.Control | Util.ModifierKeys.Shift, Keys.End);
            success &= hook.RegisterHotKey(Util.ModifierKeys.Control | Util.ModifierKeys.Shift, Keys.Delete);
            success &= hook.RegisterHotKey(Util.ModifierKeys.Control | Util.ModifierKeys.Shift, Keys.PageUp);
            success &= hook.RegisterHotKey(Util.ModifierKeys.Control | Util.ModifierKeys.Shift,
                Keys.PageDown);

            if (!success)
            {
                MessageBox.Show(
                    "Could not register all required shortcuts.\nPlease check that application is not already running.\n\nApplication will now exit...");
                canClose = true;
                Application.Exit();
            }

            return success;
        }

        public Icon getActiveIcon()
        {
            return Resources.green_ico;
        }

        public Icon getInactiveIcon()
        {
            return Resources.gray_ico;
        }

        private void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            if ((e.Modifier & (Util.ModifierKeys.Control | Util.ModifierKeys.Shift))
                == (Util.ModifierKeys.Control | Util.ModifierKeys.Shift))
            {
                if (e.Key == Keys.Insert)
                {
                    if (itemInProgress != null)
                    {
                        var timeSpent = DateTime.Now.Ticks - timerStart.Ticks;
                        var timeTotal = itemInProgress.PeriodTicks + timeSpent;

                        var tipText =
                            string.Format("Work Item: {0}\n\nTime spent now: {1}\nTime spent total: {2}",
                                itemInProgress.Title, TimeUtil.ToReadableTime(timeSpent), TimeUtil.ToReadableTime(timeTotal));
                        notifyIcon1.ShowBalloonTip(1, "Already In Progress", tipText, ToolTipIcon.Info);
                        return;
                    }

                    var wia = new WorkItemAdd(this);
                    wia.Show();
                }
                if (e.Key == Keys.Home)
                {
                    if (itemInProgress != null)
                    {
                        TryEndProgessWithConfirmation();
                        return;
                    }
                    if (listView1.Items.Count == 0) return;
                    var wi = (WorkItem) listView1.Items[listItemId];
                    if (wi.WasWorkLogged)
                    {
                        MessageBox.Show(@"This work log item was already logged.");
                        return;
                    }
                    StartProgess(wi);
                    ShowGhost();
                }
                if (e.Key == Keys.Delete)
                {
                    if (listView1.Items.Count > 0)
                    {
                        var wi = (WorkItem) listView1.Items[listItemId];
                        EditItem(wi);
                    }
                }
                if (e.Key == Keys.End)
                {
                    var wi = itemInProgress;
                    if (TryEndProgessWithConfirmation())
                    {
                        EditItem(wi);
                    }
                }
                if (e.Key == Keys.PageUp)
                {
                    if (itemInProgress != null)
                    {
                        var tipText =
                            string.Format(
                                "Please stop current work before continuing.\nWork Item: {0}\n\nTime spent before: {1}",
                                itemInProgress.Title, TimeUtil.ToReadableTime(itemInProgress.PeriodTicks));
                        notifyIcon1.ShowBalloonTip(1, "Work is in progress", tipText, ToolTipIcon.Error);
                        return;
                    }
                    var prevItem = listView1.Items[listItemId];
                    // up in group
                    if (prevItem.Group.Items.IndexOf(prevItem) > 0)
                    {
                        listItemId = prevItem.Group.Items[prevItem.Group.Items.IndexOf(prevItem) - 1].Index;
                    }
                    // up to prev group
                    if (prevItem.Group.Items.IndexOf(prevItem) == 0 && listView1.Groups.IndexOf(prevItem.Group) > 0)
                    {
                        var prevGoupIndex = listView1.Groups.IndexOf(prevItem.Group) - 1;
                        var prevGroup = listView1.Groups[prevGoupIndex];
                        listItemId = prevGroup.Items[prevGroup.Items.Count - 1].Index;
                    }

                    LoadWorkItems();
                    if (timerGhost.Enabled || WindowState != FormWindowState.Normal) ShowGhost();
                    return;
                }
                if (e.Key == Keys.PageDown)
                {
                    if (itemInProgress != null)
                    {
                        var tipText =
                            string.Format(
                                "Please stop current work before continuing.\nWork Item: {0}\n\nTime spent before: {1}",
                                itemInProgress.Title, TimeUtil.ToReadableTime(itemInProgress.PeriodTicks));
                        notifyIcon1.ShowBalloonTip(1, "Work is in progress", tipText, ToolTipIcon.Error);
                        return;
                    }
                    var prevItem = listView1.Items[listItemId];
                    // down in group
                    if (prevItem.Group.Items.IndexOf(prevItem) < prevItem.Group.Items.Count - 1)
                    {
                        listItemId = prevItem.Group.Items[prevItem.Group.Items.IndexOf(prevItem) + 1].Index;
                    }
                    // down to next group
                    if (prevItem.Group.Items.IndexOf(prevItem) == prevItem.Group.Items.Count - 1 &&
                        listView1.Groups.IndexOf(prevItem.Group) < listView1.Groups.Count - 1)
                    {
                        var prevGoupIndex = listView1.Groups.IndexOf(prevItem.Group) + 1;
                        var prevGroup = listView1.Groups[prevGoupIndex];
                        listItemId = prevGroup.Items[0].Index;
                    }

                    LoadWorkItems();
                    if (timerGhost.Enabled || WindowState != FormWindowState.Normal) ShowGhost();
                    return;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            originalTitle = Text;

            //Hide();
            notifyIcon1.Icon = Icon = getInactiveIcon();
            LoadWorkItems();

            if (Common.ScreenShotsEnabled)
            {
                lastScreenShotTaken = DateTime.Now;
                timerScreenShots.Start();
            }

            timerWorkRatio.Start();
            CheckWorkRatio();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fs = new FormSettings();
            fs.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadWorkItems();
        }

        private ListViewGroup GetGroupByName(string name)
        {
            foreach (ListViewGroup listViewGroup in getItemGroups())
            {
                if (listViewGroup.Name == name)
                {
                    return listViewGroup;
                }
            }
            return null;
        }

        public void LoadWorkItems()
        {
            listView1.Items.Clear();

            var i = 0;
            var wis = WorkLog.getWorkItems();
            var totalNonDistributedTicksPerGroup = new Dictionary<String, long>();
            var totalDistributedTicksPerGroup = new Dictionary<String, long>();
            var sortedList = wis.GetSortedList();
            foreach (WorkItem wi in sortedList)
            {
                if (!totalNonDistributedTicksPerGroup.ContainsKey(wi.GroupName))
                    totalNonDistributedTicksPerGroup[wi.GroupName] = 0;
                if (!totalDistributedTicksPerGroup.ContainsKey(wi.GroupName))
                    totalDistributedTicksPerGroup[wi.GroupName] = 0;
                if (wi.IsDistributed)
                {
                    totalDistributedTicksPerGroup[wi.GroupName] += wi.PeriodTicks;
                }
                else
                {
                    totalNonDistributedTicksPerGroup[wi.GroupName] += wi.PeriodTicks;
                }
            }
            var wiList = new WorkItem[wis.Count];
            foreach (WorkItem wi in sortedList)
            {
                var group = GetGroupByName(wi.GroupName);
                if (wi.GroupName != "" && group == null)
                {
                    group = listView1.Groups.Add(wi.GroupName, wi.GroupName);
                }
                wi.Group = group;

                if (listItemId < 0) listItemId = 0;
                if (wi.SubItems.Count >= 2) wi.SubItems.Clear();

                if (wi.WasWorkLogged)
                {
                    wi.ForeColor = Color.LightGray;
                }
                if (wi.IsDistributed)
                {
                    wi.ForeColor = Color.LightSalmon;
                }
                if (i == listItemId)
                {
                    wi.Font = new Font(DefaultFont, FontStyle.Bold);
                }
                if (itemInProgress != null && itemInProgress.Id == wi.Id)
                {
                    wi.ForeColor = Color.Green;
                }

                var lvsi = new ListViewItem.ListViewSubItem
                           {
                               Text = wi.StartTime.ToString("MM.dd HH:mm")
                           };
                wi.SubItems.Add(lvsi);

                var firstLine = "";
                if (wi.Title.IndexOf("\n") > 0)
                {
                    var numberOfLines = 0;
                    var lines = wi.Title.Split(new[] {'\n'});
                    foreach (var line in lines)
                    {
                        if (line.Trim().Length != 0) numberOfLines++;
                    }
                    firstLine = string.Format("{0} (+{1} lines)", wi.Title.Substring(0, wi.Title.IndexOf("\n")),
                        numberOfLines - 1);
                }
                else
                {
                    firstLine = wi.Title;
                }
                wi.ToolTipText = wi.Title;

                lvsi = new ListViewItem.ListViewSubItem {Text = firstLine};
                wi.SubItems.Add(lvsi);

                wi.RealTicks = wi.PeriodTicks;
                if (totalNonDistributedTicksPerGroup[wi.GroupName] > 0 && totalDistributedTicksPerGroup[wi.GroupName] > 0)
                {
                    Console.WriteLine("totalNonDistributedTicks=" + TimeUtil.ToReadableTime(totalNonDistributedTicksPerGroup[wi.GroupName]));
                    Console.WriteLine("totalDistributedTicks=" + TimeUtil.ToReadableTime(totalDistributedTicksPerGroup[wi.GroupName]));
                    Console.WriteLine("wi.PeriodTicks=" + TimeUtil.ToReadableTime(wi.PeriodTicks));
                    if (wi.IsDistributed)
                    {
                        wi.RealTicks = 0;
                    }
                    else 
                    {
                        wi.RealTicks += (long)(((totalDistributedTicksPerGroup[wi.GroupName] * 1.0) / totalNonDistributedTicksPerGroup[wi.GroupName]) * wi.PeriodTicks);
                    }
                }
                lvsi = new ListViewItem.ListViewSubItem
                    {
                        Text = (wi.InProgress ? "> " : "") + TimeUtil.ToReadableTime(wi.RealTicks)
                    };
                if (wi.PeriodTicks != wi.RealTicks)
                {
                    lvsi.Text = string.Format("{0} ({1})", lvsi.Text, TimeUtil.ToReadableTime(wi.PeriodTicks));
                }
                wi.SubItems.Add(lvsi);

                wiList[i] = wi;
                i++;
            }
            listView1.Items.AddRange(wiList);

            OriganizeGroups();
            CheckWorkRatio(true);
        }

        public void StartProgess(WorkItem wi)
        {
            if (itemInProgress != null) return;

            var tipText =
                "Work Item: " + wi.Title + "\n" +
                "\n" +
                "Time spent before: " + TimeUtil.ToReadableTime(wi.PeriodTicks);

            notifyIcon1.Icon = Icon = getActiveIcon();
            notifyIcon1.ShowBalloonTip(3, "Work Started", tipText, ToolTipIcon.Info);

            itemInProgress = wi;
            timerStart = DateTime.Now;
            timer1.Start();
            wi.StartProgress();
            LoadWorkItems();
        }

        private bool TryEndProgessWithConfirmation()
        {
            var timeSpent = DateTime.Now.Ticks - timerStart.Ticks;

            var allowToStop = true;
            if (timeSpent > TimeSpan.FromMinutes(3).Ticks && timeSpent < TimeSpan.FromMinutes(40).Ticks)
            {
                allowToStop = (MessageBox.Show(
                    "You have not spent the recommended 40 minutes working.\n\nAre you sure you want to stop the timer?",
                    "Are you sure?", MessageBoxButtons.YesNo) == DialogResult.Yes);
            }
            if (allowToStop) 
            {
                EndProgess();
                return true;
            }
            return false;
        }

        private void EndProgess(long extraTicksToSubtract = 0)
        {
            if (itemInProgress == null) return;

            notifyIcon1.Icon = Icon = getInactiveIcon();

            var timeSpent = DateTime.Now.Ticks - timerStart.Ticks - extraTicksToSubtract;
            var timeTotal = itemInProgress.PeriodTicks + (timeSpent > TimeSpan.FromMinutes(2).Ticks ? timeSpent : 0);

            timer1.Stop();
            itemInProgress.StopProgress();

            if (timeSpent > TimeSpan.FromMinutes(2).Ticks)
            {
                itemInProgress.PeriodTicks += timeSpent;
                itemInProgress.Title += "\r\n" + "(" + timerStart.Day + "." + timerStart.Month + " " +
                                        timerStart.ToShortTimeString() + " -> " + DateTime.Now.ToShortTimeString() +
                                        ") " +
                                        TimeUtil.ToReadableTime(timeSpent) +
                                        (extraTicksToSubtract > 0
                                            ? " (excl " + TimeUtil.ToReadableTime(extraTicksToSubtract) + " idle)"
                                            : "") + " - ";
                WorkLog.updateItem(itemInProgress.Id, itemInProgress);
            }

            var tipText =
                string.Format("Work Item: {0}\n\nTime spent now: {1}{2}\nTime spent total: {3}", itemInProgress.Title,
                    TimeUtil.ToReadableTime(timeSpent), (timeSpent <= TimeSpan.FromMinutes(2).Ticks ? " (ignored)" : ""),
                    TimeUtil.ToReadableTime(timeTotal));
            notifyIcon1.ShowBalloonTip(3, "Work Stopped", tipText, ToolTipIcon.Info);

            itemInProgress = null;
            LoadWorkItems();
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Opacity = 1;
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            canClose = true;
            Close();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !canClose)
            {
                e.Cancel = true;
                WindowState = FormWindowState.Minimized;
            }
            else
            {
                EndProgess();
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            var wi = (WorkItem) listView1.SelectedItems[0];
            EditItem(wi);
        }

        private void EditItem(WorkItem wi)
        {
            var wie = new WorkItemEdit(this, wi);
            wie.Show();
        }

        private void notifyIcon1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (WindowState == FormWindowState.Normal)
                {
                    WindowState = FormWindowState.Minimized;
                    Hide();
                }
                else
                {
                    Opacity = 1;
                    Show();
                    WindowState = FormWindowState.Normal;
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            long ticks = 0, realTicks = 0;
            foreach (var item in listView1.Items)
            {
                if (item is WorkItem && ((WorkItem) item).Selected)
                {
                    ticks += ((WorkItem) item).PeriodTicks;
                    realTicks += ((WorkItem)item).RealTicks;
                }
            }
            label2.Text = string.Format("Total time: {0} ({1})", TimeUtil.ToReadableTime(realTicks, false, false), TimeUtil.ToReadableTime(ticks, false, false));
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void ShowGhost()
        {
            TopMost = true;
            timerGhost.Stop();
            Opacity = 1;
            Show();
            WindowState = FormWindowState.Normal;

            timerGhost.Interval = 2000;
            timerGhost.Start();
        }

        private void timerGhost_Tick(object sender, EventArgs e)
        {
            if (TopMost) TopMost = false;
            if (Opacity <= 0)
            {
                timerGhost.Stop();
                Hide();
                WindowState = FormWindowState.Minimized;
                Opacity = 1;
                return;
            }

            timerGhost.Interval = 10;
            Opacity -= 0.1;
        }

        private void timerScreenShots_Tick(object sender, EventArgs e)
        {
            if (itemInProgress == null) return;
            if (!Directory.Exists(Common.ScreenShotsDir))
            {
                MessageBox.Show(
                    @"Directory where screenshots are saved doesn't exist!\nScreen shot saving will be disabled.");
                timerScreenShots.Stop();
                return;
            }

            if (DateTime.Now.Ticks - lastScreenShotTaken.Ticks >= TimeSpan.FromSeconds(Common.ScreenShotsTimeout).Ticks)
            {
                lastScreenShotTaken = DateTime.Now;

                var identifier = itemInProgress.StartTime.ToString("yy-MM-dd H.mm.ss");
                var timeNow = DateTime.Now.ToString("yy-MM-dd H.mm.ss");
                var screenShotFileName = string.Format(@"{0}\Task {1} at {2}.jpg", Common.ScreenShotsDir, identifier,
                    timeNow);

                var bounds = Screen.GetBounds(Point.Empty);
                using (var bitmap = new Bitmap(bounds.Width, bounds.Height))
                {
                    using (var g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                    }
                    bitmap.Save(screenShotFileName, ImageFormat.Jpeg);
                }
            }
        }

        private void timerIdle_Tick(object sender, EventArgs e)
        {
            if (itemInProgress == null) return;

            var idleSeconds = IdleTime.GetUserIdleSeconds();
            if (idleSeconds >= 5*60)
            {
                EndProgess(TimeSpan.FromSeconds(idleSeconds).Ticks);
                MessageBox.Show(
                    string.Format("You were idle for {0} seconds ({1} minutes), so timer was stopped automatically.",
                        idleSeconds, (idleSeconds/60)), Text);
            }
        }

        public ListViewGroupCollection getItemGroups()
        {
            return listView1.Groups;
        }

        public IEnumerable<ListViewItem> getOrderedItems()
        {
            var items = new List<ListViewItem>();

            foreach (ListViewGroup itemGroup in getItemGroups())
            {
                foreach (ListViewItem item in itemGroup.Items)
                {
                    items.Add(item);
                }
            }
            return items;
        }

        private void contextMenuStripList_Opening(object sender, CancelEventArgs e)
        {
            //var label = contextMenuStripList.Items[0];
            moveToGroupToolStripMenuItem.DropDownItems.Clear();
            //contextMenuStripList.Items.Add(label);
            foreach (ListViewGroup itemGroup in getItemGroups())
            {
                var menuItem = moveToGroupToolStripMenuItem.DropDownItems.Add(itemGroup.Name);
                menuItem.Click += GroupChangeClick;
            }
            if (moveToGroupToolStripMenuItem.DropDownItems.Count > 0)
            {
                moveToGroupToolStripMenuItem.Enabled = true;
            }
        }

        private void GroupChangeClick(object sender, EventArgs e)
        {
            // find group
            var group = GetGroupByName(((ToolStripItem) sender).Text);
            if (group == null) return;

            foreach (ListViewItem selectedItem in listView1.SelectedItems)
            {
                ((WorkItem) selectedItem).GroupName = group.Name;
                // selectedItem.Group = group;
            }
            WorkLog.saveWorkItems();
            LoadWorkItems();
            OriganizeGroups();
        }

        private void OriganizeGroups()
        {
            var toBeRemoved = new List<ListViewGroup>();
            foreach (ListViewGroup listViewGroup in listView1.Groups)
            {
                if (listViewGroup.Items.Count == 0)
                {
                    toBeRemoved.Add(listViewGroup);
                }
                else
                {
                    long ticks = 0;
                    foreach (WorkItem wi in listViewGroup.Items)
                    {
                        ticks += wi.PeriodTicks;
                    }
                    listViewGroup.Header = string.Format("{0}   -   {1}", listViewGroup.Name,
                        TimeUtil.ToReadableTime(ticks, false));
                }
            }
            foreach (var listViewGroup in toBeRemoved)
            {
                listView1.Groups.Remove(listViewGroup);
            }
            ((ListViewGroupSorter) listView1).SortGroups();
        }

        private void markWorkLoggedToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void listView1_MouseHover(object sender, EventArgs e)
        {
        }

        private void markWorkLoggedNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem selectedItem in listView1.SelectedItems)
            {
                ((WorkItem) selectedItem).WasWorkLogged = true;
            }
            WorkLog.saveWorkItems();
            LoadWorkItems();
        }

        private void add5MinutesNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem selectedItem in listView1.SelectedItems)
            {
                if (((WorkItem) selectedItem).WasWorkLogged) continue;

                if (!((WorkItem) selectedItem).Title.EndsWith("\n"))
                {
                    ((WorkItem) selectedItem).Title += "\r\n";
                }
                ((WorkItem) selectedItem).Title += string.Format("+5 minutes ({0})\r\n",
                    DateTime.Now.ToString("yyyy.MM.dd H:mm:ss"));
                ((WorkItem) selectedItem).PeriodTicks += TimeSpan.FromMinutes(5).Ticks;
            }
            WorkLog.saveWorkItems();
            LoadWorkItems();
        }

        private void listView1_MouseMove(object sender, MouseEventArgs e)
        {
            var item = listView1.GetItemAt(e.X, e.Y);
            if (item == hoveredItem) return;

            hoveredItem = item;
            if (hoveredItem is WorkItem)
            {
                toolTip1.SetToolTip(listView1, ((WorkItem) hoveredItem).Title);
            }
            else
            {
                toolTip1.SetToolTip(listView1, "");
            }
        }

        private void CheckWorkRatio(bool quiet = false)
        {
            var todayGroupName = DateTime.Now.Hour < 10
                ? DateTime.Now.AddDays(-1).ToString("yyyy.MM.dd")
                : DateTime.Now.ToString("yyyy.MM.dd");
            var todayGroup = GetGroupByName(todayGroupName);
            if (todayGroup == null)
            {
                return;
            }

            long workedTicks = 0;
            foreach (var item in todayGroup.Items)
            {
                if (item is WorkItem)
                {
                    workedTicks += ((WorkItem) item).PeriodTicks;
                }
            }
            if (workedTicks <= 0) return;

            var workedHours = TimeUtil.ToReadableTime(workedTicks);

            var workingDayStartTime = DateTime.Now.Date;
            if (DateTime.Now.Hour < 10)
            {
                workingDayStartTime = DateTime.Now.AddDays(-1).Date;
            }
            workingDayStartTime = workingDayStartTime.AddHours(10);

            var elapsedTicks = DateTime.Now.Ticks - workingDayStartTime.Ticks;
            var ticksNeedToWork = TimeSpan.FromHours(Common.HoursPerDay).Ticks;

            var effectiveness = Math.Round((workedTicks*100.0)/elapsedTicks);
            var percent = Math.Round((workedTicks*100.0)/ticksNeedToWork);

            Console.WriteLine(@"Worked: {0}, Elapsed: {1}, % of work done: {2}, efectiveness: {3}%",
                TimeSpan.FromTicks(workedTicks), TimeSpan.FromTicks(elapsedTicks), percent, effectiveness);

            if (percent <= 0) return;

            var x1 = (1.0*elapsedTicks)/percent;
            var x2 = 100*x1;
            var ticksTotalToWork = (long) x2;
            Console.WriteLine(@"Work will be done at {0}", workingDayStartTime.AddTicks(ticksTotalToWork));

            Text = string.Format(@"{0} - {1}% done, {2}% effective", originalTitle, percent, effectiveness);
            notifyIcon1.Text = Text;

            label1.Text = string.Format("Effectiveness: {0}%\r\nEnd of work: {1}", effectiveness,
                workingDayStartTime.AddTicks(ticksTotalToWork).ToString("yyyy.MM.dd HH:mm:ss"));

            if (quiet || itemInProgress != null) return;
            if (!Common.EfectivenessWarningEnabled)
            {
                return;
            }

            if (percent >= 100)
            {
                return;
            }

            if (ticksTotalToWork > TimeSpan.FromHours(14).Ticks)
            {
                var tipText =
                    string.Format("Work done: {0}% ({1})\nEffectiveness: {2}%\n\nEnd of work: {3} (> midnight)", percent,
                        workedHours.Trim(), effectiveness,
                        workingDayStartTime.AddTicks(ticksTotalToWork).ToString("yyyy.MM.dd HH:mm:ss"));
                notifyIcon1.ShowBalloonTip(3, "WARNING!", tipText, ToolTipIcon.Error);
            }
            else
            {
                if (effectiveness < 60)
                {
                    var tipText =
                        string.Format("Work done: {0}% ({1})\nEffectiveness: {2}% (< 60%)\n\nEnd of work: {3}", percent,
                            workedHours.Trim(), effectiveness,
                            workingDayStartTime.AddTicks(ticksTotalToWork).ToString("yyyy.MM.dd HH:mm:ss"));
                    notifyIcon1.ShowBalloonTip(5, "WARNING!", tipText, ToolTipIcon.Error);
                }
            }

            if (effectiveness < 50)
            {
                timerWorkRatio.Interval = 3000;
            }
            else if (effectiveness < 60)
            {
                timerWorkRatio.Interval = 10000;
            }
            else
            {
                timerWorkRatio.Interval = 30000;
            }
        }

        private void timerWorkRatio_Tick(object sender, EventArgs e)
        {
            CheckWorkRatio();
        }

        public void ShowNotifyMessage(string title, string msg)
        {
            notifyIcon1.ShowBalloonTip(1000, title, msg, ToolTipIcon.Info);
        }

        private void timerCheckForUpdates_Tick(object sender, EventArgs e)
        {
            if (_updater.IsImportantUpdateAvailable())
            {
                _baloonAction = NotifyIconBaloonAction.CheckForUpdates;
                notifyIcon1.ShowBalloonTip(10000, "Update is available", "A new version of ZxcScreenShot is available!",
                    ToolTipIcon.Info);
            }
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            switch (_baloonAction)
            {
                case NotifyIconBaloonAction.CheckForUpdates:
                    _updater.ShowApplicationUpdatePrompt();

                    timerCheckForUpdates.Enabled = false;
                    break;
            }
            _baloonAction = NotifyIconBaloonAction.None;
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_updater.IsAnyUpdateAvailable())
            {
                _updater.ShowApplicationUpdatePrompt();
            }
            else
            {
                MessageBox.Show("No updates were found.");
            }
        }
    }
}