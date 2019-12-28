namespace DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        MovieId = c.Int(nullable: false, identity: true),
                        Title = c.String(unicode: false),
                        OrginalTitle = c.String(unicode: false),
                        Rank = c.Int(),
                        Year = c.Int(),
                        Duration = c.String(unicode: false),
                        Rate = c.Int(),
                        RateTotalVotes = c.Int(),
                        DistributionCompany = c.String(unicode: false),
                        Description = c.String(unicode: false),
                        ReleaseDate = c.DateTime(nullable: false, precision: 0),
                        Studio = c.String(unicode: false),
                        BoxOffice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Production = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MovieId);
            
            CreateTable(
                "dbo.Persons",
                c => new
                    {
                        PersonId = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Surname = c.String(unicode: false),
                        isActor = c.Boolean(nullable: false),
                        isDirector = c.Boolean(nullable: false),
                        isScenarist = c.Boolean(nullable: false),
                        isPhotographer = c.Boolean(nullable: false),
                        isComposer = c.Boolean(nullable: false),
                        isDescription = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PersonId);
            
            CreateTable(
                "dbo.MovieTypes",
                c => new
                    {
                        MovieTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Type = c.Int(nullable: false),
                        Description = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.MovieTypeId);

            CreateTable(
                "dbo.PersonMovies",
                c => new
                {
                    Person_PersonId = c.Int(nullable: false),
                    Movie_MovieId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.Person_PersonId, t.Movie_MovieId })
                .ForeignKey("dbo.Persons", t => t.Person_PersonId, cascadeDelete: true)
                .ForeignKey("dbo.Movies", t => t.Movie_MovieId, cascadeDelete: true);
            //.Index(t => t.Person_PersonId)
            //.Index(t => t.Movie_MovieId);

            CreateTable(
                "dbo.MovieTypeMovies",
                c => new
                {
                    MovieType_MovieTypeId = c.Int(nullable: false),
                    Movie_MovieId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.MovieType_MovieTypeId, t.Movie_MovieId })
                .ForeignKey("dbo.MovieTypes", t => t.MovieType_MovieTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Movies", t => t.Movie_MovieId, cascadeDelete: true);
                //.Index(t => t.MovieType_MovieTypeId)
                //.Index(t => t.Movie_MovieId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MovieTypeMovies", "Movie_MovieId", "dbo.Movies");
            DropForeignKey("dbo.MovieTypeMovies", "MovieType_MovieTypeId", "dbo.MovieTypes");
            DropForeignKey("dbo.PersonMovies", "Movie_MovieId", "dbo.Movies");
            DropForeignKey("dbo.PersonMovies", "Person_PersonId", "dbo.Persons");
            DropIndex("dbo.MovieTypeMovies", new[] { "Movie_MovieId" });
            DropIndex("dbo.MovieTypeMovies", new[] { "MovieType_MovieTypeId" });
            DropIndex("dbo.PersonMovies", new[] { "Movie_MovieId" });
            DropIndex("dbo.PersonMovies", new[] { "Person_PersonId" });
            DropTable("dbo.MovieTypeMovies");
            DropTable("dbo.PersonMovies");
            DropTable("dbo.MovieTypes");
            DropTable("dbo.Persons");
            DropTable("dbo.Movies");
        }
    }
}
