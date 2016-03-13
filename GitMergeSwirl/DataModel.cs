using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

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
            public DbSet<ReleaseParentMapping> ReleaseParentMappings { get; set; }
            public DbSet<ChangeLog> ChangeLogs { get; set; }

            public DbSet<PrivateBranchToReleaseBranchMapping> PrivateBranchToReleaseBranchMappings { get; set; }


        }

        /// <summary>
        /// Model of private and release branches
        /// </summary>
        /// <remarks> Single entry per branch</remarks>
        public class Branch //: IComparable
        {
            // Full name of an git branch, case sensitive, (git is non case sensitive, company policy is enforce case sensitive in branch names)
            [Key]
            public string CanonicalName { get; set; }
            // Current Sha1, change to this value will cause NewCommitsOnBranch to be true
            public string ShaTip { get; set; }
            // Valid only for Private Branches, Null for Release branches types
            public string ReleaseParentCanonicalName { get; set; }
            public string ReleaseParentSha { get; set; }
            public virtual List<BranchCommit> Commits { get; set; }
            public BranchType BranchType { get; set; }
            public bool MergedTested { get; set; }

            // 
            public List<PrivateBranchToReleaseBranchMapping> ToReleaseBranch { get; set; }



            // Interdicts there been another commit on this branch since last run, and testing should be done again
            [NotMapped]
            public bool NewCommitsOnBranch { get; set; }

            // Branch entry in database, Save will update existing entry
            [NotMapped]
            public bool InDatabase { get; set; } // Is this entry in the DB


            //[NotMapped]
            //// Flag this record 
            //public bool RecordChanged { get; set; } // Is this entry in the DB


            // Mark to remove database entry on next save (branch has been removed)
            // [NotMapped]
            // public bool RemovedFromDatabase { get; set; }

            // Reference to the Git Repo Branch Object
            [System.ComponentModel.Browsable(false)] // DataGridView Hide Value
            [NotMapped]
            public LibGit2Sharp.Branch GitBranch { get; set; }

            // reference to the Git Repo Commit Object
            [System.ComponentModel.Browsable(false)] // DataGridView Hide Value
            [NotMapped]
            public LibGit2Sharp.Commit GitCommit { get; set; }


            public Branch()
            {
                InDatabase = false;
                //   RemovedFromDatabase = false;
                NewCommitsOnBranch = false;
                //    RecordChanged = false;
            }

           
            public bool IsEqual(Branch other)
            {
                if (other.ToReleaseBranch != null)
                {
                    var test = other.ToReleaseBranch.Any(r => r.RecordUpdate == true);
                    if (test) return false;
                }

                return string.Equals(CanonicalName, other.CanonicalName) &&
                       string.Equals(ShaTip, other.ShaTip) &&
                       string.Equals(ReleaseParentCanonicalName, other.ReleaseParentCanonicalName) &&
                       string.Equals(ReleaseParentSha, other.ReleaseParentSha) ;
                // Equals(Commits, other.Commits) &&
                // BranchType == other.BranchType && MergedTested == other.MergedTested &&
                // Equals(ToReleaseBranch, other.ToReleaseBranch) &&
                // NewCommitsOnBranch == other.NewCommitsOnBranch &&
                // InDatabase == other.InDatabase && 
                // Equals(GitBranch, other.GitBranch) &&
                // Equals(GitCommit, other.GitCommit);
            }

            
                // What shoudl this mehtod be call, standard
            public void AssignValue(Branch branch)
            {
                this.ShaTip = branch.ShaTip;
                this.ReleaseParentCanonicalName = branch.ReleaseParentCanonicalName;
                this.ReleaseParentSha = branch.ReleaseParentSha;
                // this.Commits // Not this one 
                this.MergedTested = branch.MergedTested;
                this.ToReleaseBranch = branch.ToReleaseBranch;

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
        public class PrivateBranchToReleaseBranchMapping
        {
            // Primary Key (both private and release branch)
            [Key]
            [Column(Order = 1)]
            public string PrivateBranchCanonicalName { get; set; } // Git Friendly Name

            // All Release Branches mapping back to the primary
            [Key]
            [Column(Order = 2)]
            public string ReleaseBranchCanonicalName { get; set; } // Git Friendly Name

            public string PrivateBranchSha { get; set; }

            public string ReleaseBranchSha { get; set; }

            [NotMapped]
            public bool RecordUpdate { get; set; }


            // The ahead and behinds are used to determine which branch is the true 
            public int? Ahead { get; set; }
            public int? Behind { get; set; }

            // This is the commit that is most common to both branches,
            // If the SHA1 is the same as an release branch then the private branch is from a clean branch off the release branch
            public string BaseCommitSha { get; set; }
            [NotMapped]
            public LibGit2Sharp.Commit BaseCommit { get; set; }


            public int? BaseAhead { get; set; }
            public int? BaseBehind { get; set; }


            // Release or Private -- Used to limit data sets
            public BranchType BranchType { get; set; }

            public PrivateBranchToReleaseBranchMapping()
            {
                RecordUpdate = false;
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
        public class ReleaseParentMapping
        {
            [Key]
            [Column(Order = 1)]
            // Git Friendly Name, not the Canonical Name
            public string ReleaseName { get; set; }
            [Key]
            [Column(Order = 2)]
            // Git Friendly Name, not the Canonical Name
            public string ParentName { get; set; }
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
            Archive,
            MergeTest,
            Other
        }

        public static BranchType SetBranchType(string branchCanonicalName)
        {
            if (branchCanonicalName.Contains(@"/heads/private/")) return DataModel.BranchType.Private;
            if (branchCanonicalName.Contains(@"/heads/releases/")) return DataModel.BranchType.Release;

            return DataModel.BranchType.Other;
        }

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
