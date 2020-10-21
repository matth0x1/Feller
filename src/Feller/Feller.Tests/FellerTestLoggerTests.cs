using Feller.Loggers.Test;
using Feller.Tests.Utilities;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;

using static Feller.Tests.Utilities.RetryPolicies;

namespace Feller.Tests
{
    class FellerTestLoggerTests
    {
        [Test]
        public void LogsMessage()
        {
            using var logger = new FellerTestLogger()
            {
                CategoryName = GetType().FullName.ToString()
            };

            var timeOfLogCall = DateTime.Now;
            logger.LogInformation("Test message {TestValueA} {TestValueB}", 0.001, PrimaryColours.Red.ToString());

            DefaultRetryPolicy.Execute(() => FellerTestLogger.Logs.Count > 1);
            FellerTestLogger.Logs.TryDequeue(out var log);

            Assert.IsNotNull(log);
            Assert.IsNotNull(log.Fields);
            Assert.IsTrue(log.Fields.Count == 6);

            Assert.IsTrue((timeOfLogCall - log.Timestamp.Value).TotalMilliseconds < 5);
            Assert.AreEqual(0.001, log.Fields["TestValueA"]);
            Assert.AreEqual(PrimaryColours.Red.ToString(), log.Fields["TestValueB"]);
            Assert.AreEqual("Test message 0.001 Red", log.Fields["Message"]);
            Assert.AreEqual(LogLevel.Information, log.Fields["Level"]);
        }
    }
}
