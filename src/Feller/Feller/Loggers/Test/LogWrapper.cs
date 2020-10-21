using Feller.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feller.Loggers.Test
{
    public class LogWrapper
    {
        public IDictionary<string, object> Fields { get; private set; }

        public string Message => Fields[BaseFields.Message] as string;
        public EventId? Event => Fields[BaseFields.Event] as EventId?;
        public Exception Exception => Fields[BaseFields.Exception] as Exception;
        public LogLevel? Level => Fields[BaseFields.Level] as LogLevel?;
        public DateTimeOffset? Timestamp => Fields[BaseFields.Timestamp] as DateTimeOffset?;
        public string CategoryName => Fields[BaseFields.CategoryName] as string;

        public LogWrapper(IEnumerable<KeyValuePair<string, object>> fields)
        {
            if (fields == null)
            {
                throw new ArgumentNullException(nameof(fields));
            }

            Fields = fields.ToDictionary(f => f.Key, f => f.Value);
        }
    }
}
