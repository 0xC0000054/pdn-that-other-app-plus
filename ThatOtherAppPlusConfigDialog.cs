////////////////////////////////////////////////////////////////////////
//
// This file is part of pdn-that-other-app-plus, a Effect plugin for
// Paint.NET that exports the current layer to other image editors.
//
// Copyright (c) 2020, 2024 Nicholas Hayes
//
// This file is licensed under the MIT License.
// See LICENSE.txt for complete licensing and attribution information.
//
////////////////////////////////////////////////////////////////////////

using PaintDotNet;
using PaintDotNet.AppModel;
using PaintDotNet.Effects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using ThatOtherAppPlus.Controls;
using ThatOtherAppPlus.Properties;


namespace ThatOtherAppPlus
{
    using ApplicationIconInfo = KeyValuePair<ApplicationInfo, Bitmap>;

    internal partial class ThatOtherAppPlusConfigDialog : EffectConfigDialog
    {
        private List<ApplicationInfo> applications;
        private ListViewItem[] listViewCache;
        private int cacheStartIndex;
        private bool appRunning;
        private bool formClosePending;
        private Surface surface;
        private ThatOtherAppPlusSettings settings;

        public ThatOtherAppPlusConfigDialog()
        {
            InitializeComponent();
            this.applications = new List<ApplicationInfo>();
            this.UseAppThemeColors = true;
            this.Text = ThatOtherAppPlusEffect.StaticName;
        }

        protected override void InitialInitToken()
        {
            this.theEffectToken = new ThatOtherAppPlusConfigToken();
        }

        protected override void InitTokenFromDialog()
        {
            ThatOtherAppPlusConfigToken token = (ThatOtherAppPlusConfigToken)this.theEffectToken;

            token.Surface?.Dispose();
            token.Surface = this.surface;
        }

        protected override void InitDialogFromToken(EffectConfigToken effectTokenCopy)
        {
            // Not required for this plugin.
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);

            PluginThemingUtil.UpdateControlBackColor(this);
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);

            PluginThemingUtil.UpdateControlForeColor(this);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (this.backgroundWorker.IsBusy)
            {
                if (this.DialogResult == DialogResult.Cancel)
                {
                    this.formClosePending = true;
                    this.backgroundWorker.CancelAsync();
                }
                e.Cancel = true;
            }

            if (!e.Cancel)
            {
                this.settings?.Flush();
            }

