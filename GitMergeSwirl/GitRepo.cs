using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
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
            repo = new Repository(config.gitRepoPath);
            //Branches = new List<SXBranch>();

        }

        public List<DataModel.Branch> GetBranches()
        {
            var gb = new List<DataModel.Branch>();
            foreach (Branch branch in repo.Branches)
            {
                var b = new DataModel.Branch
                {
                    CanonicalName = branch.CanonicalName,
                    GitCommit = branch.Tip
                };
                b.GitBranch = branch;
                // Last Commit
                b.ShaTip = b.GitCommit.Sha;


                if (branch.CanonicalName.Contains(@"/heads/private/")) b.BranchType = DataModel.BranchType.Private;
                else if (branch.CanonicalName.Contains(@"/heads/releases/")) b.BranchType = DataModel.BranchType.Release;
                else b.BranchType = DataModel.BranchType.Other;

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
            var commitLog = repo.Commits;

            privateBranch.ToReleaseBranch = new List<DataModel.PrivateBranchToReleaseBranch>();


            foreach (var releaseBranch in releaseBranches)
            {
                //  if(!releaseBranch.CanonicalName.Contains("2012.1")) continue; // TEST

                var b = new DataModel.PrivateBranchToReleaseBranch();
                b.BaseCommit = repo.ObjectDatabase.FindMergeBase(privateBranch.GitCommit, releaseBranch.GitCommit);
                b.BaseCommitSha = b.BaseCommit.Sha; // Store in DB
                b.PrivateBranchCanonicalName = privateBranch.CanonicalName;
                b.ReleaseBranchCanonicalName = releaseBranch.CanonicalName;

                privateBranch.RecordChanged = true;


                var pBranch = privateBranch.GitBranch;


                // Test one
                //var ahead = commitLog.QueryBy(new CommitFilter { IncludeReachableFrom = pBranch.Tip, ExcludeReachableFrom = b.BaseCommit });
                //var behind = commitLog.QueryBy(new CommitFilter { IncludeReachableFrom = releaseBranch.GitBranch.Tip, ExcludeReachableFrom = b.BaseCommit });

                //b.Ahead = ahead.Count();
                //b.Behind = behind.Count();

                // Test 2
                var div = repo.ObjectDatabase.CalculateHistoryDivergence(pBranch.Tip, b.BaseCommit);
                b.Ahead = div.AheadBy;
                b.Behind = div.BehindBy;


                privateBranch.ToReleaseBranch.Add(b);

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
