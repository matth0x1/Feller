using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using static Feller.Core.BaseFields;

namespace Feller.Core
{
    public abstract class FellerLoggerBase : ILogger
    {
        public string CategoryName { get; set; }
        internal IExternalScopeProvider ScopeProvider { private get; set; }

        public IDisposable BeginScope<TState>(TState state)
        {
            return ScopeProvider?.Push(state);
        }

        protected abstract void Log(IEnumerable<KeyValuePair<string, object>> fields);
        private const string DefaultMessageName = "{OriginalFormat}";

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var fields = new Dictionary<string, object>();

            // Collect fields from logging scope
            ScopeProvider?.ForEachScope((scope, state) => fields.AddOrUpdate(scope as IDictionary<string, object>), fields);

            // Collect fields from log state
            if (state is IEnumerable<KeyValuePair<string, object>> stateFields)
            {
                fields.AddOrUpdate(stateFields.ToDictionary(s => s.Key, s => s.Value));
                fields.Remove(DefaultMessageName);
            }

            fields.Add(Message, formatter(state, exception));

            fields.Add(Timestamp, DateTimeOffset.Now);
            fields.Add(Level, logLevel);
            fields.Add(BaseFields.CategoryName, CategoryName);

            if (eventId != default)
            {
                fields.Add(Event, eventId);
            }

            Log(fields);
        }
    }
}
