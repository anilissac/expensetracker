using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ExpenseTracker.DAL.Configurations
{
    public class ETConfig : IETConfig
    {
        private readonly IConfiguration _configuration;
        public ETConfig(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public string DatabaseConnection
        {
            get
            {
                return this._configuration["ConnectionStrings:DatabaseConnection"];
            }
            set { }
        }

        public string GetConnectionString(string connectionName)
        {
            return this._configuration.GetConnectionString(connectionName);
        }

        public IConfigurationSection GetConfigurationSection(string key)
        {
            return this._configuration.GetSection(key);
        }
    }
}
