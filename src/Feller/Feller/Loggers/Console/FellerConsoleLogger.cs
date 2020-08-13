using Feller.Core;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Feller.Loggers.Console
{
    public class FellerConsoleLogger : FellerLoggerBase
    {
        protected override void Log(IEnumerable<KeyValuePair<string, object>> fields)
        {
            System.Console.WriteLine(JsonConvert.SerializeObject(fields));
        }
    }
}
