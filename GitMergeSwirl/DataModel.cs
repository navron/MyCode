using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using LibGit2Sharp;

namespace DevOps.GitMergeSwirl
{
    /// <summary>
    /// Contains Pojo Class for the Git Merge Tool 
    /// </summary>
    public class DataModel
    {
        /// <summary>
        /// Entity Framework DB Class
        /// </summary>
        public class BranchContext : DbContext
        {
            // This is how you give a database the name (otherwise it will be the type name of this class)
            public BranchContext() : base("GitMerge") { }

            public DbSet<Branch> Branchs { get; set; }

            public DbSet<BranchCommit> BranchCommits { get; set; }
            public DbSet<BranchMergeTest> BranchMergeTests { get; set; }
            public DbSet<BranchMergeTestResult> BranchMergeTestResults { get; set; }
            public DbSet<ReleaseMapping> ReleaseParentMappings { get; set; }
            public DbSet<ChangeLog> ChangeLogs { get; set; }

            public DbSet<PrivateToReleaseMapping> PrivateToReleaseMappings { get; set; }
        }

        /// <summary>
        /// Model of private and release branches
        /// </summary>
        /// <remarks> Single entry per branch</remarks>
        public class Branch
        {
            // Full name of an git branch, case sensitive, (git is non case sensitive, company policy is enforce case sensitive in branch names)
            [Key]
            public string CanonicalName { get; set; }
            // Current Sha1, change to this value will cause NewCommitsOnBranch to be true
            public string ShaTip { get; set; }
            // Valid only for Private Branches, Null for Release branches types
            public string ReleaseParentCanonicalName { get; set; }
            public string ReleaseParentSha { get; set; }
   
            public BranchType BranchType { get; set; }
            public bool MergedTested { get; set; }

            // just change to virtual
            public virtual List<PrivateToReleaseMapping> ToReleaseBranch { get; set; }

            public virtual List<BranchCommit> Commits { get; set; }


            // Interdicts there been another commit on this branch since last run, and testing should be done again
            [NotMapped]
            public bool NewCommitsOnBranch { get; set; }

            // Branch entry in database, Save will update existing entry
            [NotMapped]
            public bool InDatabase { get; set; } // Is this entry in the DB

            [NotMapped]
            public bool ToRemove { get; set; } // To Remove This Entry


            //[NotMapped]
            //// Flag this record 
            //public bool RecordChanged { get; set; } // Is this entry in the DB


            // Mark to remove database entry on next save (branch has been removed)
            // [NotMapped]
            // public bool RemovedFromDatabase { get; set; }

            // Reference to the Git Repo Branch Object
            [Browsable(false)] // DataGridView Hide Value
            [NotMapped]
            public LibGit2Sharp.Branch GitBranch { get; set; }

            // reference to the Git Repo Commit Object
            [Browsable(false)] // DataGridView Hide Value
            [NotMapped]
            public Commit GitCommit { get; set; }


            public Branch()
            {
                InDatabase = false;
                //   RemovedFromDatabase = false;
                NewCommitsOnBranch = false;
                //    RecordChanged = false;
                ToRemove = false;
            }
        }

        public class BranchCommit
        {
            [Key]
            public int BranchCommitSha { get; set; }
            public virtual Branch Branch { get; set; }
            public string Sha { get; set; }
            public string CommitComment { get; set; }
            public string JiraTaskId { get; set; }
        }

        /// <summary>
        ///  Private branch (and Release branch) mapping to every Release Branch
        /// </summary>
        /// <remarks> 
        ///     Has a One to Many(Release Branch count) entries
        ///     Class is used to determine the parent of the private branch
        ///     Class is used to determine that the release branch are in correct mapping order as per the ReleaseParent Class.
        /// </remarks>
        public class PrivateToReleaseMapping
        {
            // Primary Key (both private and release branch)
            [Key]
            [Column(Order = 1)]
            public string PrivateBranchCanonicalName { get; set; } // Git Friendly Name
            public string PrivateBranchSha { get; set; }

