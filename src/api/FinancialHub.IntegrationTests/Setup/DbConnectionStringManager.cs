using System;
using System.IO;
using System.Runtime.InteropServices;

namespace FinancialHub.IntegrationTests.Setup
{
    internal static class DbConnectionStringManager
    {
        private const string DockerConnectionString = "Server=localhost,1450;Database=financial_hub;user=sa;pwd=P@ssw0rd!;";
        private const string LocalDbConnectionString = "Server=(LocalDB)\\MSSQLLocalDB;Database=financial_hub;Trusted_Connection=True;";

        public static string ConnectionString
        {
            get
            {   
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && LocalDbExists())
                {
                    return LocalDbConnectionString;
                }

                return DockerConnectionString;
            }
        }
        private static readonly string sqlServerLocalDbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            "Microsoft SQL Server"
        );

        private static bool LocalDbExists()
        {
            var files =  Directory.GetFiles(
                sqlServerLocalDbPath,
                "SqlLocalDB.exe",
                new EnumerationOptions() 
                { 
                    IgnoreInaccessible = true,
                    RecurseSubdirectories = true
                }
           );

            return files.Length > 0;
        }
    }
}
