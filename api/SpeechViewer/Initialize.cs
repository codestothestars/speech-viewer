using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace SpeechViewer
{
    /// <summary>
    /// Initializes the speech API, including its schema and default data.
    ///
    /// <para>
    /// Has no effect if the API has already been initialized.
    /// </para>
    /// </summary>
    public class Initialize
    {
        /// <summary>
        /// The migrator that the function uses to perform database migrations.
        /// </summary>
        Migrator Migrator { get; set; }

        /// <summary>
        /// Creates a new <see cref="Initialize"/> function that will use the specified services.
        /// </summary>
        /// <param name="migrator">The migrator that the function will use to perform database migrations.</param>
        public Initialize(Migrator migrator)
        {
            Migrator = migrator;
        }

        /// <summary>
        /// Runs the function, initializing the speech API data.
        /// </summary>
        /// <param name="request">The HTTP request that triggered the function.</param>
        /// <param name="context">The execution context of the function.</param>
        /// <param name="logger">The function's logger.</param>
        /// <returns>An No-Content response signifying the operation's success.</returns>
        [FunctionName("Initialize")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "initialize")]
            HttpRequest request,
            ExecutionContext context,
            ILogger logger
        )
        {
            await Migrator.MigrateUp();

            return new NoContentResult();
        }
    }
}