            // All Release Branches mapping back to the primary
            [Key]
            [Column(Order = 2)]
            public string ReleaseBranchCanonicalName { get; set; } // Git Friendly Name
            public string ReleaseBranchSha { get; set; }

            [NotMapped]
            public bool RecordUpdate { get; set; } // DON"T THINK THIS IS NEEDED

            // The ahead and behinds are used to determine which branch is the true 
            public int? Ahead { get; set; }
            public int? Behind { get; set; }

            // This is the commit that is most common to both branches,
            // If the SHA1 is the same as an release branch then the private branch is from a clean branch off the release branch
            // public string BaseCommitSha { get; set; }
           // [NotMapped]
           // public LibGit2Sharp.Commit BaseCommit { get; set; }

            public PrivateToReleaseMapping()
            {
                RecordUpdate = false;
            }
        }

        // Determines the which release branch is closet to the private branch and thus where the private branch was branch from
        public class PrivateToReleaseMappingComparer : IComparer<PrivateToReleaseMapping>
        {
            public int Compare(PrivateToReleaseMapping x, PrivateToReleaseMapping y)
            {
                // If values are Null, then that item is last in the list, This is for shorting the compare when FindReleaseParentForPrivateBranch step are skip due to extract matches

                if (!x.Ahead.HasValue || !x.Behind.HasValue) return 1;
                if (!y.Ahead.HasValue || !y.Behind.HasValue) return 1;

                // If the Ahead values are the same then return based on the behind values
                return x.Ahead == y.Ahead ? x.Behind.Value.CompareTo(y.Behind.Value) : x.Ahead.Value.CompareTo(y.Ahead.Value);
            }
        }

        /// <summary>
        /// Release Branch to Release Parent Mapping
        /// </summary>
        /// <remarks>
        /// This Mapping is maintain outside this application (TODO, Add support for updating through this application)
        /// 
        /// 1. The Root Release Branch will have an "null" for an Parent Value
        /// 2. Master is not store in this table (possible should be) and the Master branch is used as the last commit to, (bottom of the tree)
        ///    The Parents of Master is determined by all Releases branches that is not a Parent Release. (i.e. loose leaves)
        /// </remarks>
        public class ReleaseMapping
        {
            [Key]
            [Column(Order = 1)]
            // Git Friendly Name, not the Canonical Name
            public string ReleaseName { get; set; }
            [Key]
            [Column(Order = 2)]
            // Git Friendly Name, not the Canonical Name
            public string ParentName { get; set; }

            // Name that would appear at the end of an private branch for merge reasons
            [NotMapped]
            public string ShortName { get; set; }
        }

        /// <summary>
        /// Branch Merge Tests
        /// </summary>
        public class BranchMergeTest
        {
            [Key]
            public string CanonicalName { get; set; }
            public virtual List<BranchMergeTestResult> Results { get; set; }
            public bool? Result { get; set; } // And for all Results
        }

        /// <summary>
        /// Branch Merge Tests Results per Release Branch
        /// </summary>
        public class BranchMergeTestResult
        {
            [Key]
            public int TestId { get; set; }
            public string ReleaseBranchCanonicalName { get; set; }
            public string ReleaseBranchSha { get; set; }
            public bool? Result { get; set; }
            public string ResultLog { get; set; }
        }

        // Branch Type, Determined by Naming of an branch. 
        public enum BranchType
        {
            Release,
            Private,
            MergeTarget, // Special type of private branch where the name ending has dash release branch (short name) i.e.  -2014.0
            Archive,
            Other
        }

        //public static BranchType SetBranchType(string branchCanonicalName)
        //{
        //    if (branchCanonicalName.Contains(@"/heads/releases/")) return BranchType.Release;
        //    if (branchCanonicalName.Contains(@"/heads/private/")) return BranchType.Private;

        //    return BranchType.Other;
        //}

        public class ChangeLog
        {
            [Key]
            public int ChangeId { get; private set; }
            public string Who { get; set; }
            public DateTime When { get; set; }
            public string What { get; set; }
        }
    }
}
