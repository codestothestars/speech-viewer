using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpeechViewer
{
    /// <summary>
    /// Performs database migrations for the API.
    /// </summary>
    public class Migrator
    {
        /// <summary>
        /// The name of the migration table.
        /// </summary>
        static string Table { get; } = "migration";

        /// <summary>
        /// The connection through which the migrator interacts with the database.
        /// </summary>
        SqlConnection Connection { get; }

        /// <summary>
        /// The application logger.
        /// </summary>
        ILogger<Migrator> Logger { get; set; }

        /// <summary>
        /// Creates a new migrator that will use the specified services.
        /// </summary>
        /// <param name="logger">The application logger.</param>
        /// <param name="connection">The connection through which the migrator will interact with the database.</param>
        public Migrator(ILogger<Migrator> logger, SqlConnection connection)
        {
            Connection = connection;
            Logger = logger;
        }

        /// <summary>
        /// Reverses all migrations that have been applied to the database and removes the migration table.
        /// </summary>
        /// <returns>A task that completes when the migrations have been reversed and the migration table has been removed.</returns>
        public virtual async Task MigrateDown()
        {
            var exists = await MigrationTableExists();

            if (!exists)
            {
                return;
            }

            foreach (var migration in GetMigrations(Direction.Down))
            {
                var name = migration.Name;

                if (await AppliedMigration(migration))
                {
                    Logger.LogInformation($"Migration {name} not yet reversed. Reversing now.");

                    await RunMigration(migration, $"DELETE FROM [{Table}] WHERE [name] = @name");
                }
                else
                {
                    Logger.LogInformation($"Migration {name} already reversed. Moving on.");
                }
            }

            using (var drop = new SqlCommand($"DROP TABLE [{Table}]", Connection))
            {
                await drop.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Applies all migrations that have not yet been applied to the database, creating the migration table first if it does not exist.
        /// </summary>
        /// <returns>A task that completes when the migrations have been applied.</returns>
        public virtual async Task MigrateUp()
        {
            var exists = await MigrationTableExists();

            if (!exists)
            {
                using (var createTable = new SqlCommand($"CREATE TABLE [{Table}]([name] NVARCHAR(64) UNIQUE NOT NULL)", Connection))
                {
                    await createTable.ExecuteNonQueryAsync();
                }
            }

            foreach (var migration in GetMigrations(Direction.Up))
            {
                var name = migration.Name;

                if (await AppliedMigration(migration))
                {
                    Logger.LogInformation($"Migration {name} already applied. Moving on.");
                }
                else
                {
                    Logger.LogInformation($"Migration {name} not yet applied. Applying now.");

                    await RunMigration(migration, $"INSERT INTO [{Table}]([name]) VALUES(@name)");
                }
            }
        }

        /// <summary>
        /// Indicates whether the specified migration has been applied to the database.
        /// </summary>
        /// <param name="migration">The migration to check.</param>
        /// <returns>A task that completes with a boolean indicating whether the migration has been applied to the database.</returns>
        async Task<bool> AppliedMigration(Migration migration)
        {
            var name = migration.Name;

            Logger.LogInformation($"Checking whether migration {name} has been applied.");

            using (var check = new SqlCommand($"SELECT 1 FROM [{Table}] WHERE [name] = @name", Connection))
            {
                var parameter = new SqlParameter("@name", SqlDbType.NChar, name.Length)
                {
                    Value = name
                };

                check.Parameters.Add(parameter);

                await check.PrepareAsync();

                var exists = await check.ExecuteScalarAsync();

                return exists != null;
            }
        }

        /// <summary>
        /// Retrieves the paths to the API's migration files in the run order appropriate for the specified direction.
        /// </summary>
        /// <param name="direction">The migration direction determining the order of the returned paths.</param>
        /// <returns>The paths to the API's migration files in the run order appropriate for the specified direction.</returns>
        IOrderedEnumerable<string> GetMigrationFiles(Direction direction)
        {
            var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var migrations = Path.GetFullPath(Path.Combine(binDirectory, "..", "migrations"));

            var files = Directory.EnumerateFiles(migrations);

            Func<string, string> byName = name => name;

            return direction == Direction.Up ? files.OrderBy(byName) : files.OrderByDescending(byName);
        }

        /// <summary>
        /// Retrieves the migrations that bring the database to the final target state intended by the specified direction
        /// (up for an up-to-date database, down for an empty database).
        ///
        /// <para>
        /// The migrations are returned in the order in which they are intended to be applied.
        /// </para>
        /// </summary>
        /// <param name="direction">The migration direction in which the retrieved migrations will bring the database.</param>
        /// <returns>The database migrations to run in the specified direction, in the run order appropriate for the specified direction.</returns>
        IEnumerable<Migration> GetMigrations(Direction direction)
        {
            var down = Direction.Down.ToString().ToLower();
            var up = Direction.Up.ToString().ToLower();

            var filePattern = new Regex($@"(?<name>[\w\-]+)-(?<direction>{down}|{up})\.sql$");

            foreach (var file in GetMigrationFiles(direction))
            {
                var match = filePattern.Match(file);

                if (!match.Success)
                {
                    Logger.LogWarning($"Skipping migration file {file} because its name does not match the expected format {filePattern}.");

                    continue;
                }

                var groups = match.Groups;

                var fileDirection = groups["direction"].Value;
                var name = groups["name"].Value;

                if (fileDirection == direction.ToString().ToLower())
                {
                    yield return new Migration
                    {
                        File = file,
                        Name = name
                    };
                }
            }
        }

        /// <summary>
        /// Indicates whether the migration table exists in the database.
        /// </summary>
        /// <returns>A task that completes with a boolean indicating whether the migration table exists.</returns>
        async Task<bool> MigrationTableExists()
        {
            using (var check = new SqlCommand($"SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{Table}'", Connection))
            {
                var exists = await check.ExecuteScalarAsync();

                return exists != null;
            }
        }

        /// <summary>
        /// Reads the contents of the specified migration's file.
        /// </summary>
        /// <param name="migration">The migration whose file contents will be read.</param>
        /// <returns>The contents of the specified migration's file.</returns>
        async Task<string> ReadMigration(Migration migration)
        {
            using (var reader = new StreamReader(migration.File))
            {
                var content = await reader.ReadToEndAsync();

                return content;
            }
        }

        /// <summary>
        /// Runs the specified migration on the database, additionally executing the specified "commit" command
        /// (generally an insert or delete into the migration table).
        ///
        /// <para>
        /// If either the migration or commit command fails, both are rolled back.
        /// </para>
        /// </summary>
        /// <param name="migration">The migration to run.</param>
        /// <param name="commitCommand">The command to run with the migration.</param>
        /// <returns>A task that completes when the migration is complete.</returns>
        async Task RunMigration(Migration migration, string commitCommand)
        {
            var content = await ReadMigration(migration);

            var name = migration.Name;

            var transaction = Connection.BeginTransaction();

            try
            {
                Logger.LogWarning("content");
                Logger.LogWarning(content);

                using (var run = new SqlCommand(content, Connection, transaction))
                {
                    await run.ExecuteNonQueryAsync();
                }

                Logger.LogWarning("commitCommand");
                Logger.LogWarning(commitCommand);

                using (var commit = new SqlCommand(commitCommand, Connection, transaction))
                {
                    var parameter = new SqlParameter("@name", SqlDbType.NChar, name.Length)
                    {
                        Value = name
                    };

                    commit.Parameters.Add(parameter);

                    await commit.PrepareAsync();

                    await commit.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, $"Failed to run migration {name}");

                try
                {
                    await transaction.RollbackAsync();
                }
                catch (Exception rollbackException)
                {
                    Logger.LogError(rollbackException, $"Failed to roll back transaction");
                }

                throw exception;
            }
        }

        /// <summary>
        /// Indicates which direction a migration operation may bring the database.
        /// </summary>
        enum Direction
        {
            /// <summary>
            /// Indicates a migration that will bring the database toward an empty state.
            /// </summary>
            Down,

            /// <summary>
            /// Indicates a migration that will bring the database toward an up-to-date state.
            /// </summary>
            Up
        }

        /// <summary>
        /// A database migration.
        /// </summary>
        class Migration
        {
            /// <summary>
            /// The full path to the migration file.
            /// </summary>
            public string File { get; set; }

            /// <summary>
            /// The migration's name.
            ///
            /// <para>
            /// This is the value stored in the database to track whether the migration has been applied.
            /// </para>
            /// </summary>
            public string Name { get; set; }
        }
    }
}