            base.OnFormClosing(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {

                string userDataPath = this.Services.GetService<IUserFilesService>().UserFilesPath;

                string path = Path.Combine(userDataPath, "ThatOtherAppPlus.xml");

                this.settings = new ThatOtherAppPlusSettings(path);

                // Loading the settings is split into a separate method to allow the defaults
                // to be used if an error occurs when reading the saved settings.
                this.settings.LoadSavedSettings();
            }
            catch (ArgumentException ex)
            {
                ShowErrorMessage(ex);
            }
            catch (IOException ex)
            {
                ShowErrorMessage(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                ShowErrorMessage(ex);
            }
            catch (XmlException ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.settings != null)
            {
                this.applications.AddRange(this.settings.Applications);
                this.listViewManageApplications.VirtualListSize = this.applications.Count;
                UpdateApplicationList();
            }
        }

        private void ShowErrorMessage(Exception ex)
        {
            IExceptionDialogService service = (IExceptionDialogService)this.Services.GetService(typeof(IExceptionDialogService));

            if (service != null)
            {
                service.ShowErrorDialog(this, ex);
            }
            else
            {
                MessageBox.Show(this, ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void appListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonRemoveApplication.Enabled = this.listViewManageApplications.SelectedIndices.Count > 0;
        }

        private void appListView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (this.listViewCache != null &&
                e.ItemIndex >= this.cacheStartIndex &&
                e.ItemIndex < this.cacheStartIndex + this.listViewCache.Length)
            {
                e.Item = this.listViewCache[e.ItemIndex - this.cacheStartIndex];
            }
            else
            {
                e.Item = new ListViewItem(this.applications[e.ItemIndex].Path);
            }
        }

        private void appListView_CacheVirtualItems(object sender, CacheVirtualItemsEventArgs e)
        {
            // Check if the cache needs to be refreshed.
            if (this.listViewCache != null &&
                e.StartIndex >= this.cacheStartIndex &&
                e.EndIndex <= this.cacheStartIndex + this.listViewCache.Length)
            {
                // If the newly requested cache is a subset of the old cache,
                // no need to rebuild everything, so do nothing.
                return;
            }

            this.cacheStartIndex = e.StartIndex;
            // The indexes are inclusive.
            int length = e.EndIndex - e.StartIndex + 1;
            this.listViewCache = new ListViewItem[length];

            // Fill the cache with the appropriate ListViewItems.
            for (int i = 0; i < length; i++)
            {
                this.listViewCache[i] = new ListViewItem(this.applications[i + this.cacheStartIndex].Path);
            }
        }

        private void InvalidateDirectoryListViewCache(int changedIndex)
        {
            if (this.listViewCache != null)
            {
                int endIndex = this.cacheStartIndex + this.listViewCache.Length;
                if (changedIndex >= this.cacheStartIndex && changedIndex <= endIndex)
                {
                    this.listViewCache = null;
                    if (endIndex > this.listViewManageApplications.VirtualListSize)
                    {
                        endIndex = this.listViewManageApplications.VirtualListSize;
                    }
                    // The indexes in the CacheVirtualItems event are inclusive,
                    // so we need to subtract 1 from the end index.
                    appListView_CacheVirtualItems(this, new CacheVirtualItemsEventArgs(this.cacheStartIndex, endIndex - 1));
                }

                this.listViewManageApplications.Invalidate();
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    bool firstItem = this.applications.Count == 0;

                    this.applications.Add(new ApplicationInfo(this.openFileDialog.FileName));
                    this.listViewManageApplications.VirtualListSize = this.applications.Count;
                    if (firstItem)
                    {
                        this.listViewManageApplications.Invalidate();
                    }
                    else
                    {
                        InvalidateDirectoryListViewCache(this.applications.Count);
                    }
                    UpdateManagedApplicationsSetting();
                    UpdateApplicationList();
                }
                catch (FileNotFoundException ex)
                {
                    ShowErrorMessage(ex);
                }
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (this.listViewManageApplications.SelectedIndices.Count > 0)
            {
                int index = this.listViewManageApplications.SelectedIndices[0];

                this.applications.RemoveAt(index);
                this.listViewManageApplications.VirtualListSize = this.applications.Count;
                InvalidateDirectoryListViewCache(this.applications.Count);
                UpdateManagedApplicationsSetting();
                UpdateApplicationList();
            }
        }

        private void RunApplicationButtonClicked(object sender, EventArgs e)
        {
            if (this.appRunning)
            {
                return;
            }
            this.appRunning = true;

            try
            {
                ApplicationInfo info = (ApplicationInfo)((ImageButton)sender).Tag;

                RunApplication(info);
            }
            finally
            {
                this.appRunning = false;
            }
        }

        private void RunApplication(ApplicationInfo applicationInfo)
        {
            try
            {
                bool imageChanged = false;
                string tempImagePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".png");

                using (Bitmap source = this.EnvironmentParameters.SourceSurface.CreateAliasedBitmap())
                {
                    source.Save(tempImagePath, System.Drawing.Imaging.ImageFormat.Png);
                }

                using (Process process = new Process())
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = applicationInfo.Path,
                        Arguments = "\"" + tempImagePath + "\""
                    };

                    process.StartInfo = startInfo;
                    process.Start();

                    if (MessageBox.Show(this, Resources.EditingDone, this.Text, MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        using (FileStream stream = new FileStream(tempImagePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.DeleteOnClose))
                        using (Bitmap image = new Bitmap(stream))
                        {
                            this.surface?.Dispose();
                            this.surface = Surface.CopyFromBitmap(image);
                            imageChanged = true;
                        }
                    }
                }

                if (imageChanged)
                {
                    FinishTokenUpdate();
                }
            }
            catch (ArgumentException ex)
            {
                ShowErrorMessage(ex);
            }
            catch (IOException ex)
            {
                ShowErrorMessage(ex);
            }
            catch (Win32Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        private void UpdateApplicationList()
        {
            if (!this.backgroundWorker.IsBusy)
            {
                this.Cursor = Cursors.WaitCursor;
                this.backgroundWorker.RunWorkerAsync(this.applications);
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            List<ApplicationInfo> applicationInfo = (List<ApplicationInfo>)e.Argument;

            List<ApplicationIconInfo> icons = new List<ApplicationIconInfo>(applicationInfo.Count);

            for (int i = 0; i < applicationInfo.Count; i++)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                ApplicationInfo info = applicationInfo[i];

                try
                {
                    using (Icon icon = Icon.ExtractAssociatedIcon(info.Path))
                    {
                        if (icon != null)
                        {
                            icons.Add(new ApplicationIconInfo(info, icon.ToBitmap()));
                        }
                        else
                        {
                            icons.Add(new ApplicationIconInfo(info, SystemIcons.Application.ToBitmap()));
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                    icons.Add(new ApplicationIconInfo(info, SystemIcons.Application.ToBitmap()));
                }
            }

            e.Result = icons;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.formClosePending)
            {
                Close();
            }
            else if (!e.Cancelled)
            {
                if (e.Error != null)
                {
                    ShowErrorMessage(e.Error);
                }
                else
                {
                    List<ApplicationIconInfo> icons = (List<ApplicationIconInfo>)e.Result;

                    this.applicationIconPanel.SuspendLayout();
                    this.applicationIconPanel.Controls.Clear();

                    for (int i = 0; i < icons.Count; i++)
                    {
                        ApplicationIconInfo item = icons[i];

                        ImageButton control = new ImageButton(item.Value, item.Key)
                        {
                            BackColor = this.BackColor,
                            ForeColor = this.ForeColor
                        };
                        control.Click += RunApplicationButtonClicked;

                        this.toolTip.SetToolTip(control, item.Key.DisplayName);

                        this.applicationIconPanel.Controls.Add(control);
                    }

                    this.applicationIconPanel.ResumeLayout(true);
                }
                this.Cursor = Cursors.Default;
            }
        }

        private void UpdateManagedApplicationsSetting()
        {
            if (this.settings != null)
            {
                this.settings.Applications = new HashSet<ApplicationInfo>(this.applications);
            }
        }
    }
}
