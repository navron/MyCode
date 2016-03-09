﻿using System.Collections.Generic;
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
            var gitBranches = Repo.GetBranches(); // All Wanted and Current Git Branches
            var dbBranches = GetBranchesFromDB(); // All Current Branches form the database

            // Sync to the Database Set to the Git Repository Set
            foreach (var gitBranch in gitBranches)
            {
                // Check that the Git Branch is in the Database Branch list
                foreach (var dbBranch in dbBranches)
                {
                    if (gitBranch.CanonicalName != dbBranch.CanonicalName) continue;
                    gitBranch.InDatabase = true;

                    // Are the Tips the same, When false all Merge testing must be done again
                    gitBranch.BranchUpdated = (dbBranch.ShaTip == gitBranch.ShaTip);
                }
                // If not in database Do Add Stuff
                if (!gitBranch.InDatabase)
                {

                }
            }

            WorkingBranchList = gitBranches;
//            TheList = gitBranches.Where(s => s.BranchType == DataModel.BranchType.Private || s.BranchType == DataModel.BranchType.Release).ToList();
        }

        public void SycnToDB()
        {
                   // Create a list of dbBranches Names that need to be deleted
            //var collectForDeletion = new List<string>();  // CanonicalName
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
            //    var updatedBranches = wantedBranches.Where(s => s.BranchUpdated == false);
            //    //update branches need any testing re done

            //    db.SaveChanges(); // Sync Console App
            //}
        }

    

        public void FindReleaseParents()
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


            using (var db = new DataModel.BranchContext())
            {
                //foreach (var privateBranch in privateBranchs)
                //{
                //    Repo.FindReleaseParentForPrivateBranch(privateBranch, releaseBranchs);
                //    db.Branchs.Attach(privateBranch);
                //    db.ToReleaseBranches.AddRange(privateBranch.ToReleaseBranch);
                //}


                ////    db.Branchs.Attach(testBranch);
                //db.Entry(testBranch).State = EntityState.Modified;
                //  testBranch.MergedTested = true;

                //db.ToReleaseBranches.AddRange(testBranch.ToReleaseBranch);



                db.SaveChanges();
            }
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
