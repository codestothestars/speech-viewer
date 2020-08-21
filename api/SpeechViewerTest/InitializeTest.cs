using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SpeechViewer;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SpeechViewerTest
{
    [TestClass]
    public class InitializeTest
    {
        Initialize Initialize { get; set; }

        Mock<Migrator> Migrator { get; set; }

        [TestInitialize]
        public void Setup()
        {
            var connection = new SqlConnection();

            var logger = new Mock<ILogger<Migrator>>();

            Migrator = new Mock<Migrator>(logger.Object, connection);

            Initialize = new Initialize(Migrator.Object);
        }

        [TestMethod]
        public async Task Initialize_MigratesTheDatabaseDown()
        {
            Migrator
                .Setup(mgrtr => mgrtr.MigrateUp())
                .Returns(Task.CompletedTask);

            await Run();

            Migrator.Verify(mgrtr => mgrtr.MigrateUp(), Times.Once());
        }

        [TestMethod]
        public async Task Initialize_AfterTheDatabaseIsMigratedDown_Completes()
        {
            var migrated = new TaskCompletionSource<object>();

            Migrator
                .Setup(mgrtr => mgrtr.MigrateUp())
                .Returns(migrated.Task);

            var running = Run();

            Assert.IsFalse(running.IsCompleted);

            migrated.SetResult(null);

            var completed = await Task.WhenAny(running, Task.Delay(1000)) == running;

            if (!completed)
            {
                Assert.Fail("Expected Initialize to complete, but it did not.");
            }
        }

        [TestMethod]
        public async Task Initialize_ReturnsANoContentResult()
        {
            Migrator
               .Setup(mgrtr => mgrtr.MigrateUp())
               .Returns(Task.CompletedTask);

            var result = await Run();

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        Task<IActionResult> Run()
        {
            return Initialize.Run(null, null, null);
        }
    }
}
