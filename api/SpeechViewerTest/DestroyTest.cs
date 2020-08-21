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
    public class DestroyTest
    {
        Destroy Destroy { get; set; }

        Mock<Migrator> Migrator { get; set; }

        [TestInitialize]
        public void Setup()
        {
            var connection = new SqlConnection();

            var logger = new Mock<ILogger<Migrator>>();

            Migrator = new Mock<Migrator>(logger.Object, connection);

            Destroy = new Destroy(Migrator.Object);
        }

        [TestMethod]
        public async Task Destroy_MigratesTheDatabaseDown()
        {
            Migrator
                .Setup(mgrtr => mgrtr.MigrateDown())
                .Returns(Task.CompletedTask);

            await Run();

            Migrator.Verify(mgrtr => mgrtr.MigrateDown(), Times.Once());
        }

        [TestMethod]
        public async Task Destroy_AfterTheDatabaseIsMigratedDown_Completes()
        {
            var migrated = new TaskCompletionSource<object>();

            Migrator
                .Setup(mgrtr => mgrtr.MigrateDown())
                .Returns(migrated.Task);

            var running = Run();

            Assert.IsFalse(running.IsCompleted);

            migrated.SetResult(null);

            var completed = await Task.WhenAny(running, Task.Delay(1000)) == running;

            if (!completed)
            {
                Assert.Fail("Expected Destroy to complete, but it did not.");
            }
        }

        [TestMethod]
        public async Task Destroy_ReturnsANoContentResult()
        {
            Migrator
               .Setup(mgrtr => mgrtr.MigrateDown())
               .Returns(Task.CompletedTask);

            var result = await Run();

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        Task<IActionResult> Run()
        {
            return Destroy.Run(null, null, null);
        }
    }
}
