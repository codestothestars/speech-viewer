using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SpeechViewer
{
    /// <summary>
    /// Retrieves the content of a specified speech.
    /// </summary>
    public class GetSpeechContent
    {
        /// <summary>
        /// The connection through which the function interacts with the database.
        /// </summary>
        SqlConnection Connection { get; }

        /// <summary>
        /// Creates a new <see cref="GetSpeechContent"/> function that will use the specified services.
        /// </summary>
        /// <param name="connection">The connection through which the function will interact with the database.</param>
        public GetSpeechContent(SqlConnection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Runs the function, retrieving the content of the speech identified in the request URL.
        /// </summary>
        /// <param name="request">The HTTP request that triggered the function.</param>
        /// <param name="context">The execution context of the function.</param>
        /// <param name="logger">The function's logger.</param>
        /// <returns>An OK response containing a JSON string array of the speech paragraphs in the order in which they appear in the speech.</returns>
        [FunctionName("GetSpeechContent")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "speeches/{speech:int}/content")]
            HttpRequest request,
            ExecutionContext context,
            ILogger logger,
            int speech
        )
        {
            var paragraphs = new List<string>();

            var parameter = new SqlParameter("@speech", SqlDbType.Int)
            {
                Value = speech
            };

            var query = @"
                SELECT [text]
                FROM [speech_paragraph]
                WHERE [speech] = @speech
                ORDER BY [index]
            ";

            using (var command = new SqlCommand(query, Connection))
            {
                command.Parameters.Add(parameter);

                await command.PrepareAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        paragraphs.Add(reader.GetString(0));
                    }
                }
            }

            return new OkObjectResult(paragraphs);
        }
    }
}
