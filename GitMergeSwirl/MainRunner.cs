using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DevOps.GitMergeSwirl
{
    public class MainRunner
    {
        public static MainRunner Instance { get; } = new MainRunner();


        //public DBSchema.BranchContext BranchContext;
        //   public readonly List<Schema.Branch> SXBranches;

        public GitRepo Repo { get; private set; }
        public Config Config { get; private set; }

        public List<DataModel.Branch> WorkingBranchList { get; private set; }


        public List<DataModel.PrivateBranchToReleaseBranchMapping> WorkingBranchToParentList { get; private set; }

        private MainRunner()
        {
            Config = new Config();
            Repo = new GitRepo(Config);
            WorkingBranchList = new List<DataModel.Branch>();
            // Init Lists and DB
            //  BranchContext = new DBSchema.BranchContext();
            //     SXBranches = new List<Schema.Branch>();
        }

        public void Clear()
        {
            //      SXBranches.Clear();
        }

        /// <summary>
        /// Read from Git and the DB and form an in memory view
        /// </summary>
        public void SyncGitAndDBToMemory()
        {
            var watch = Stopwatch.StartNew();

            var gitBranches = Repo.GetBranches(); // All Wanted and Current Git Branches
            var dbBranches = GetBranchesFromDB(); // All Current Branches form the database

            // The current Git Branch set is Set of data we only with to work with, (any branch not in the current git set should be deleted)
            // Sync to the Database Set to the Git Repository Set
            foreach (var gitBranch in gitBranches)
            {
                var dbBranch = dbBranches.FirstOrDefault(b => b.CanonicalName == gitBranch.CanonicalName);
                if (dbBranch == null) // not in Database
                {
                    gitBranch.NewCommitsOnBranch = true; // New Branch is always 
                }
                else // Exist in database, check that the commit is the same
                {
                    gitBranch.InDatabase = true;
                    // Are the Tips the same, When false all Merge testing must be done again
                    gitBranch.NewCommitsOnBranch = (dbBranch.ShaTip == gitBranch.ShaTip);
                }
            }

            // Case the  Repo.GetBranches returns all branches then limit the working result to the branch types here
            // WorkingBranchList = gitBranches.Where(s => s.BranchType == DataModel.BranchType.Private || s.BranchType == DataModel.BranchType.Release).ToList();
            WorkingBranchList = gitBranches;

            var s = $"SyncGitAndDBToMemory:{watch.Elapsed}, Release Branches:{gitBranches.Count(r => r.BranchType == DataModel.BranchType.Release)}";
            s += $", Private Branches:{gitBranches.Count(p => p.BranchType == DataModel.BranchType.Private)}, Total:{gitBranches.Count()}";
            Console.WriteLine(s);
        }

        public void SyncInMemoryResultsToDB()
        {
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

            using (var db = new DataModel.BranchContext())
            {
                // Check that the branch does not existing in the working branch list and removed it if needed
                foreach (var dbBranch in db.Branchs)
                {
                    if (WorkingBranchList.All(b => b.CanonicalName != dbBranch.CanonicalName))
                    {
                        db.Branchs.Remove(dbBranch);
                    }
                }

                // Add new Branches
                var newBranches = WorkingBranchList.Where(s => s.InDatabase == false).ToList();
                db.Branchs.AddRange(newBranches);

                // Ingoring updated values for now (Don't know how to save these yet)
                var updatedBranches = WorkingBranchList.Where(s => s.NewCommitsOnBranch == false);

                db.SaveChanges();

            }
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
        }

    

        public void FindBranchParents()
        {
            //var releaseBranchs = TheList.Where(s => s.BranchType == DataModel.BranchType.Release).ToList();
            //var privateBranchs = TheList.Where(s => s.BranchType == DataModel.BranchType.Private).ToList();

            //   var r = releaseBranchs.Select(gitBranch => gitBranch.GitCommit).ToList();

            //    var t = @"refs/heads/private/nquixley/STREAMS-47810-TeamCityChecker";
            //var testBranch = privateBranchs.FirstOrDefault(b => b.CanonicalName == t);
            //if (testBranch != null)
            //{
            //    var p = testBranch.GitCommit;
            // //   Repo.GetReleaseBranchParentForPrivateBranch(p, r);
            //    Repo.FindReleaseParentForPrivateBranch(testBranch, releaseBranchs);
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

        private List<DataModel.Branch> GetBranchesFromDB()
        {
            var dbBranches = new List<DataModel.Branch>();
            using (var db = new DataModel.BranchContext())
            {
                dbBranches.AddRange(db.Branchs);
            }
            return dbBranches;
        }

    }
}
