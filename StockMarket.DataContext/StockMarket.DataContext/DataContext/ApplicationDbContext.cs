using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using StockMarket.Models.StocksModel;
using StockMarket.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.DataContext.DataContext
{
    public class ApplicationDbContext : IdentityDbContext<UsersModel>
    {
        #region Overloaded Constructor With DbContext Options
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="dbContextOptions"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(options: dbContextOptions)
        {
            try
            {
                var relationalDatabaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (relationalDatabaseCreator is not null)
                {
                    if (!relationalDatabaseCreator.Exists())
                    {
                        relationalDatabaseCreator.Create();
                    }
                    if (!relationalDatabaseCreator.HasTables())
                    {
                        relationalDatabaseCreator.Create();
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error Message: {exception.Message}");
            }
        } 
        #endregion

        #region DbSet To Create Tables in Database
        /// <summary>
        /// 
        /// </summary>
        public DbSet<StockModel> TblStocks { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<CommentModel> TblComments { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<PortfolioModel> TblPortfolio { get; set; }
        #endregion

        #region On Model Creating
        /// <summary>
        /// OnModelCreating
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Adding Many To Many Relationships Among the Database Tables */
            builder.Entity<PortfolioModel>(portfolio => portfolio.HasKey(user => new { user.UserId, user.StockId }));
            builder.Entity<PortfolioModel>().HasOne(user => user.UsersModel).WithMany(user => user.Portfolios).HasForeignKey(user => user.UserId);
            builder.Entity<PortfolioModel>().HasOne(user => user.StockModel).WithMany(user => user.Portfolios).HasForeignKey(user => user.StockId);

            /* Adding Identity Roles to Database (Table: AspNetRoles) */
            List<IdentityRole> roles = new()
            {
                new IdentityRole()
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                },

                new IdentityRole()
                {
                    Name = "User",
                    NormalizedName = "USER",
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        } 
        #endregion
    }
}
