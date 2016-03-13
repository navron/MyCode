using System;
using System.Collections.Generic;
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

            // This show all the column names with no data
            dataGridViewBranches.DataSource = runner.WorkingBranchList;
            gridViewPrivateParentBranch.DataSource = new List<DataModel.PrivateBranchToReleaseBranchMapping>();

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

        private void bntFindParentBranch_Click(object sender, EventArgs e)
        {

            for (int index = 0; index < dataGridViewBranches.SelectedRows.Count; index++)
            {
                var selectedRow = dataGridViewBranches.SelectedRows[index];
                var branch = (DataModel.Branch)selectedRow.DataBoundItem;
                runner.FindBranchParents(branch);
            }
        }

        #endregion


        #region TabPagParentBranchStatus

        private void btnFindParentBranch_Click(object sender, EventArgs e)
        {
            foreach (var branch in runner.WorkingBranchList)
            {
                runner.FindBranchParents(branch);
            }
            

            btnShowParentBranch_Click(null, null);

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



        private void btnShowParentBranch_Click(object sender, EventArgs e)
        {
            List<DataModel.PrivateBranchToReleaseBranchMapping> parentList = new List<DataModel.PrivateBranchToReleaseBranchMapping>();

            foreach (var branch in runner.WorkingBranchList)
            {
                if (branch.ToReleaseBranch != null)
                {
                    parentList.AddRange(branch.ToReleaseBranch);
                }
            }


            gridViewPrivateParentBranch.DataSource = parentList;
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
