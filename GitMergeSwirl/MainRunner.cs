using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LibGit2Sharp;

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


        private readonly DataModel.BranchContext dbBranchContext;

        public MainRunner()
        {
            Config = new Config();
            Repo = new GitRepo(Config);
            WorkingBranchList = new List<DataModel.Branch>();
            // Init Lists and DB
            //  BranchContext = new DBSchema.BranchContext();
            //     SXBranches = new List<Schema.Branch>();

            // Runner is Disposable because BranchContext (Entity framework) is being used in an connected mode
            dbBranchContext = new DataModel.BranchContext();
        }

        /// <summary>
        /// Read from Git and the DB and form an in memory view
        /// </summary>
        public void SyncGitAndDBToMemory()
        {
            var watch = Stopwatch.StartNew();

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

            // Mark Database Branch as "ToRemove if they don't exist in the Git Repo (Could just remove here)
            foreach (var dbBranch in dbBranches)
            {
                // All Git branches does not equal the current db branch
                dbBranch.ToRemove = gitBranches.All(b => b.CanonicalName != dbBranch.CanonicalName);
            }

            // Case the  Repo.GetBranches returns all branches then limit the working result to the branch types here
            WorkingBranchList = dbBranches.Where(b => b.BranchType == DataModel.BranchType.Private || b.BranchType == DataModel.BranchType.Release).ToList();

            var s = $"SyncGitAndDBToMemory:{watch.Elapsed}, Release Branches:{gitBranches.Count(r => r.BranchType == DataModel.BranchType.Release)}";
            s += $", Private Branches:{gitBranches.Count(p => p.BranchType == DataModel.BranchType.Private)}, Total:{gitBranches.Count()}";
            Console.WriteLine(s);
        }

        public void SyncInMemoryResultsToDB()
        {
            var watch = Stopwatch.StartNew();
            // var dbBranches = GetBranchesFromDB(); // Current Branches
            // Create a list of dbBranches Names that need to be deleted
            //   var collectForDeletion = new List<string>();  // CanonicalName
            //foreach (var dbBranch in dbBranches)
            //{
            //    var found = gitBranches.Any(b => b.CanonicalName == dbBranch.CanonicalName);
            //    if (!found)
            //    {
            //        collectForDeletion.Add(dbBranch.CanonicalName);
            //    }
            //}

            //using (var db = new DataModel.BranchContext())
            //{
            //    int removes = 0;
            //    //// Check that the branch does not existing in the working branch list and removed it if needed
            //    //foreach (var dbBranch in db.Branchs)
            //    //{
            //    //    if (WorkingBranchList.All(b => b.CanonicalName != dbBranch.CanonicalName))
            //    //    {
            //    //        db.Branchs.Remove(dbBranch);
            //    //        removes++;
            //    //    }
            //    //}

            //    //int updates = 0;
            //    //foreach (var dbBranch in db.Branchs)
            //    //{
            //    //    var gitbranch = WorkingBranchList.FirstOrDefault(b => b.CanonicalName == dbBranch.CanonicalName);
            //    //    if (gitbranch == null) continue; // Should always find a record
            //    //    if (dbBranch.IsEqual(gitbranch))
            //    //    {
            //    //        dbBranch.AssignValue(gitbranch);
            //    //        updates++;
            //    //    }
            //    //}


            //    // Add new Branches
            //    var newBranches = WorkingBranchList.Where(s => s.InDatabase == false).ToList();
            //    db.Branchs.AddRange(newBranches);

            dbBranchContext.SaveChanges();

            //  Console.WriteLine($"SyncInMemoryResultsToDB:{watch.Elapsed} New:{newBranches.Count()}, Update:{updates}, Remove:{removes}");
            Console.WriteLine($"SyncInMemoryResultsToDB:{watch.Elapsed}");


            //{
            //    foreach (string canonicalName in collectForDeletion)
            //    {
            //        var bs = db.Branchs.Where(b => b.CanonicalName == canonicalName).ToList();
            //        db.Branchs.RemoveRange(bs);
            //    }

            //    // For each DB Branch is it in the Git Repo. If not then mark for deletion
            //    //foreach (var dbBranch in db.Branchs)
            //    //{
            //    //    var found = gBranches.Any(b => b.CanonicalName == dbBranch.CanonicalName);
            //    //    if (!found)
            //    //    {
            //    //        db.Branchs.Remove(dbBranch);
            //    // //       collectForDeletion.Add(dbBranch);
            //    //    }
            //    //}


            //    //db.Branchs.RemoveRange(collectForDeletion);

            //    var newBranches = wantedBranches.Where(s => s.InDatabase == false).ToList();
            //    db.Branchs.AddRange(newBranches);

            //    // Ingoring updated values for now
            //    var updatedBranches = wantedBranches.Where(s => s.NewCommitsOnBranch == false);
            //    //update branches need any testing re done

            //    db.SaveChanges(); // Sync Console App
            //}




            //using (var db = new DataModel.BranchContext())
            //{
            //    //foreach (var privateBranch in privateBranchs)
            //    //{
            //    //    Repo.FindReleaseParentForPrivateBranch(privateBranch, releaseBranchs);
            //    //    db.Branchs.Attach(privateBranch);
            //    //    db.ToReleaseBranches.AddRange(privateBranch.ToReleaseBranch);
            //    //}


            //    ////    db.Branchs.Attach(testBranch);
            //    //db.Entry(testBranch).State = EntityState.Modified;
            //    //  testBranch.MergedTested = true;

            //    //db.ToReleaseBranches.AddRange(testBranch.ToReleaseBranch);



            //    db.SaveChanges();
            //}
        }

        public void SetParentBranch()
        {
            var count = 0;
            foreach (var branch in WorkingBranchList)
            {
                if (branch.ToReleaseBranch == null || branch.ToReleaseBranch.Count == 0) continue;

                // sort to Release Branch by Ahead then Behind (Take lowest of both)
                

                var b = branch.ToReleaseBranch;
                b.Sort(new DataModel.PrivateBranchToReleaseBranchMappingComparer());
                var e = b.First();
                branch.ReleaseParentCanonicalName = e.ReleaseBranchCanonicalName;
                branch.ReleaseParentSha = e.ReleaseBranchSha;
                count++;
            }

            Console.WriteLine($"SetParentBranch New:{count}");
        }


        public async Task FindBranchParents(DataModel.Branch branch)
        {
            //TODO call FindReleaseParentForPrivateBranch as async await (in seriles) 
            var watch = Stopwatch.StartNew();

            var releaseBranches = WorkingBranchList.Where(s => s.BranchType == DataModel.BranchType.Release).ToList();

            await Task.Run(() => Repo.FindReleaseParentForPrivateBranchAsync(branch, releaseBranches));

            Console.WriteLine($"FindReleaseParentForPrivateBranch:{watch.Elapsed} Branch:{branch.CanonicalName}");

        }


        public void Dispose()
        {
            // Not being called TODO Fix this
            dbBranchContext?.Dispose();
        }
    }
}
