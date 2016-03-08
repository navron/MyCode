namespace DevOps.GitMergeSwirl
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
            this.buttonSetup = new System.Windows.Forms.Button();
            this.buttonGitBranches = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonSHowBranchs = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonSycnBranches = new System.Windows.Forms.Button();
            this.buttonCreateDB = new System.Windows.Forms.Button();
            this.buttonFind = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbLog
            // 
            this.tbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLog.Location = new System.Drawing.Point(0, 0);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(771, 507);
            this.tbLog.TabIndex = 0;
            // 
            // buttonSetup
            // 
            this.buttonSetup.Location = new System.Drawing.Point(12, 12);
            this.buttonSetup.Name = "buttonSetup";
            this.buttonSetup.Size = new System.Drawing.Size(75, 23);
            this.buttonSetup.TabIndex = 1;
            this.buttonSetup.Text = "Setup DB ";
            this.buttonSetup.UseVisualStyleBackColor = true;
            this.buttonSetup.Click += new System.EventHandler(this.buttonSetup_Click);
            // 
            // buttonGitBranches
            // 
            this.buttonGitBranches.Location = new System.Drawing.Point(93, 12);
            this.buttonGitBranches.Name = "buttonGitBranches";
            this.buttonGitBranches.Size = new System.Drawing.Size(75, 23);
            this.buttonGitBranches.TabIndex = 2;
            this.buttonGitBranches.Text = "Get Branch from Repo";
            this.buttonGitBranches.UseVisualStyleBackColor = true;
            this.buttonGitBranches.Click += new System.EventHandler(this.buttonGitBranches_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClear.Location = new System.Drawing.Point(660, 74);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 3;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // buttonSHowBranchs
            // 
            this.buttonSHowBranchs.Location = new System.Drawing.Point(174, 12);
            this.buttonSHowBranchs.Name = "buttonSHowBranchs";
            this.buttonSHowBranchs.Size = new System.Drawing.Size(75, 23);
            this.buttonSHowBranchs.TabIndex = 4;
            this.buttonSHowBranchs.Text = "SHow Branchs";
            this.buttonSHowBranchs.UseVisualStyleBackColor = true;
            this.buttonSHowBranchs.Click += new System.EventHandler(this.buttonSHowBranchs_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonFind);
            this.panel1.Controls.Add(this.buttonSycnBranches);
            this.panel1.Controls.Add(this.buttonCreateDB);
            this.panel1.Controls.Add(this.buttonGitBranches);
            this.panel1.Controls.Add(this.buttonClear);
            this.panel1.Controls.Add(this.buttonSHowBranchs);
            this.panel1.Controls.Add(this.buttonSetup);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(771, 100);
            this.panel1.TabIndex = 6;
            // 
            // buttonSycnBranches
            // 
            this.buttonSycnBranches.Location = new System.Drawing.Point(280, 12);
            this.buttonSycnBranches.Name = "buttonSycnBranches";
            this.buttonSycnBranches.Size = new System.Drawing.Size(75, 23);
            this.buttonSycnBranches.TabIndex = 6;
            this.buttonSycnBranches.Text = "Sycn DB";
            this.buttonSycnBranches.UseVisualStyleBackColor = true;
            this.buttonSycnBranches.Click += new System.EventHandler(this.buttonSycnBranches_Click);
            // 
            // buttonCreateDB
            // 
            this.buttonCreateDB.Location = new System.Drawing.Point(361, 12);
            this.buttonCreateDB.Name = "buttonCreateDB";
            this.buttonCreateDB.Size = new System.Drawing.Size(75, 23);
            this.buttonCreateDB.TabIndex = 5;
            this.buttonCreateDB.Text = "Create DB";
            this.buttonCreateDB.UseVisualStyleBackColor = true;
            this.buttonCreateDB.Click += new System.EventHandler(this.buttonCreateDB_Click);
            // 
            // buttonFind
            // 
            this.buttonFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFind.Location = new System.Drawing.Point(442, 12);
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(75, 23);
            this.buttonFind.TabIndex = 7;
            this.buttonFind.Text = "Find";
            this.buttonFind.UseVisualStyleBackColor = true;
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 507);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Button buttonSetup;
        private System.Windows.Forms.Button buttonGitBranches;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonSHowBranchs;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonCreateDB;
        private System.Windows.Forms.Button buttonSycnBranches;
        private System.Windows.Forms.Button buttonFind;
    }
}

