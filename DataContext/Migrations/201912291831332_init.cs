// ***********************************************************************
// Assembly         : DataContext
// Author           : Mariusz
// Created          : 12-30-2019
//
// Last Modified By : Mariusz
// Last Modified On : 01-04-2020
// ***********************************************************************
// <copyright file="201912291831332_init.cs" company="">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Class init.
    /// Implements the <see cref="System.Data.Entity.Migrations.DbMigration" />
    /// Implements the <see cref="System.Data.Entity.Migrations.Infrastructure.IMigrationMetadata" />
    /// </summary>
    /// <seealso cref="System.Data.Entity.Migrations.DbMigration" />
    /// <seealso cref="System.Data.Entity.Migrations.Infrastructure.IMigrationMetadata" />
    public partial class init : DbMigration
    {
        // W PROJECIE UZYTO METODOLOGII CODE FIRST
        // POLEGA ONA NA TYM ZE ZA POMOCA UTWORZONYCH KLAS GENETUJEMY PONIZSZY KOD KTORY TWORZY BAZE DANYCH
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        MovieId = c.Int(nullable: false, identity: true),
                        Title = c.String(unicode: false),
                        OrginalTitle = c.String(unicode: false),
                        Director = c.String(unicode: false),
                        Rank = c.Int(),
                        Year = c.Int(),
                        Duration = c.String(unicode: false),
                        Rate = c.Int(),
                        RateTotalVotes = c.Int(),
                        Description = c.String(unicode: false),
                        ReleaseDate = c.DateTime(nullable: false, precision: 0),
                        BoxOffice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Production = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.MovieId);
            
            CreateTable(
                "dbo.Persons",
                c => new
                    {
                        PersonId = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Surname = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.PersonId);
            
            CreateTable(
                "dbo.MovieTypes",
                c => new
                    {
                        MovieTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Type = c.Byte(nullable: false),
                        Description = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.MovieTypeId);
            
            CreateTable(
                "dbo.PersonDtoMovieDtoes",
                c => new
                    {
                        PersonDto_PersonId = c.Int(nullable: false),
                        MovieDto_MovieId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PersonDto_PersonId, t.MovieDto_MovieId })
                .ForeignKey("dbo.Persons", t => t.PersonDto_PersonId, cascadeDelete: true)
                .ForeignKey("dbo.Movies", t => t.MovieDto_MovieId, cascadeDelete: true);
                //.Index(t => t.PersonDto_PersonId)
                //.Index(t => t.MovieDto_MovieId);
            
            CreateTable(
                "dbo.MovieTypeDtoMovieDtoes",
                c => new
                    {
                        MovieTypeDto_MovieTypeId = c.Int(nullable: false),
                        MovieDto_MovieId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MovieTypeDto_MovieTypeId, t.MovieDto_MovieId })
                .ForeignKey("dbo.MovieTypes", t => t.MovieTypeDto_MovieTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Movies", t => t.MovieDto_MovieId, cascadeDelete: true);
                //.Index(t => t.MovieTypeDto_MovieTypeId)
                //.Index(t => t.MovieDto_MovieId);
            
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            DropForeignKey("dbo.MovieTypeDtoMovieDtoes", "MovieDto_MovieId", "dbo.Movies");
            DropForeignKey("dbo.MovieTypeDtoMovieDtoes", "MovieTypeDto_MovieTypeId", "dbo.MovieTypes");
            DropForeignKey("dbo.PersonDtoMovieDtoes", "MovieDto_MovieId", "dbo.Movies");
            DropForeignKey("dbo.PersonDtoMovieDtoes", "PersonDto_PersonId", "dbo.Persons");
            DropIndex("dbo.MovieTypeDtoMovieDtoes", new[] { "MovieDto_MovieId" });
            DropIndex("dbo.MovieTypeDtoMovieDtoes", new[] { "MovieTypeDto_MovieTypeId" });
            DropIndex("dbo.PersonDtoMovieDtoes", new[] { "MovieDto_MovieId" });
            DropIndex("dbo.PersonDtoMovieDtoes", new[] { "PersonDto_PersonId" });
            DropTable("dbo.MovieTypeDtoMovieDtoes");
            DropTable("dbo.PersonDtoMovieDtoes");
            DropTable("dbo.MovieTypes");
            DropTable("dbo.Persons");
            DropTable("dbo.Movies");
        }
    }
}
