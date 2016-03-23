using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            // Make sure that the runner object is disposed
            FormClosed += (object sender, FormClosedEventArgs e) => { runner.Dispose(); };

            // This show all the column names with no data
            gridViewBranches.DataSource = runner.WorkingBranchList;
            gridViewPrivateParentBranch.DataSource = new List<DataModel.PrivateToReleaseMapping>();

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
            gridViewBranches.DataSource = runner.WorkingBranchList;
            Console.WriteLine($"Refresh List #:{runner.WorkingBranchList.Count}");
        }

        private void btnSycnGitBranchesToDB_Click(object sender, EventArgs e)
        {
            runner.SyncInMemoryResultsToDB();
        }

        private async void bntFindParentBranch_Click(object sender, EventArgs e)
        {
            for (int index = 0; index < gridViewBranches.SelectedRows.Count; index++)
            {
                var selectedRow = gridViewBranches.SelectedRows[index];
                var branch = (DataModel.Branch)selectedRow.DataBoundItem;
                await runner.FindBranchParents(branch);
            }
        }

        private void btnSetParnetBranch_Click(object sender, EventArgs e)
        {
            runner.SetParentBranch();
        }


        #endregion

        #region TabPagParentBranchStatus

        private async void btnFindParentBranch_Click(object sender, EventArgs e)
        {
            var watch = Stopwatch.StartNew();
            foreach (var branch in runner.WorkingBranchList)
            {
                await runner.FindBranchParents(branch);
            }
            btnShowParentBranch_Click(null, null);
            Console.WriteLine($"FindReleaseParentForPrivateBranch:{watch.Elapsed} Count:{runner.WorkingBranchList.Count}");
        }
        

        private void btnShowParentBranch_Click(object sender, EventArgs e)
        {
            // build a list from the working branch list. 
            // Would love to use a tree view (soon)
            List<DataModel.PrivateToReleaseMapping> parentList = new List<DataModel.PrivateToReleaseMapping>();

            foreach (var branch in runner.WorkingBranchList)
            {
                if (branch.ToReleaseBranch != null)
                {
                    parentList.AddRange(branch.ToReleaseBranch);
                }
            }
            gridViewPrivateParentBranch.DataSource = parentList;
        }

        #endregion
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
