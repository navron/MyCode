using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DevOps.GitMergeSwirl
{
    public partial class MainForm : Form
    {
        private readonly MainRunner runner;

        public MainForm()
        {
            InitializeComponent();
            runner = MainRunner.Instance;

            // This show all the column names
            dataGridViewBranches.DataSource = runner.WorkingBranchList;
            gridViewPrivateParentBranch.DataSource = runner.WorkingBranchToParentList;

            Console.SetOut(new ControlWriter(tbLog));
        }


        private void buttonClear_Click(object sender, EventArgs e)
        {
            tbLog.Text = string.Empty;
        }

        #region TabPageGitBranches

        private void btnCheckGitBranches_Click(object sender, EventArgs e)
        {
            MainRunner.Instance.SyncGitAndDBToMemory();
            btnRefreshBranchList_Click(null, null);
        }

        private void btnRefreshBranchList_Click(object sender, EventArgs e)
        {
            dataGridViewBranches.DataSource = runner.WorkingBranchList;
        }

        private void btnSycnGitBranchesToDB_Click(object sender, EventArgs e)
        {
            runner.SyncInMemoryResultsToDB();
        }

        #endregion


        #region TabPagParentBranchStatus

        private void btnFindParentBranch_Click(object sender, EventArgs e)
        {
            runner.FindBranchParents();
            gridViewPrivateParentBranch.DataSource = runner.WorkingBranchToParentList;
        }
        #endregion

        private void buttonSHowBranchs_Click(object sender, EventArgs e)
        {
            //foreach (var b in MainRunner.Instance.TheList)
            //{
            //    var t = $"R:{b.BranchType} Name:{b.CanonicalName}";
            //    tbLog.AppendText(t + Environment.NewLine);
            //}
        }




    }

    public class ControlWriter : TextWriter
    {
        private readonly Control textbox;
        public ControlWriter(Control textbox)
        {
            this.textbox = textbox;
        }

        public override void Write(char value)
        {
            textbox.Text += value;
        }

        public override void Write(string value)
        {
            textbox.Text += value;
        }

        public override Encoding Encoding => Encoding.ASCII;
    }
}
