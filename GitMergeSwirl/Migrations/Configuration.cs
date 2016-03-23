using System.Data.Entity.Migrations;

namespace DevOps.GitMergeSwirl.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DataModel.BranchContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "GitMerge Migrations";
            AutomaticMigrationDataLossAllowed = true;

        }

        protected override void Seed(DataModel.BranchContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            // As of Feb 2016
            context.ReleaseParentMappings.AddOrUpdate(
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.1", ParentName = "" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.4", ParentName = "releases/2012.1" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.15", ParentName = "releases/2012.5" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.16", ParentName = "releases/2012.16.SAT" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.16", ParentName = "releases/2012.20" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.16", ParentName = "releases/2012.21" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.16", ParentName = "releases/2012.5" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.17", ParentName = "releases/2012.14" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.17", ParentName = "releases/2012.17.OPT" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.17", ParentName = "releases/2012.19" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.17", ParentName = "releases/2012.5" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.19", ParentName = "releases/2012.1" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.20", ParentName = "releases/2012.1" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.27", ParentName = "releases/2012.1" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.29", ParentName = "releases/2012.27" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.29", ParentName = "releases/2012.5" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.30", ParentName = "releases/2012.15" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.34", ParentName = "releases/2012.17" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.34", ParentName = "releases/2012.34.OPT" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.34.OPT", ParentName = "releases/2012.17.OPT" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.36", ParentName = "releases/2012.20" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.36", ParentName = "releases/2012.27" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.41", ParentName = "releases/2012.30" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.41", ParentName = "releases/2012.8" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.45", ParentName = "releases/2012.36" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.5", ParentName = "releases/2012.1" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2012.8", ParentName = "releases/2012.1" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2014.0", ParentName = "releases/2012.15" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2014.0", ParentName = "releases/2012.16" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2014.0", ParentName = "releases/2012.17" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2014.0", ParentName = "releases/2012.29" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2014.0", ParentName = "releases/2012.36" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2014.0", ParentName = "releases/2012.41" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2014.0.StagingTMR", ParentName = "releases/2012.45" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2014.0.StagingTMR", ParentName = "releases/2014.0" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2014.20", ParentName = "releases/2012.34" },
                new DataModel.ReleaseMapping { ReleaseName = "releases/2014.20", ParentName = "releases/2014.0" },
                new DataModel.ReleaseMapping { ReleaseName = "master", ParentName = "releases/2014.20" },
                new DataModel.ReleaseMapping { ReleaseName = "master", ParentName = "releases/2014.0.StagingTMR" }
            );

        }
    }
}
