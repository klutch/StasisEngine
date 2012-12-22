﻿namespace StasisEditor.Views
{
    partial class EditorView
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
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuFile = new System.Windows.Forms.MenuItem();
            this.menuFileExit = new System.Windows.Forms.MenuItem();
            this.menuLevel = new System.Windows.Forms.MenuItem();
            this.menuLevelNew = new System.Windows.Forms.MenuItem();
            this.menuLevelLoad = new System.Windows.Forms.MenuItem();
            this.menuLevelSave = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuLevelClose = new System.Windows.Forms.MenuItem();
            this.mainSplit = new System.Windows.Forms.SplitContainer();
            this.editorTabControl = new System.Windows.Forms.TabControl();
            this.levelTab = new System.Windows.Forms.TabPage();
            this.levelView = new StasisEditor.Views.LevelView();
            this.materialsTab = new System.Windows.Forms.TabPage();
            this.materialView = new StasisEditor.Views.MaterialView();
            this.blueprintsTab = new System.Windows.Forms.TabPage();
            this.blueprintView = new StasisEditor.Views.BlueprintView();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplit)).BeginInit();
            this.mainSplit.Panel2.SuspendLayout();
            this.mainSplit.SuspendLayout();
            this.editorTabControl.SuspendLayout();
            this.levelTab.SuspendLayout();
            this.materialsTab.SuspendLayout();
            this.blueprintsTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFile,
            this.menuLevel});
            // 
            // menuFile
            // 
            this.menuFile.Index = 0;
            this.menuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFileExit});
            this.menuFile.Text = "File";
            // 
            // menuFileExit
            // 
            this.menuFileExit.Index = 0;
            this.menuFileExit.Text = "Exit";
            this.menuFileExit.Click += new System.EventHandler(this.menuFileExit_Click);
            // 
            // menuLevel
            // 
            this.menuLevel.Index = 1;
            this.menuLevel.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuLevelNew,
            this.menuLevelLoad,
            this.menuLevelSave,
            this.menuItem2,
            this.menuLevelClose});
            this.menuLevel.Text = "Level";
            // 
            // menuLevelNew
            // 
            this.menuLevelNew.Index = 0;
            this.menuLevelNew.Text = "New";
            this.menuLevelNew.Click += new System.EventHandler(this.menuLevelNew_Click);
            // 
            // menuLevelLoad
            // 
            this.menuLevelLoad.Index = 1;
            this.menuLevelLoad.Text = "Load";
            // 
            // menuLevelSave
            // 
            this.menuLevelSave.Enabled = false;
            this.menuLevelSave.Index = 2;
            this.menuLevelSave.Text = "Save";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 3;
            this.menuItem2.Text = "-";
            // 
            // menuLevelClose
            // 
            this.menuLevelClose.Enabled = false;
            this.menuLevelClose.Index = 4;
            this.menuLevelClose.Text = "Close";
            this.menuLevelClose.Click += new System.EventHandler(this.menuLevelClose_Click);
            // 
            // mainSplit
            // 
            this.mainSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mainSplit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mainSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.mainSplit.Location = new System.Drawing.Point(0, 0);
            this.mainSplit.Name = "mainSplit";
            // 
            // mainSplit.Panel1
            // 
            this.mainSplit.Panel1.BackColor = System.Drawing.SystemColors.Control;
            // 
            // mainSplit.Panel2
            // 
            this.mainSplit.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.mainSplit.Panel2.Controls.Add(this.editorTabControl);
            this.mainSplit.Size = new System.Drawing.Size(1004, 569);
            this.mainSplit.SplitterDistance = 261;
            this.mainSplit.TabIndex = 1;
            this.mainSplit.TabStop = false;
            // 
            // editorTabControl
            // 
            this.editorTabControl.Controls.Add(this.levelTab);
            this.editorTabControl.Controls.Add(this.materialsTab);
            this.editorTabControl.Controls.Add(this.blueprintsTab);
            this.editorTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editorTabControl.Location = new System.Drawing.Point(0, 0);
            this.editorTabControl.Margin = new System.Windows.Forms.Padding(0);
            this.editorTabControl.Name = "editorTabControl";
            this.editorTabControl.SelectedIndex = 0;
            this.editorTabControl.Size = new System.Drawing.Size(737, 567);
            this.editorTabControl.TabIndex = 0;
            // 
            // levelTab
            // 
            this.levelTab.BackColor = System.Drawing.SystemColors.ControlDark;
            this.levelTab.Controls.Add(this.levelView);
            this.levelTab.Location = new System.Drawing.Point(4, 22);
            this.levelTab.Name = "levelTab";
            this.levelTab.Size = new System.Drawing.Size(729, 541);
            this.levelTab.TabIndex = 0;
            this.levelTab.Text = "Level";
            // 
            // levelView
            // 
            this.levelView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.levelView.Location = new System.Drawing.Point(0, 0);
            this.levelView.Margin = new System.Windows.Forms.Padding(0);
            this.levelView.Name = "levelView";
            this.levelView.Size = new System.Drawing.Size(729, 541);
            this.levelView.TabIndex = 0;
            // 
            // materialsTab
            // 
            this.materialsTab.Controls.Add(this.materialView);
            this.materialsTab.Location = new System.Drawing.Point(4, 22);
            this.materialsTab.Margin = new System.Windows.Forms.Padding(0);
            this.materialsTab.Name = "materialsTab";
            this.materialsTab.Size = new System.Drawing.Size(729, 541);
            this.materialsTab.TabIndex = 1;
            this.materialsTab.Text = "Materials";
            this.materialsTab.UseVisualStyleBackColor = true;
            // 
            // materialView
            // 
            this.materialView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialView.Location = new System.Drawing.Point(0, 0);
            this.materialView.Margin = new System.Windows.Forms.Padding(0);
            this.materialView.Name = "materialView";
            this.materialView.Size = new System.Drawing.Size(729, 541);
            this.materialView.TabIndex = 0;
            // 
            // blueprintsTab
            // 
            this.blueprintsTab.Controls.Add(this.blueprintView);
            this.blueprintsTab.Location = new System.Drawing.Point(4, 22);
            this.blueprintsTab.Margin = new System.Windows.Forms.Padding(0);
            this.blueprintsTab.Name = "blueprintsTab";
            this.blueprintsTab.Size = new System.Drawing.Size(729, 541);
            this.blueprintsTab.TabIndex = 2;
            this.blueprintsTab.Text = "Blueprints";
            this.blueprintsTab.UseVisualStyleBackColor = true;
            // 
            // blueprintView
            // 
            this.blueprintView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blueprintView.Location = new System.Drawing.Point(0, 0);
            this.blueprintView.Margin = new System.Windows.Forms.Padding(0);
            this.blueprintView.Name = "blueprintView";
            this.blueprintView.Size = new System.Drawing.Size(729, 541);
            this.blueprintView.TabIndex = 0;
            // 
            // EditorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 569);
            this.Controls.Add(this.mainSplit);
            this.Menu = this.mainMenu1;
            this.Name = "EditorView";
            this.Text = "Stasis Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EditorForm_Closed);
            this.mainSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplit)).EndInit();
            this.mainSplit.ResumeLayout(false);
            this.editorTabControl.ResumeLayout(false);
            this.levelTab.ResumeLayout(false);
            this.materialsTab.ResumeLayout(false);
            this.blueprintsTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer mainSplit;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuFile;
        private System.Windows.Forms.MenuItem menuFileExit;
        private System.Windows.Forms.MenuItem menuLevel;
        private System.Windows.Forms.MenuItem menuLevelNew;
        private System.Windows.Forms.MenuItem menuLevelLoad;
        private System.Windows.Forms.MenuItem menuLevelSave;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuLevelClose;
        private System.Windows.Forms.TabControl editorTabControl;
        private System.Windows.Forms.TabPage levelTab;
        private System.Windows.Forms.TabPage materialsTab;
        private MaterialView materialView;
        private LevelView levelView;
        private System.Windows.Forms.TabPage blueprintsTab;
        private BlueprintView blueprintView;

    }
}