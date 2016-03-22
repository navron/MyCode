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
                b.BranchType = DataModel.SetBranchType(b.CanonicalName);
                gitBranches.Add(b);
            }

            // limit to only Release and Private for now
            var wantedBranches = gitBranches.Where(b => b.BranchType == DataModel.BranchType.Private || b.BranchType == DataModel.BranchType.Release).ToList();
            return wantedBranches;
        }

        public async Task<bool> FindReleaseParentForPrivateBranchAsync(DataModel.Branch privateBranch, List<DataModel.Branch> releaseBranches)
        {
            if (privateBranch.ToReleaseBranch == null)
            {
                privateBranch.ToReleaseBranch = new List<DataModel.PrivateBranchToReleaseBranchMapping>();
            }

            // Test for each branch in the test set (All Release branches)
            foreach (var releaseBranch in releaseBranches)
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
                        continue; // No need to recheck again
                    }
                }
                else
                {
                    test = new DataModel.PrivateBranchToReleaseBranchMapping
                    {
                        // Short cut (may be) if the base Commit has the same hash as a release branch then that is what branch its from 
                        //   BaseCommit = repository.ObjectDatabase.FindMergeBase(privateBranch.GitCommit, releaseBranch.GitCommit),
                        PrivateBranchCanonicalName = privateBranch.CanonicalName,
                        ReleaseBranchCanonicalName = releaseBranch.CanonicalName,
                        PrivateBranchSha = privateBranch.ShaTip,
                        ReleaseBranchSha = releaseBranch.ShaTip,
                        BranchType = DataModel.SetBranchType(privateBranch.CanonicalName)
                    };
                }
                test.RecordUpdate = true;

                var pBranch = privateBranch.GitBranch;

                // Get the Commit difference between the release commit and this branch commit
                // This takes time so call with the await operator
                var div = await Task.Run(() => repository.ObjectDatabase.CalculateHistoryDivergence(pBranch.Tip, releaseBranch.GitCommit));
                test.Ahead = div.AheadBy;
                test.Behind = div.BehindBy;

                privateBranch.ToReleaseBranch.Add(test);
            }
            return true;
        }
    }

}
