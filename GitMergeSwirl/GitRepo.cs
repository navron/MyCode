using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace DevOps.GitMergeSwirl
{
    public class GitRepo
    {
        private readonly Repository repository;

        public GitRepo(Config config)
        {
            try
            {
                repository = new Repository(config.gitRepoPath);
            }
            catch (Exception)
            {
                Console.WriteLine($"ERROR: Can not read Git Repository, Check that {config.gitRepoPath} exists");
                throw;
            }
        }

        // Get the Git Branches from the repository and pack the object into our model of an branch. 
        public List<DataModel.Branch> GetBranches()
        {
            var gitBranches = new List<DataModel.Branch>();
            foreach (var branch in repository.Branches)
            {
                var b = new DataModel.Branch
                {
                    CanonicalName = branch.CanonicalName,
                    GitCommit = branch.Tip,
                    GitBranch = branch
                };
                // Last Commit
                b.ShaTip = b.GitCommit.Sha;
                gitBranches.Add(b);
            }

            // limit to only Release and Private for now
            //var wantedBranches = gitBranches.Where(b => b.BranchType == DataModel.BranchType.Private || b.BranchType == DataModel.BranchType.Release).ToList();
  //          var wantedBranches = gitBranches.Where(b => b.BranchType != DataModel.BranchType.Archive && b.BranchType != DataModel.BranchType.Other).ToList();
//            return wantedBranches;
            return gitBranches;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously (Its because the Parallel ForEach has the await operator I think is a bug with ReShaper)
        public async Task FindReleaseParentForPrivateBranchAsync(DataModel.Branch privateBranch, List<DataModel.Branch> releaseBranches)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (privateBranch.ToReleaseBranch == null)
            {
                privateBranch.ToReleaseBranch = new List<DataModel.PrivateToReleaseMapping>();
            }

            // Test for each branch in the test set (All Release branches)
            // Parallel takes this from 2.4 seconds to 0.3 seconds (about 5 minutes to 30 seconds)
            Parallel.ForEach(releaseBranches, async releaseBranch => await Task.Run(()=> FindReleaseParentForPrivateBranchAsync(releaseBranch, privateBranch)));
        }

        // Do the work on for the find
        private async Task FindReleaseParentForPrivateBranchAsync(DataModel.Branch releaseBranch, DataModel.Branch privateBranch)
        {

            // Check to see if the test has already been done, and skip 
            var test = privateBranch.ToReleaseBranch.FirstOrDefault(r => r.ReleaseBranchCanonicalName == releaseBranch.CanonicalName);
            if (test != null)
            {
                if (test.PrivateBranchSha == privateBranch.ShaTip &&
                    test.ReleaseBranchSha == releaseBranch.ShaTip &&
                    test.Ahead != null &&
                    test.Behind != null)
                {
                    // No need to recheck again
                    return;
                }
            }
            else
            {
                test = new DataModel.PrivateToReleaseMapping
                {
                    // Short cut (may be) if the base Commit has the same hash as a release branch then that is what branch its from 
                    //   BaseCommit = repository.ObjectDatabase.FindMergeBase(privateBranch.GitCommit, releaseBranch.GitCommit),
                    PrivateBranchCanonicalName = privateBranch.CanonicalName,
                    ReleaseBranchCanonicalName = releaseBranch.CanonicalName,
                    PrivateBranchSha = privateBranch.ShaTip,
                    ReleaseBranchSha = releaseBranch.ShaTip,
                };
            }
            test.RecordUpdate = true; //TODO WHAT WAS THIS FOR AGAIN ?

            var pBranch = privateBranch.GitBranch;

            // Get the Commit difference between the release commit and this branch commit
            // This takes time so call with the await operator
            var div = await Task.Run(() => repository.ObjectDatabase.CalculateHistoryDivergence(pBranch.Tip, releaseBranch.GitCommit));
            test.Ahead = div.AheadBy;
            test.Behind = div.BehindBy;

            privateBranch.ToReleaseBranch.Add(test);
        }
    }


}
