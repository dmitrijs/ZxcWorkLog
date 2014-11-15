using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Windows.Forms;
using ZxcWorkLog.Properties;

namespace ZxcWorkLog.Util
{
    abstract class Updater
    {
        private static Updater _instance;

        // ReSharper disable once EmptyConstructor
        protected Updater()
        {
        }

        protected void RestartApplication()
        {
            Program.FormMain.Close();
            Application.Restart();
        }

        public static Updater GetInstance()
        {
            if (_instance == null)
            {
                _instance = ApplicationDeployment.IsNetworkDeployed
                    ? (Updater)new NetworkDeployedUpdater()
                    : new NonUpdater();
            }
            return _instance;
        }

        public abstract bool IsAnyUpdateAvailable();
        public abstract bool IsImportantUpdateAvailable();
        public abstract void ShowApplicationUpdatePrompt();
        public abstract void ShowUpdateChangeLog();
        public abstract Version GetCurrentVersion();
        public abstract bool AppHasUpdated();
    }

    class NonUpdater : Updater
    {
        public override bool IsAnyUpdateAvailable()
        {
            return true;
        }

        public override bool IsImportantUpdateAvailable()
        {
            return false;
        }

        public override void ShowApplicationUpdatePrompt()
        {
            RestartApplication();
        }

        public override void ShowUpdateChangeLog()
        {
        }

        public override Version GetCurrentVersion()
        {
            return null;
        }

        public override bool AppHasUpdated()
        {
            return false;
        }
    }

    class NetworkDeployedUpdater : Updater
    {
        private UpdateCheckInfo _updateInfo;
        private readonly Version _lastUsedVersion;

        public NetworkDeployedUpdater()
        {
            _lastUsedVersion = GetAndUpdateLastUsedVersion();
        }

        private void CheckForUpdates()
        {
            _updateInfo = ApplicationDeployment.CurrentDeployment.CheckForDetailedUpdate();
        }

        public override bool IsAnyUpdateAvailable()
        {
            CheckForUpdates();
            return _updateInfo.UpdateAvailable;
        }

        public override bool IsImportantUpdateAvailable()
        {
            CheckForUpdates();
            if (_updateInfo.UpdateAvailable)
            {
                var updateVersionWithoutRevision = new Version(_updateInfo.AvailableVersion.ToString(3));
                var currentVersionWithoutRevision = new Version(ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(3));

                if (updateVersionWithoutRevision > currentVersionWithoutRevision)
                {
                    return true;
                }
            }
            return false;
        }

        public override void ShowApplicationUpdatePrompt()
        {
            var message =
                string.Format(
                    "New version of ZxcWorkLog is available!\n\nYour version: {0}\nNew version: {1}\n\nInstall the update?",
                    ApplicationDeployment.CurrentDeployment.CurrentVersion,
                    _updateInfo.AvailableVersion
                    );

            if (MessageBox.Show(message, @"Update found", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                ApplicationDeployment.CurrentDeployment.Update();
                RestartApplication();
            }
        }

        public override void ShowUpdateChangeLog()
        {
            if (_lastUsedVersion == null)
            {
                return;
            }
            ShowChangeLog(ApplicationDeployment.CurrentDeployment.CurrentVersion, _lastUsedVersion);
        }

        public override Version GetCurrentVersion()
        {
            return ApplicationDeployment.CurrentDeployment.CurrentVersion;
        }

        public override bool AppHasUpdated()
        {
            return _lastUsedVersion != ApplicationDeployment.CurrentDeployment.CurrentVersion;
        }

        private Version GetAndUpdateLastUsedVersion()
        {
            var previousVersionStr = Settings.Default.LastUsedVersion;

            Settings.Default.LastUsedVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            Settings.Default.Save();

            if (previousVersionStr.Length == 0)
            {
                return null;
            }
            return new Version(previousVersionStr);
        }

        private void ShowChangeLog(Version currentVersion, Version previousVersion)
        {
            var versionToChanges = new Dictionary<Version, List<string>>();

            var changeLog = "";
            foreach (var versionToChange in versionToChanges)
            {
                if (versionToChange.Key > previousVersion && versionToChange.Key <= currentVersion)
                {
                    changeLog += "** Version " + versionToChange.Key + "\n\n";
                    changeLog += string.Join("\n", versionToChange.Value) + "\n\n";
                }
            }

            if (changeLog.Length == 0)
            {
                Program.FormMain.ShowNotifyMessage(@"Success", @"You are now using the latest version of ZxcWorkLog!");
            }
            else
            {
                MessageBox.Show(string.Format("Changes since last installed version:\n\n{0}", changeLog), @"Update was successful");
            }
        }
    }
}
