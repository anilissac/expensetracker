using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using ExpenseTracker.DAL.Models;
namespace ExpenseTracker.DAL.Configurations
{
    /// <summary>
    /// Overriding partial class of entity db context -Qourt_CoreContext to avoid overwriting of connection string in every scafold update.
    /// </summary>
    public partial class ETDBContext : MEDSIMSContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
                var connectionString = configuration.GetConnectionString("DatabaseConnection");

                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
