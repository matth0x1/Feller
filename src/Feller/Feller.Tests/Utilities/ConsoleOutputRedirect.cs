using System;
using System.IO;
using System.Text;

namespace Feller.Tests.Utilities
{
    public class ConsoleOutputRedirect : IDisposable
    {
        private TextWriter _originalOutput;
        private StringWriter _consoleOutWriter;
        private StringBuilder _sb = new StringBuilder();

        public ConsoleOutputRedirect()
        {
            _consoleOutWriter = new StringWriter(_sb);

            _originalOutput = Console.Out;
            Console.SetOut(_consoleOutWriter);
        }

        public string GetOuptut()
        {
            _consoleOutWriter.Flush();
            var output = _sb.ToString();
            _sb.Clear();

            return output;
        }

        public void Dispose()
        {
            Console.SetOut(_originalOutput);

            _consoleOutWriter.Dispose();
        }
    }
}
