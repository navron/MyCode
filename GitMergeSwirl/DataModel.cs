using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

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

            public DbSet<PrivateBranchToReleaseBranch> ToReleaseBranches { get; set; }
            

        }

        /// <summary>
        /// 
        /// </summary>
        public class Branch
        {
            [Key]
            public string CanonicalName { get; set; }
            public string ShaTip { get; set; } // The Tip of the Branch
            public string ReleaseParentCanonicalName { get; set; }
            public string ReleaseParentSha { get; set; }
            public virtual List<BranchCommit> Commits { get; set; }
            public BranchType BranchType { get; set; }
            public bool MergedTested { get; set; }

            public List<PrivateBranchToReleaseBranch> ToReleaseBranch { get; set; }

            // Temporary Values
            [NotMapped]
            public bool InDatabase { get; set; } // Is this entry in the DB
            [NotMapped]
            public bool RecordChanged { get; set; } // Is this entry in the DB
            [NotMapped]
            public LibGit2Sharp.Branch GitBranch { get; set; } // reference to the Git Repo Commit Object

            [NotMapped]
            public LibGit2Sharp.Commit GitCommit { get; set; } // reference to the Git Repo Commit Object
            [NotMapped]
            public bool BranchUpdated { get; set; } // Has there been another commit on this branch
            [NotMapped]
            public bool DeleteInDatabase { get; set; } // Mark to remove database entry
            public Branch()
            {
                InDatabase = false;
                DeleteInDatabase = false;
                BranchUpdated = false;
                RecordChanged = false;
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

        public class PrivateBranchToReleaseBranch
        {
            [Key]
            [Column(Order = 1)]
            public string PrivateBranchCanonicalName { get; set; } // Git Friendly Name
            [Key]
            [Column(Order = 2)]
            public string ReleaseBranchCanonicalName { get; set; } // Git Friendly Name

            [NotMapped]
            public LibGit2Sharp.Commit BaseCommit { get; set; }
            public string BaseCommitSha { get; set; }
            

            public int? CommitCountToBaseCommit { get; set; }
            public int? Ahead { get; set; }
            public int? Behind { get; set; }

        }

        public class ReleaseParentMapping
        {
            [Key]
            [Column(Order = 1)]
            public string ReleaseName { get; set; } // Git Friendly Name
            [Key]
            [Column(Order = 2)]
            public string ParentName { get; set; } // Git Friendly Name

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

        public enum BranchType
        {
            Release,
            Private,
            Archive,
            MergeTest,
            Other
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
