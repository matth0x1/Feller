using Feller.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Feller.Loggers.Console
{
    public class FellerConsoleLogger : FellerChannelBackedLoggerBase, IDisposable
    {
        protected override void WriteLog(IEnumerable<KeyValuePair<string, object>> fields)
        {
            _writer.TryWrite(() => System.Console.WriteLine(JsonConvert.SerializeObject(fields)));
        }
    }
}
