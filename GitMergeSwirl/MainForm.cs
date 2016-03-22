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
            runner = new MainRunner();

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
            runner.SyncGitAndDBToMemory();
            btnRefreshBranchList_Click(null, null);
        }

        private void btnRefreshBranchList_Click(object sender, EventArgs e)
        {
            dataGridViewBranches.DataSource = runner.WorkingBranchList;
            Console.WriteLine($"Refresh List #:{runner.WorkingBranchList.Count}");
        }

        private void btnSycnGitBranchesToDB_Click(object sender, EventArgs e)
        {
            runner.SyncInMemoryResultsToDB();
        }

        private async void bntFindParentBranch_Click(object sender, EventArgs e)
        {

            for (int index = 0; index < dataGridViewBranches.SelectedRows.Count; index++)
            {
                var selectedRow = dataGridViewBranches.SelectedRows[index];
                var branch = (DataModel.Branch)selectedRow.DataBoundItem;
                await runner.FindBranchParents(branch);
            }
        }

        #endregion


        #region TabPagParentBranchStatus

        private async void btnFindParentBranch_Click(object sender, EventArgs e)
        {
            foreach (var branch in runner.WorkingBranchList)
            {
                await runner.FindBranchParents(branch);
            }


            btnShowParentBranch_Click(null, null);

        }
        #endregion

        private void btnShowParentBranch_Click(object sender, EventArgs e)
        {
            // build a list from the working branch list. 
            // Would love to use a tree view (soon)
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

        private void btnSetParnetBranch_Click(object sender, EventArgs e)
        {
            runner.SetParentBranch();
        }
    }

    public class ControlWriter : TextWriter
    {
        private readonly StringBuilder buffer;
        private readonly TextBox textbox;
        public ControlWriter(TextBox textbox)
        {
            buffer = new StringBuilder();
            this.textbox = textbox;
        }

        public override void Write(char value)
        {
            // Cheap shot to only write when the string ends
            // Note should check last char for \r and current char for \n, but just \r work for this program
            if (value == '\r')
            {
                textbox.AppendText(buffer + Environment.NewLine);
                buffer.Clear();
            }
            else
            {
                buffer.Append(value);
            }
        }

        public override void Write(string value) // method is not used by this program
        {
            textbox.Text += value;
        }

        public override Encoding Encoding => Encoding.ASCII;
    }
}
