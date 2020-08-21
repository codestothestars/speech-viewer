using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.SqlClient;

[assembly: FunctionsStartup(typeof(SpeechViewer.Startup))]

namespace SpeechViewer
{
    /// <summary>
    /// Configures startup behavior for the speech viewer API.
    /// </summary>
    public class Startup : FunctionsStartup
    {
        /// <summary>
        /// Configures services for the application.
        /// </summary>
        /// <param name="builder">The functions host builder with which services will be registered.</param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;

            services.AddScoped<SqlConnection>(provider =>
            {
                var connectionString = new SqlConnectionStringBuilder
                {
                    DataSource = GetEnvironmentVariable("databaseServer"),
                    InitialCatalog = GetEnvironmentVariable("databaseName"),
                    Password = GetEnvironmentVariable("databasePassword"),
                    UserID = GetEnvironmentVariable("databaseUser")
                };

                var connection = new SqlConnection(connectionString.ConnectionString);

                connection.Open();

                return connection;
            });

            services.AddTransient<Migrator>();
        }

        /// <summary>
        /// Retrieves the value of the environment variable with the specified name.
        /// </summary>
        /// <param name="variable">The name of the environment variable to retrieve.</param>
        /// <returns>The value of the specified environment variable.</returns>
        static string GetEnvironmentVariable(string variable)
        {
            return Environment.GetEnvironmentVariable(variable);
        }
    }
}
