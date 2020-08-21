using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace SpeechViewer
{
    /// <summary>
    /// Removes all content managed by the speech API, including its schema and data.
    ///
    /// <para>
    /// Has no effect if the API schema already does not exist.
    /// </para>
    /// </summary>
    public class Destroy
    {
        /// <summary>
        /// The migrator that the function uses to perform database migrations.
        /// </summary>
        Migrator Migrator { get; set; }

        /// <summary>
        /// Creates a new <see cref="Destroy"/> function that will use the specified services.
        /// </summary>
        /// <param name="migrator">The migrator that the function will use to perform database migrations.</param>
        public Destroy(Migrator migrator)
        {
            Migrator = migrator;
        }

        /// <summary>
        /// Runs the function, destroying the speech API data.
        /// </summary>
        /// <param name="request">The HTTP request that triggered the function.</param>
        /// <param name="context">The execution context of the function.</param>
        /// <param name="logger">The function's logger.</param>
        /// <returns>An No-Content response signifying the operation's success.</returns>
        [FunctionName("Destroy")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "destroy")]
            HttpRequest request,
            ExecutionContext context,
            ILogger logger
        )
        {
            await Migrator.MigrateDown();

            return new NoContentResult();
        }
    }
}
