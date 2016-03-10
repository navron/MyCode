﻿namespace DevOps.GitMergeSwirl
{
    partial class MainForm
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
            this.tbLog = new System.Windows.Forms.TextBox();
            this.btnCheckGitBranches = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCheckReleaseBranchState = new System.Windows.Forms.Button();
            this.btnCheckBranchAheads = new System.Windows.Forms.Button();
            this.btnSaveGitInfoToDB = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageGitBranches = new System.Windows.Forms.TabPage();
            this.dataGridViewBranches = new System.Windows.Forms.DataGridView();
            this.tabPageMergeTests = new System.Windows.Forms.TabPage();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.btnRunCommand = new System.Windows.Forms.Button();
            this.tbRunCommand = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.tbDatabaseServer = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelDBServer = new System.Windows.Forms.Label();
            this.labelUserName = new System.Windows.Forms.Label();
            this.gridViewPrivateParentBranch = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.btnFindParentBranch = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageGitBranches.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBranches)).BeginInit();
            this.tabPageMergeTests.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewPrivateParentBranch)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbLog
            // 
            this.tbLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tbLog.Location = new System.Drawing.Point(0, 571);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(1239, 251);
            this.tbLog.TabIndex = 0;
            // 
            // btnCheckGitBranches
            // 
            this.btnCheckGitBranches.Location = new System.Drawing.Point(12, 12);
            this.btnCheckGitBranches.Name = "btnCheckGitBranches";
            this.btnCheckGitBranches.Size = new System.Drawing.Size(120, 23);
            this.btnCheckGitBranches.TabIndex = 1;
            this.btnCheckGitBranches.Text = "Check Git Branches";
            this.btnCheckGitBranches.UseVisualStyleBackColor = true;
            this.btnCheckGitBranches.Click += new System.EventHandler(this.btnCheckGitBranches_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClear.Location = new System.Drawing.Point(1142, 791);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 3;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCheckReleaseBranchState);
            this.panel1.Controls.Add(this.btnCheckBranchAheads);
            this.panel1.Controls.Add(this.btnSaveGitInfoToDB);
            this.panel1.Controls.Add(this.btnCheckGitBranches);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1225, 46);
            this.panel1.TabIndex = 6;
            // 
            // btnCheckReleaseBranchState
            // 
            this.btnCheckReleaseBranchState.Location = new System.Drawing.Point(138, 12);
            this.btnCheckReleaseBranchState.Name = "btnCheckReleaseBranchState";
            this.btnCheckReleaseBranchState.Size = new System.Drawing.Size(187, 23);
            this.btnCheckReleaseBranchState.TabIndex = 8;
            this.btnCheckReleaseBranchState.Text = "Check Release State Order";
            this.btnCheckReleaseBranchState.UseVisualStyleBackColor = true;
            // 
            // btnCheckBranchAheads
            // 
            this.btnCheckBranchAheads.Location = new System.Drawing.Point(331, 12);
            this.btnCheckBranchAheads.Name = "btnCheckBranchAheads";
            this.btnCheckBranchAheads.Size = new System.Drawing.Size(126, 23);
            this.btnCheckBranchAheads.TabIndex = 7;
            this.btnCheckBranchAheads.Text = "Check Ahead Behinds";
            this.btnCheckBranchAheads.UseVisualStyleBackColor = true;
            this.btnCheckBranchAheads.Click += new System.EventHandler(this.btnCheckBranchAheads_Click);
            // 
            // btnSaveGitInfoToDB
            // 
            this.btnSaveGitInfoToDB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveGitInfoToDB.Location = new System.Drawing.Point(1135, 12);
            this.btnSaveGitInfoToDB.Name = "btnSaveGitInfoToDB";
            this.btnSaveGitInfoToDB.Size = new System.Drawing.Size(75, 23);
            this.btnSaveGitInfoToDB.TabIndex = 6;
            this.btnSaveGitInfoToDB.Text = "Sycn To DB";
            this.btnSaveGitInfoToDB.UseVisualStyleBackColor = true;
            this.btnSaveGitInfoToDB.Click += new System.EventHandler(this.buttonSycnBranches_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageGitBranches);
            this.tabControl.Controls.Add(this.tabPageMergeTests);
            this.tabControl.Controls.Add(this.tabPageSettings);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1239, 571);
            this.tabControl.TabIndex = 8;
            // 
            // tabPageGitBranches
            // 
            this.tabPageGitBranches.Controls.Add(this.dataGridViewBranches);
            this.tabPageGitBranches.Controls.Add(this.panel1);
            this.tabPageGitBranches.Location = new System.Drawing.Point(4, 22);
            this.tabPageGitBranches.Name = "tabPageGitBranches";
            this.tabPageGitBranches.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGitBranches.Size = new System.Drawing.Size(1231, 545);
            this.tabPageGitBranches.TabIndex = 0;
            this.tabPageGitBranches.Text = "Git Branches";
            this.tabPageGitBranches.UseVisualStyleBackColor = true;
            // 
            // dataGridViewBranches
            // 
            this.dataGridViewBranches.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBranches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewBranches.Location = new System.Drawing.Point(3, 49);
            this.dataGridViewBranches.Name = "dataGridViewBranches";
            this.dataGridViewBranches.Size = new System.Drawing.Size(1225, 493);
            this.dataGridViewBranches.TabIndex = 8;
            // 
            // tabPageMergeTests
            // 
            this.tabPageMergeTests.Controls.Add(this.gridViewPrivateParentBranch);
            this.tabPageMergeTests.Controls.Add(this.panel2);
            this.tabPageMergeTests.Location = new System.Drawing.Point(4, 22);
            this.tabPageMergeTests.Name = "tabPageMergeTests";
            this.tabPageMergeTests.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMergeTests.Size = new System.Drawing.Size(1231, 545);
            this.tabPageMergeTests.TabIndex = 1;
            this.tabPageMergeTests.Text = "Merge Status";
            this.tabPageMergeTests.UseVisualStyleBackColor = true;
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.Controls.Add(this.btnRunCommand);
            this.tabPageSettings.Controls.Add(this.tbRunCommand);
            this.tabPageSettings.Controls.Add(this.textBox3);
            this.tabPageSettings.Controls.Add(this.tbDatabaseServer);
            this.tabPageSettings.Controls.Add(this.textBox1);
            this.tabPageSettings.Controls.Add(this.label3);
            this.tabPageSettings.Controls.Add(this.label4);
            this.tabPageSettings.Controls.Add(this.labelDBServer);
            this.tabPageSettings.Controls.Add(this.labelUserName);
            this.tabPageSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings.Size = new System.Drawing.Size(1231, 545);
            this.tabPageSettings.TabIndex = 2;
            this.tabPageSettings.Text = "Setting";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // btnRunCommand
            // 
            this.btnRunCommand.Location = new System.Drawing.Point(498, 257);
            this.btnRunCommand.Name = "btnRunCommand";
            this.btnRunCommand.Size = new System.Drawing.Size(75, 23);
            this.btnRunCommand.TabIndex = 8;
            this.btnRunCommand.Text = "Run";
            this.btnRunCommand.UseVisualStyleBackColor = true;
            // 
            // tbRunCommand
            // 
            this.tbRunCommand.Location = new System.Drawing.Point(96, 259);
            this.tbRunCommand.Name = "tbRunCommand";
            this.tbRunCommand.Size = new System.Drawing.Size(396, 20);
            this.tbRunCommand.TabIndex = 7;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(168, 122);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 6;
            // 
            // tbDatabaseServer
            // 
            this.tbDatabaseServer.Location = new System.Drawing.Point(168, 83);
            this.tbDatabaseServer.Name = "tbDatabaseServer";
            this.tbDatabaseServer.Size = new System.Drawing.Size(100, 20);
            this.tbDatabaseServer.TabIndex = 5;
            this.tbDatabaseServer.Text = "localhost";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(168, 49);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 262);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Command:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "label4";
            // 
            // labelDBServer
            // 
            this.labelDBServer.AutoSize = true;
            this.labelDBServer.Location = new System.Drawing.Point(34, 83);
            this.labelDBServer.Name = "labelDBServer";
            this.labelDBServer.Size = new System.Drawing.Size(56, 13);
            this.labelDBServer.TabIndex = 1;
            this.labelDBServer.Text = "DB Server";
            // 
            // labelUserName
            // 
            this.labelUserName.AutoSize = true;
            this.labelUserName.Location = new System.Drawing.Point(34, 49);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(99, 13);
            this.labelUserName.TabIndex = 0;
            this.labelUserName.Text = "Domain User Name";
            // 
            // gridViewPrivateParentBranch
            // 
            this.gridViewPrivateParentBranch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridViewPrivateParentBranch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridViewPrivateParentBranch.Location = new System.Drawing.Point(3, 49);
            this.gridViewPrivateParentBranch.Name = "gridViewPrivateParentBranch";
            this.gridViewPrivateParentBranch.Size = new System.Drawing.Size(1225, 493);
            this.gridViewPrivateParentBranch.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.btnFindParentBranch);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1225, 46);
            this.panel2.TabIndex = 9;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(1135, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "Sycn To DB";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // btnFindParentBranch
            // 
            this.btnFindParentBranch.Location = new System.Drawing.Point(12, 12);
            this.btnFindParentBranch.Name = "btnFindParentBranch";
            this.btnFindParentBranch.Size = new System.Drawing.Size(120, 23);
            this.btnFindParentBranch.TabIndex = 1;
            this.btnFindParentBranch.Text = "Find Parent Branch";
            this.btnFindParentBranch.UseVisualStyleBackColor = true;
            this.btnFindParentBranch.Click += new System.EventHandler(this.btnFindParentBranch_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1239, 822);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.tbLog);
            this.Name = "MainForm";
            this.Text = "Git Merge Swirl";
            this.panel1.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageGitBranches.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBranches)).EndInit();
            this.tabPageMergeTests.ResumeLayout(false);
            this.tabPageSettings.ResumeLayout(false);
            this.tabPageSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewPrivateParentBranch)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Button btnCheckGitBranches;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSaveGitInfoToDB;
        private System.Windows.Forms.Button btnCheckBranchAheads;
        private System.Windows.Forms.Button btnCheckReleaseBranchState;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageGitBranches;
        private System.Windows.Forms.TabPage tabPageMergeTests;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.TextBox tbRunCommand;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox tbDatabaseServer;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelDBServer;
        private System.Windows.Forms.Label labelUserName;
        private System.Windows.Forms.Button btnRunCommand;
        private System.Windows.Forms.DataGridView dataGridViewBranches;
        private System.Windows.Forms.DataGridView gridViewPrivateParentBranch;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnFindParentBranch;
    }
}

