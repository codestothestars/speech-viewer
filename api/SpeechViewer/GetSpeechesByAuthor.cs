using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechViewer
{
    /// <summary>
    /// Retrieves basic info on all speeches stored by the API, grouped by author.
    /// </summary>
    public class GetSpeechesByAuthor
    {
        /// <summary>
        /// The connection through which the function interacts with the database.
        /// </summary>
        SqlConnection Connection { get; }

        /// <summary>
        /// Creates a new <see cref="GetSpeechesByAuthor"/> function that will use the specified services.
        /// </summary>
        /// <param name="connection">The connection through which the function will interact with the database.</param>
        public GetSpeechesByAuthor(SqlConnection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Runs the function, retrieving basic info on all speeches stored by the API, grouped by author.
        /// </summary>
        /// <param name="request">The HTTP request that triggered the function.</param>
        /// <param name="context">The execution context of the function.</param>
        /// <param name="logger">The function's logger.</param>
        /// <returns>An OK response containing a JSON array of <see cref="SpeechesByAuthor"/>s.</returns>
        [FunctionName("GetSpeechesByAuthor")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "speeches/by-author")]
            HttpRequest request,
            ExecutionContext context,
            ILogger logger
        )
        {
            var query = @"
                SELECT [author].[id], [author].[first_name], [author].[last_name], [speech].[id], [speech].[name]
                FROM [author] JOIN [speech] ON [author].[id] = [speech].[author]
            ";

            var results = new List<SpeechByAuthor>();

            using (var command = new SqlCommand(query, Connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        results.Add(new SpeechByAuthor
                        {
                            AuthorId = reader.GetInt32(0),
                            AuthorFirstName = reader.GetString(1),
                            AuthorLastName = reader.GetString(2),
                            SpeechId = reader.GetInt32(3),
                            SpeechName = reader.GetString(4)
                        });
                    }
                }
            }

            var speechesByAuthor = results.GroupBy(result => result.AuthorId).Select(group =>
            {
                var author = group.First();

                return new SpeechesByAuthor
                {
                    Author = new AuthorInfo
                    {
                        FirstName = author.AuthorFirstName,
                        Id = author.AuthorId,
                        LastName = author.AuthorLastName
                    },
                    Speeches = group
                        .Select(result => new AuthorSpeechInfo
                        {
                            Id = result.SpeechId,
                            Name = result.SpeechName
                        })
                        .ToArray()
                };
            });

            return new OkObjectResult(speechesByAuthor);
        }

        /// <summary>
        /// An author-speech pair retrieved from the database.
        /// </summary>
        class SpeechByAuthor
        {
            /// <summary>
            /// The author's identifier.
            /// </summary>
            public int AuthorId { get; set; }

            /// <summary>
            /// The author's first name.
            /// </summary>
            public string AuthorFirstName { get; set; }

            /// <summary>
            /// The author's last name.
            /// </summary>
            public string AuthorLastName { get; set; }

            /// <summary>
            /// The speech's identifier.
            /// </summary>
            public int SpeechId { get; set; }

            /// <summary>
            /// The speech's name.
            /// </summary>
            public string SpeechName { get; set; }
        }
    }
}
