using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Feller.Core
{
    public class FellerLoggerProvider<TLogger> : ILoggerProvider
        where TLogger : FellerLoggerBase
    {
        protected readonly IServiceProvider ServiceProvider;
        protected readonly ConcurrentDictionary<string, TLogger> Loggers = new ConcurrentDictionary<string, TLogger>();
        protected IExternalScopeProvider ScopeProvider { get; set; }

        public FellerLoggerProvider(IServiceProvider serviceProvider, IExternalScopeProvider? scopeProvider)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            ScopeProvider = scopeProvider ?? new LoggerExternalScopeProvider();
        }

        public FellerLoggerProvider(IServiceProvider serviceProvider)
            : this(serviceProvider, null) { }

        public ILogger CreateLogger(string categoryName)
        {
            TLogger BuildLogger(string categoryName)
            {
                var logger = ServiceProvider.GetRequiredService<TLogger>();

                if (logger == null)
                {
                    throw new NullReferenceException($"Logger: {typeof(TLogger).FullName}. Category: '{categoryName}'");
                }

                logger.ScopeProvider = ScopeProvider;
                logger.CategoryName = categoryName;

                return logger;
            }

            return Loggers.GetOrAdd(categoryName, BuildLogger);
        }

        public void Dispose()
        {
            foreach (var logger in Loggers.OfType<IDisposable>())
            {
                logger.Dispose();
            }

            Loggers.Clear();
        }
    }
}
