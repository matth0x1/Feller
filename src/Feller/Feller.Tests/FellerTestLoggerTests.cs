using Feller.Loggers.Test;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Polly;
using Polly.Retry;
using System;
using System.Drawing;

namespace Feller.Tests
{
    class FellerTestLoggerTests
    {
        private RetryPolicy<bool> _retryPolicy = Policy.HandleResult(false).WaitAndRetry(500, i => TimeSpan.FromMilliseconds(2));

        [Test]
        public void LogsMessage()
        {
            using var logger = new FellerTestLogger()
            {
                CategoryName = GetType().FullName.ToString()
            };

            var timeOfLogCall = DateTime.Now;
            logger.LogInformation("Test message {TestValueA} {TestValueB}", 0.001, Color.Red);

            _retryPolicy.Execute(() => FellerTestLogger.Logs.Count > 1);
            FellerTestLogger.Logs.TryDequeue(out var log);

            Assert.IsNotNull(log);
            Assert.IsNotNull(log.Fields);
            Assert.IsTrue(log.Fields.Count == 6);

            //Assert.IsTrue((timeOfLogCall - deserialiedLog.Value<DateTime>("Timestamp")).TotalMilliseconds < 5);
            //Assert.AreEqual(0.001, deserialiedLog.Value<double>("TestValueA"));
            //Assert.AreEqual(Color.Red.Name, deserialiedLog.Value<string>("TestValueB"));
            //Assert.AreEqual("Test message 0.001 Color [Red]", deserialiedLog.Value<string>("Message"));
            //Assert.AreEqual(2, deserialiedLog.Value<int>("Level"));
        }
    }
}
