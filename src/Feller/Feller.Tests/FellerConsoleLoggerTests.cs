using Feller.Loggers.Console;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Drawing;
using Feller.Tests.Utilities;

namespace Feller.Tests
{
    public partial class FellerConsoleLoggerTests
    {
        private ConsoleOutputRedirect _consoleOutput;

        [SetUp]
        public void SetUp()
        {
            _consoleOutput = new ConsoleOutputRedirect();
        }

        [TearDown]
        public void TearDown()
        {
            // Reset the redirect and output any console messages not retrieved by a test.
            var output = _consoleOutput.GetOuptutAsString();
            _consoleOutput.Dispose();
            Console.WriteLine(output);
        }

        [Test]
        public void LogsMessage()
        {
            var logger = new FellerConsoleLogger();

            logger.LogInformation("Test message {TestValueA} {TestValueB}", 0.001, Color.Red);

            using var consoleOutput = _consoleOutput.GetOuptutAsStringReader();
            var log = consoleOutput.ReadLine();
            var deserialiedLog = JObject.Parse(log);

            Assert.IsTrue((DateTime.Now - deserialiedLog.Value<DateTime>("Timestamp")).TotalMilliseconds < 100);
            Assert.AreEqual(0.001, deserialiedLog.Value<double>("TestValueA"));
            Assert.AreEqual(Color.Red.Name, deserialiedLog.Value<string>("TestValueB"));
            Assert.AreEqual("Test message 0.001 Color [Red]", deserialiedLog.Value<string>("Message"));
            Assert.AreEqual(2, deserialiedLog.Value<int>("Level"));
        }
    }
}
