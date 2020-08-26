using Feller.Loggers.Console;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Drawing;
using Feller.Tests.Utilities;
using Polly;
using Polly.Retry;

namespace Feller.Tests
{
    public partial class FellerConsoleLoggerTests
    {
        private ConsoleOutputRedirect _consoleOutput;
        private RetryPolicy<bool> _retryPolicy = Policy.HandleResult(false).WaitAndRetry(50, i => TimeSpan.FromMilliseconds(2));

        [SetUp]
        public void SetUp()
        {
            _consoleOutput = new ConsoleOutputRedirect();
        }

        [TearDown]
        public void TearDown()
        {
            // Reset the redirect and output any console messages not retrieved by a test.
            var output = _consoleOutput.GetOuptut();
            _consoleOutput.Dispose();

            if (!string.IsNullOrEmpty(output))
            {
                Console.WriteLine(output);
            }
        }

        [Test]
        public void LogsMessage()
        {
            using var logger = new FellerConsoleLogger()
            {
                CategoryName = GetType().FullName.ToString()
            };

            var timeOfLogCall = DateTime.Now;
            logger.LogInformation("Test message {TestValueA} {TestValueB}", 0.001, Color.Red);

            string log = null;

            _retryPolicy.Execute(() =>
            {
                log = _consoleOutput.GetOuptut();
                return !string.IsNullOrEmpty(log);
            });

            var deserialiedLog = JObject.Parse(log);

            Assert.IsTrue((timeOfLogCall - deserialiedLog.Value<DateTime>("Timestamp")).TotalMilliseconds < 5);
            Assert.AreEqual(0.001, deserialiedLog.Value<double>("TestValueA"));
            Assert.AreEqual(Color.Red.Name, deserialiedLog.Value<string>("TestValueB"));
            Assert.AreEqual("Test message 0.001 Color [Red]", deserialiedLog.Value<string>("Message"));
            Assert.AreEqual(2, deserialiedLog.Value<int>("Level"));
            Assert.AreEqual("Feller.Tests.FellerConsoleLoggerTests", deserialiedLog.Value<string>("CategoryName"));
        }
    }
}
