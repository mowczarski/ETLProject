// ***********************************************************************
// Assembly         : DataContext
// Author           : Mariusz
// Created          : 12-30-2019
//
// Last Modified By : Mariusz
// Last Modified On : 12-30-2019
// ***********************************************************************
// <copyright file="201912291831332_init.designer.cs" company="">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataContext.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;

    /// <summary>
    /// Class init. This class cannot be inherited.
    /// Implements the <see cref="System.Data.Entity.Migrations.DbMigration" />
    /// Implements the <see cref="System.Data.Entity.Migrations.Infrastructure.IMigrationMetadata" />
    /// </summary>
    /// <seealso cref="System.Data.Entity.Migrations.DbMigration" />
    /// <seealso cref="System.Data.Entity.Migrations.Infrastructure.IMigrationMetadata" />
    [GeneratedCode("EntityFramework.Migrations", "6.4.0")]
    public sealed partial class init : IMigrationMetadata
    {
        /// <summary>
        /// The resources
        /// </summary>
        private readonly ResourceManager Resources = new ResourceManager(typeof(init));

        /// <summary>
        /// Gets the unique identifier for the migration.
        /// </summary>
        /// <value>The identifier.</value>
        string IMigrationMetadata.Id
        {
            get { return "201912291831332_init"; }
        }

        /// <summary>
        /// Gets the state of the model before this migration is run.
        /// </summary>
        /// <value>The source.</value>
        string IMigrationMetadata.Source
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <value>The target.</value>
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}
