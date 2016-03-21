using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using LibGit2Sharp;

namespace DevOps.GitMergeSwirl
{
    public class GitRepo
    {

        private Repository repo;


        //private Dictionary<int, int> existingProjects; //key to id map
        //    public List<SXBranch> Branches;
        //        private HashSet<string> existingCommits;

        public GitRepo(Config config)
        {
            try
            {
                repo = new Repository(config.gitRepoPath);
            }
            catch (Exception)
            {
                Console.WriteLine($"ERROR: Can not read Git Repository, Check that {config.gitRepoPath} exists");
                throw;
            }
        }

        public List<DataModel.Branch> GetBranches()
        {
            var gb = new List<DataModel.Branch>();
            foreach (var branch in repo.Branches)
            {
                var b = new DataModel.Branch
                {
                    CanonicalName = branch.CanonicalName,
                    GitCommit = branch.Tip,
                    GitBranch = branch
                };
                // Last Commit
                b.ShaTip = b.GitCommit.Sha;

                b.BranchType = DataModel.SetBranchType(b.CanonicalName);

                gb.Add(b);
                //          Branches.Add(new SXBranch(branch));
                //var temp = branch.CanonicalName;
                //var t2 = branch.FriendlyName;
                //var t3 = branch.Tip;
                //var id = t3.Sha;
                //var t = t3.Tree;
                //var c = branch.Commits;
            }

            // limit to only Release and Private for now
            var wantedBranches = gb.Where(s => s.BranchType == DataModel.BranchType.Private || s.BranchType == DataModel.BranchType.Release).ToList();

            return wantedBranches;
        }

        public void GetReleaseBranchParentForPrivateBranch(Commit commit, List<Commit> releasesCommits)
        {
            // repo.ObjectDatabase.FindMergeBase(commit, releasesCommits);

            releasesCommits.Add(commit);
            var c = repo.ObjectDatabase.FindMergeBase(releasesCommits, MergeBaseFindingStrategy.Octopus);
            //    foreach (SXBranch sxBranch in Branches)
            {

            }
        }

        public void FindReleaseParentForPrivateBranch(DataModel.Branch privateBranch, List<DataModel.Branch> releaseBranches)
        {
            // This funcations takes time .. limit usage
            // I also want the as Async await

           
        //    var commitLog = repo.Commits;

            if (privateBranch.ToReleaseBranch == null)
            {
                privateBranch.ToReleaseBranch = new List<DataModel.PrivateBranchToReleaseBranchMapping>();
            }

            // Test for each branch in the test set (All Release branches)
            foreach (var releaseBranch in releaseBranches)
            {

                var test = privateBranch.ToReleaseBranch.FirstOrDefault(r => r.ReleaseBranchCanonicalName == releaseBranch.CanonicalName);
                if (test != null)
                {
                    if (test.PrivateBranchSha == privateBranch.ShaTip &&
                        test.ReleaseBranchSha == releaseBranch.ShaTip &&
                        test.Ahead != null &&
                        test.Behind != null)
                    {
                        continue; // No need to recheck again
                    }
                }
                else
                {
                    test = new DataModel.PrivateBranchToReleaseBranchMapping
                    {
                        //   BaseCommit = repo.ObjectDatabase.FindMergeBase(privateBranch.GitCommit, releaseBranch.GitCommit),
                        PrivateBranchCanonicalName = privateBranch.CanonicalName,
                        ReleaseBranchCanonicalName = releaseBranch.CanonicalName,
                        PrivateBranchSha = privateBranch.ShaTip,
                        ReleaseBranchSha = releaseBranch.ShaTip,
                        BranchType = DataModel.SetBranchType(privateBranch.CanonicalName)
                    };
                }

                test.RecordUpdate = true;

                var pBranch = privateBranch.GitBranch;

                // This Test May not be correct,  TODO Test what should be done here.
                // var div = repo.ObjectDatabase.CalculateHistoryDivergence(pBranch.Tip, b.BaseCommit);
                var div = repo.ObjectDatabase.CalculateHistoryDivergence(pBranch.Tip, releaseBranch.GitCommit);
                test.Ahead = div.AheadBy;
                test.Behind = div.BehindBy;


                // Testing if we should look at the base commit instead
                test.BaseCommit = repo.ObjectDatabase.FindMergeBase(privateBranch.GitCommit, releaseBranch.GitCommit);
                test.BaseCommitSha = test.BaseCommit.Sha;
                var divbase = repo.ObjectDatabase.CalculateHistoryDivergence(pBranch.Tip, test.BaseCommit);
                test.BaseAhead = divbase.AheadBy;
                test.BaseBehind = divbase.BehindBy;
                privateBranch.ToReleaseBranch.Add(test);
            }
        }
    }




    //public class SXBranch
    //{
    //    public BranchType BranchType { get; private set; }

    //    public Branch Branch { get; private set; }
    //    public string FriendlyName { get; private set; }
    //    public string Sha { get; private set; }


    //    public SXBranch(Branch branch)
    //    {
    //        Branch = branch;
    //        FriendlyName = branch.FriendlyName;
    //        var lastcommit = branch.Tip;
    //        Sha = lastcommit.Sha;

    //        //var r = new Regex(@"*origin/releases//*");
    //        //var m =  r.Match(FriendlyName);
    //        // Note this is done on the meta data so use /heads instead of /origin
    //        if (branch.CanonicalName.Contains(@"/heads/private/")) BranchType = BranchType.Private;
    //        else if (branch.CanonicalName.Contains(@"/heads/releases/")) BranchType = BranchType.Release;
    //        else BranchType = BranchType.Other;

    //    }
    //}
}
