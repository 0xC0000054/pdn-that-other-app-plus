namespace ThatOtherAppPlus
{
    partial class ThatOtherAppPlusConfigDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl = new ThatOtherAppPlus.Controls.TabControlEx();
            this.tabSelectApp = new System.Windows.Forms.TabPage();
            this.applicationIconPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.tabManageApps = new System.Windows.Forms.TabPage();
            this.buttonRemoveApplication = new System.Windows.Forms.Button();
            this.buttonAddApplication = new System.Windows.Forms.Button();
            this.listViewManageApplications = new ThatOtherAppPlus.Controls.DoubleBufferedListView();
            this.columnAppPaths = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl.SuspendLayout();
            this.tabSelectApp.SuspendLayout();
            this.tabManageApps.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonCancel.Location = new System.Drawing.Point(397, 381);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(83, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonOK.Location = new System.Drawing.Point(308, 381);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(83, 23);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Executable files | *.exe";
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabSelectApp);
            this.tabControl.Controls.Add(this.tabManageApps);
            this.tabControl.Location = new System.Drawing.Point(13, 25);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(469, 344);
            this.tabControl.TabIndex = 7;
            // 
            // tabSelectApp
            // 
            this.tabSelectApp.Controls.Add(this.applicationIconPanel);
            this.tabSelectApp.Location = new System.Drawing.Point(4, 24);
            this.tabSelectApp.Name = "tabSelectApp";
            this.tabSelectApp.Padding = new System.Windows.Forms.Padding(3);
            this.tabSelectApp.Size = new System.Drawing.Size(461, 316);
            this.tabSelectApp.TabIndex = 0;
            this.tabSelectApp.Text = "Applications";
            this.tabSelectApp.UseVisualStyleBackColor = true;
            // 
            // applicationIconPanel
            // 
            this.applicationIconPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.applicationIconPanel.Location = new System.Drawing.Point(3, 3);
            this.applicationIconPanel.Name = "applicationIconPanel";
            this.applicationIconPanel.Size = new System.Drawing.Size(455, 310);
            this.applicationIconPanel.TabIndex = 6;
            // 
            // tabManageApps
            // 
            this.tabManageApps.Controls.Add(this.buttonRemoveApplication);
            this.tabManageApps.Controls.Add(this.buttonAddApplication);
            this.tabManageApps.Controls.Add(this.listViewManageApplications);
            this.tabManageApps.Location = new System.Drawing.Point(4, 24);
            this.tabManageApps.Name = "tabManageApps";
            this.tabManageApps.Padding = new System.Windows.Forms.Padding(3);
            this.tabManageApps.Size = new System.Drawing.Size(461, 316);
            this.tabManageApps.TabIndex = 1;
            this.tabManageApps.Text = "Manage Applications";
            this.tabManageApps.UseVisualStyleBackColor = true;
            // 
            // buttonRemoveApplication
            // 
            this.buttonRemoveApplication.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonRemoveApplication.Location = new System.Drawing.Point(375, 282);
            this.buttonRemoveApplication.Name = "buttonRemoveApplication";
            this.buttonRemoveApplication.Size = new System.Drawing.Size(83, 23);
            this.buttonRemoveApplication.TabIndex = 12;
            this.buttonRemoveApplication.Text = "Remove";
            this.buttonRemoveApplication.UseVisualStyleBackColor = true;
            this.buttonRemoveApplication.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonAddApplication
            // 
            this.buttonAddApplication.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonAddApplication.Location = new System.Drawing.Point(286, 282);
            this.buttonAddApplication.Name = "buttonAddApplication";
            this.buttonAddApplication.Size = new System.Drawing.Size(83, 23);
            this.buttonAddApplication.TabIndex = 11;
            this.buttonAddApplication.Text = "Add...";
            this.buttonAddApplication.UseVisualStyleBackColor = true;
            this.buttonAddApplication.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // listViewManageApplications
            // 
            this.listViewManageApplications.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnAppPaths});
            this.listViewManageApplications.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewManageApplications.HideSelection = false;
            this.listViewManageApplications.Location = new System.Drawing.Point(3, 3);
            this.listViewManageApplications.MultiSelect = false;
            this.listViewManageApplications.Name = "listViewManageApplications";
            this.listViewManageApplications.Size = new System.Drawing.Size(455, 273);
            this.listViewManageApplications.TabIndex = 10;
            this.listViewManageApplications.UseCompatibleStateImageBehavior = false;
            this.listViewManageApplications.View = System.Windows.Forms.View.Details;
            this.listViewManageApplications.VirtualMode = true;
            this.listViewManageApplications.CacheVirtualItems += new System.Windows.Forms.CacheVirtualItemsEventHandler(this.appListView_CacheVirtualItems);
            this.listViewManageApplications.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.appListView_RetrieveVirtualItem);
            this.listViewManageApplications.SelectedIndexChanged += new System.EventHandler(this.appListView_SelectedIndexChanged);
            // 
            // columnAppPaths
            // 
            this.columnAppPaths.Text = "Applications";
            this.columnAppPaths.Width = 450;
            // 
            // ThatOtherAppPlusConfigDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(492, 414);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "ThatOtherAppPlusConfigDialog";
            this.Text = "ThatOtherAppPlus";
            this.tabControl.ResumeLayout(false);
            this.tabSelectApp.ResumeLayout(false);
            this.tabManageApps.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.FlowLayoutPanel applicationIconPanel;
        private Controls.TabControlEx tabControl;
        private System.Windows.Forms.TabPage tabSelectApp;
        private System.Windows.Forms.TabPage tabManageApps;
        private System.Windows.Forms.Button buttonAddApplication;
        private ThatOtherAppPlus.Controls.DoubleBufferedListView listViewManageApplications;
        private System.Windows.Forms.Button buttonRemoveApplication;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ColumnHeader columnAppPaths;
    }
}
