using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace DevOps.GitMergeSwirl
{
    public partial class MainForm : Form
    {
        private GitRepo repo;
        private Config config;

        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonSetup_Click(object sender, EventArgs e)
        {
            config = new Config();
            repo = new GitRepo(config);
        }

        private void buttonGitBranches_Click(object sender, EventArgs e)
        {
           var i = repo.GetBranches();
            tbLog.AppendText($"Branch Count{i}");
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            tbLog.Text = string.Empty;
        }

        private void buttonSHowBranchs_Click(object sender, EventArgs e)
        {
            foreach (var b in MainRunner.Instance.TheList)
            {
                var t = $"R:{b.BranchType} Name:{b.CanonicalName}";
                tbLog.AppendText(t + Environment.NewLine);
            }
        }

        private void buttonCreateDB_Click(object sender, EventArgs e)
        {
           
        }

        private void buttonSycnBranches_Click(object sender, EventArgs e)
        {
            var watch = Stopwatch.StartNew();
            MainRunner.Instance.SyncGitToDB();
            watch.Stop();
            tbLog.AppendText($"Find Done in {watch.Elapsed}{Environment.NewLine}");

        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            var watch = Stopwatch.StartNew();
            // the code that you want to measure comes here
            
            MainRunner.Instance.FindReleaseParents();
            watch.Stop();
            tbLog.AppendText($"Find Done in {watch.Elapsed}{Environment.NewLine}");
        }
    }
}
