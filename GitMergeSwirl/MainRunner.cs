using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DevOps.GitMergeSwirl
{
    /// <summary>
    /// Runner should be used with the using statement to clean update database connected
    /// </summary>
    /// <remarks>Design to be 
    /// </remarks>
    public class MainRunner : IDisposable
    {
        //public DBSchema.BranchContext BranchContext;
        //   public readonly List<Schema.Branch> SXBranches;

        public GitRepo Repo { get; private set; }
        public Config Config { get; private set; }

        public List<DataModel.Branch> WorkingBranchList { get; private set; }

        public readonly DataModel.BranchContext dbBranchContext;

        public List<DataModel.ReleaseMapping> ReleaseMappings { get; private set; }

        public MainRunner()
        {
            Config = new Config();
            Repo = new GitRepo(Config);
            WorkingBranchList = new List<DataModel.Branch>();
 
            // Runner is Disposable because BranchContext (Entity framework) is being used in an connected mode
            dbBranchContext = new DataModel.BranchContext();

            SetReleaseMappping();
        }

        private void SetReleaseMappping()
        {
            ReleaseMappings = dbBranchContext.ReleaseParentMappings.ToList();
            foreach (var releaseMapping in ReleaseMappings)
            {
                releaseMapping.ShortName = releaseMapping.ReleaseName.TrimStart(@"releases/".ToCharArray());
            }
        }

        public DataModel.BranchType SetBranchType(string branchCanonicalName)
        {
            if (branchCanonicalName.Contains(@"/heads/releases/")) return DataModel.BranchType.Release;
            if (branchCanonicalName.Contains(@"/heads/private/"))
            {
                var i = branchCanonicalName.LastIndexOf('-');
                if (i == -1) return DataModel.BranchType.Private;

                var mergeTarget = branchCanonicalName.Substring(i+1);
                return ReleaseMappings.Any(mapping => mapping.ShortName == mergeTarget) ? DataModel.BranchType.MergeTarget : DataModel.BranchType.Private;
            }

            return DataModel.BranchType.Other;
        }

        /// <summary>
        /// Read from Git and the DB and form an in memory view
        /// </summary>
        public void SyncGitAndDBToMemory()
        {
            var watch = Stopwatch.StartNew();
            var allowBranchTypes = new[] {DataModel.BranchType.Release, DataModel.BranchType.Private, DataModel.BranchType.MergeTarget};

            // Case to a list to cause the SQL Select to happen, this will populate the .local collection
            var dbBranches = dbBranchContext.Branchs.ToList();
            var gitBranches = Repo.GetBranches(); // All Wanted and Current Git Branches

            // The current Git Branch set is Set of data we only with to work with, (any branch not in the current git set should be deleted)
            // Sync to the Database Set to the Git Repository Set
            foreach (var gitBranch in gitBranches)
            {
                var dbBranch = dbBranches.FirstOrDefault(b => b.CanonicalName == gitBranch.CanonicalName);
                if (dbBranch == null) // not in Database
                {
                    gitBranch.BranchType = SetBranchType(gitBranch.CanonicalName);
                    if(!allowBranchTypes.Contains(gitBranch.BranchType)) continue; // Skip types we don't want to process

                    gitBranch.NewCommitsOnBranch = true; // New Branch is always 
                    gitBranch.InDatabase = false;
                    dbBranchContext.Branchs.Add(gitBranch); // Add the DB Context, will be save to DB later
                }
                else // Exist in database, check that the commit is the same
                {
                    // Map [Not Mapped] Stuff
                    dbBranch.GitBranch = gitBranch.GitBranch;
                    dbBranch.GitCommit = gitBranch.GitCommit;

                    dbBranch.InDatabase = true;
                    // Are the Tips the same, When false all Merge testing must be done again
                    dbBranch.NewCommitsOnBranch = (dbBranch.ShaTip == gitBranch.ShaTip);
                }
            }

            // Remove Non existing branch from the database view that is not currently in git
            var removeBranches = dbBranches.Where(dbBranch => gitBranches.All(b => b.CanonicalName != dbBranch.CanonicalName)).ToList();
            var removePrivateToReleaseMappings = new List<DataModel.PrivateToReleaseMapping>();
            foreach (var branch in removeBranches)
            {
                removePrivateToReleaseMappings.AddRange(branch.ToReleaseBranch);
            }
            dbBranchContext.PrivateToReleaseMappings.RemoveRange(removePrivateToReleaseMappings);
            dbBranchContext.Branchs.RemoveRange(removeBranches);

            // Re get the all the Branches to include newly added and removed items
            dbBranches = dbBranchContext.Branchs.Local.ToList();

            // Case the  Repo.GetBranches returns all branches then limit the working result to the branch types here
            //WorkingBranchList = dbBranches.Where(b => b.BranchType != DataModel.BranchType.Private || b.BranchType == DataModel.BranchType.Release).ToList();
            WorkingBranchList = dbBranches;

            var s = $"SyncGitAndDBToMemory:{watch.Elapsed}, Release Branches:{gitBranches.Count(r => r.BranchType == DataModel.BranchType.Release)}";
            s += $", Private Branches:{gitBranches.Count(p => p.BranchType == DataModel.BranchType.Private)}, Total:{gitBranches.Count()}";
            Console.WriteLine(s);
        }

        public void SyncInMemoryResultsToDB()
        {
            var watch = Stopwatch.StartNew();

            dbBranchContext.SaveChanges();

            //  Console.WriteLine($"SyncInMemoryResultsToDB:{watch.Elapsed} New:{newBranches.Count()}, Update:{updates}, Remove:{removes}");
            Console.WriteLine($"SyncInMemoryResultsToDB:{watch.Elapsed}");
        }

        public void SetParentBranch()
        {
            var count = 0;
            foreach (var branch in WorkingBranchList)
            {
                if (branch.ToReleaseBranch == null || branch.ToReleaseBranch.Count == 0) continue;

                // sort to Release Branch by Ahead then Behind (Take lowest of both)

                var b = branch.ToReleaseBranch;
                b.Sort(new DataModel.PrivateToReleaseMappingComparer());
                var e = b.First();
                branch.ReleaseParentCanonicalName = e.ReleaseBranchCanonicalName;
                branch.ReleaseParentSha = e.ReleaseBranchSha;
                count++;
            }

            Console.WriteLine($"SetParentBranch New:{count}");
        }


        public async Task FindBranchParents(DataModel.Branch branch)
        {
            var watch = Stopwatch.StartNew();

            var releaseBranches = WorkingBranchList.Where(s => s.BranchType == DataModel.BranchType.Release).ToList();

            await Task.Run(() => Repo.FindReleaseParentForPrivateBranchAsync(branch, releaseBranches));

            Console.WriteLine($"FindReleaseParentForPrivateBranch:{watch.Elapsed} Branch:{branch.CanonicalName}");
        }

        public void Dispose()
        {
            dbBranchContext?.Dispose();
        }
    }
}
