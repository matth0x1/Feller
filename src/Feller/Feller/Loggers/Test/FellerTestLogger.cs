using Feller.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Feller.Loggers.Test
{
    public class FellerTestLogger : FellerChannelBackedLoggerBase, IDisposable
    {
        public static readonly ConcurrentQueue<LogWrapper> Logs = new ConcurrentQueue<LogWrapper>();

        protected override void WriteLog(IEnumerable<KeyValuePair<string, object>> fields)
        {
            _writer.TryWrite(() => Logs.Enqueue(new LogWrapper(fields)));
        }
    }
}
