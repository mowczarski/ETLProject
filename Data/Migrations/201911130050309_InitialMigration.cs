namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        MovieId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        OrginalTitle = c.String(),
                        Rank = c.Int(),
                        Year = c.Int(),
                        Duration = c.String(),
                        Rate = c.Int(),
                        RateTotalVotes = c.Int(),
                        DistributionCompany = c.String(),
                        Description = c.String(),
                        ReleaseDate = c.DateTime(nullable: false),
                        Studio = c.String(),
                        BoxOffice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Production = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MovieId);
            
            CreateTable(
                "dbo.Persons",
                c => new
                    {
                        PersonId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        isActor = c.Boolean(nullable: false),
                        isDirector = c.Boolean(nullable: false),
                        isScenarist = c.Boolean(nullable: false),
                        isPhotographer = c.Boolean(nullable: false),
                        isComposer = c.Boolean(nullable: false),
                        isDescription = c.Boolean(nullable: false),
                        Movie_MovieId = c.Int(),
                    })
                .PrimaryKey(t => t.PersonId)
                .ForeignKey("dbo.Movies", t => t.Movie_MovieId)
                .Index(t => t.Movie_MovieId);
            
            CreateTable(
                "dbo.MovieTypes",
                c => new
                    {
                        MovieTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
                        Description = c.String(),
                        Movie_MovieId = c.Int(),
                    })
                .PrimaryKey(t => t.MovieTypeId)
                .ForeignKey("dbo.Movies", t => t.Movie_MovieId)
                .Index(t => t.Movie_MovieId);
            
            CreateTable(
                "dbo.Movies_Persons",
                c => new
                    {
                        Movie_PersonId = c.Int(nullable: false, identity: true),
                        PlayedAs = c.String(),
                        Movie_MovieId = c.Int(),
                        Person_PersonId = c.Int(),
                    })
                .PrimaryKey(t => t.Movie_PersonId)
                .ForeignKey("dbo.Movies", t => t.Movie_MovieId)
                .ForeignKey("dbo.Persons", t => t.Person_PersonId)
                .Index(t => t.Movie_MovieId)
                .Index(t => t.Person_PersonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Movies_Persons", "Person_PersonId", "dbo.Persons");
            DropForeignKey("dbo.Movies_Persons", "Movie_MovieId", "dbo.Movies");
            DropForeignKey("dbo.MovieTypes", "Movie_MovieId", "dbo.Movies");
            DropForeignKey("dbo.Persons", "Movie_MovieId", "dbo.Movies");
            DropIndex("dbo.Movies_Persons", new[] { "Person_PersonId" });
            DropIndex("dbo.Movies_Persons", new[] { "Movie_MovieId" });
            DropIndex("dbo.MovieTypes", new[] { "Movie_MovieId" });
            DropIndex("dbo.Persons", new[] { "Movie_MovieId" });
            DropTable("dbo.Movies_Persons");
            DropTable("dbo.MovieTypes");
            DropTable("dbo.Persons");
            DropTable("dbo.Movies");
        }
    }
}
