using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace DevOps.GitMergeSwirl
{
    public partial class MainForm : Form
    {
        private MainRunner runner;
        public MainForm()
        {
            InitializeComponent();
            runner = MainRunner.Instance;
            dataGridViewBranches.DataSource = runner.WorkingBranchList;
        }

        private void btnCheckGitBranches_Click(object sender, EventArgs e)
        {
            var watch = Stopwatch.StartNew();
            MainRunner.Instance.SyncGitAndDBToMemory();
            watch.Stop();
            tbLog.AppendText($"Job SyncGitAndDBToMemory: in {watch.Elapsed}{Environment.NewLine}");

            dataGridViewBranches.DataSource = runner.WorkingBranchList;
            tbLog.AppendText($"WorkingBranchList Count{runner.WorkingBranchList.Count}{Environment.NewLine}");
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            tbLog.Text = string.Empty;
        }

        private void buttonSHowBranchs_Click(object sender, EventArgs e)
        {
            //foreach (var b in MainRunner.Instance.TheList)
            //{
            //    var t = $"R:{b.BranchType} Name:{b.CanonicalName}";
            //    tbLog.AppendText(t + Environment.NewLine);
            //}
        }

        private void buttonSycnBranches_Click(object sender, EventArgs e)
        {
            var watch = Stopwatch.StartNew();
            //   MainRunner.Instance.SyncGitToDB();
            watch.Stop();
            tbLog.AppendText($"Find Done in {watch.Elapsed}{Environment.NewLine}");

        }

        private void btnCheckBranchAheads_Click(object sender, EventArgs e)
        {
            var watch = Stopwatch.StartNew();
            runner.FindBranchParents();
            watch.Stop();
            tbLog.AppendText($"Find Branch Parents: in {watch.Elapsed}{Environment.NewLine}");
        }

        private void btnFindParentBranch_Click(object sender, EventArgs e)
        {
            var watch = Stopwatch.StartNew();
            runner.FindBranchParents();
            watch.Stop();
            tbLog.AppendText($"Find Parent Branches: in {watch.Elapsed}{Environment.NewLine}");
        }
    }
}
