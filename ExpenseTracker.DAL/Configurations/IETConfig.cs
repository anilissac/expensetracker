using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ExpenseTracker.DAL.Configurations
{
    public interface IETConfig
    {
        string DatabaseConnection { get; set; }
        string GetConnectionString(string connectionName);
        IConfigurationSection GetConfigurationSection(string Key);
    }
}
