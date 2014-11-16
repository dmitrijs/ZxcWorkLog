using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ZxcWorkLog.Util
{
    public class ListViewGroupSorter
    {
        private readonly ListView _listview;

        public static bool operator ==(ListView listview, ListViewGroupSorter sorter)
        {
            return listview == sorter._listview;
        }

        public static bool operator !=(ListView listview, ListViewGroupSorter sorter)
        {
            return listview != sorter._listview;
        }

        public static implicit operator ListView(ListViewGroupSorter sorter)
        {
            return sorter._listview;
        }

        public static implicit operator ListViewGroupSorter(ListView listview)
        {
            return new ListViewGroupSorter(listview);
        }

        private ListViewGroupSorter(ListView listview)
        {
            _listview = listview;
        }

        public void SortGroups()
        {
            _listview.BeginUpdate();
            var lvgs = new List<ListViewGroup>();
            foreach (ListViewGroup lvg in _listview.Groups)
            {
                lvgs.Add(lvg);
            }
            _listview.Groups.Clear();
            lvgs.Sort(new ListViewGroupHeaderSorter());
            _listview.Groups.AddRange(lvgs.ToArray());
            _listview.EndUpdate();
        }

        #region overridden methods

        public override bool Equals(object obj)
        {
            return _listview.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _listview.GetHashCode();
        }

        public override string ToString()
        {
            return _listview.ToString();
        }

        #endregion
    }


    public class ListViewGroupHeaderSorter : IComparer<ListViewGroup>
    {
        public int Compare(ListViewGroup x, ListViewGroup y)
        {
            if (x.Header.Length > 0 && y.Header.Length > 0)
            {
                if (Char.IsDigit(x.Header[0]) && Char.IsDigit(y.Header[0]))
                {
                    return -string.Compare(x.Header, y.Header);
                }
            }
            return string.Compare(x.Header, y.Header);
        }
    }
}