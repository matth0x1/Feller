using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Feller.Core
{
    public abstract class FellerChannelBackedLoggerBase : FellerLoggerBase, IDisposable
    {
        protected ChannelWriter<Action> _writer;
        protected ChannelReader<Action> _reader;

        public FellerChannelBackedLoggerBase()
        {
            var channel = Channel.CreateUnbounded<Action>(new UnboundedChannelOptions() { SingleReader = true });
            _reader = channel.Reader;
            _writer = channel.Writer;

            Task.Run(async () => await ProcessLoggingRequests());
        }

        private async Task ProcessLoggingRequests()
        {
            while (await _reader.WaitToReadAsync())
            {
                // Fast loop around available jobs
                while (_reader.TryRead(out var job))
                {
                    job.Invoke();
                }
            }
        }

        protected override void Log(IEnumerable<KeyValuePair<string, object>> fields)
        {
            _writer.TryWrite(() => WriteLog(fields));
        }

        protected abstract void WriteLog(IEnumerable<KeyValuePair<string, object>> fields);

        public void Dispose()
        {
            _writer.Complete();
        }
    }
}
