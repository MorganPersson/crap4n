using System;
using System.IO;
using System.Reflection;

namespace Crap4n
{
    public interface ITextOutput
    {
        void WriteLine(string text);
        void WriteHeader();
        void WriteSeparator();
        void WriteRuntimeEnvironment();
    }

    public class PlainTextOutput : ITextOutput
    {
        private readonly TextWriter _writer;

        public PlainTextOutput(TextWriter writer)
        {
            _writer = writer;
        }

        void ITextOutput.WriteLine(string text)
        {
            _writer.WriteLine(text);
        }

        void ITextOutput.WriteHeader()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            Version version = executingAssembly.GetName().Version;

            var copyrights = (AssemblyCopyrightAttribute[])
                             Attribute.GetCustomAttributes(executingAssembly, typeof (AssemblyCopyrightAttribute));

            _writer.WriteLine("crap4n version {0}", version);

            foreach (AssemblyCopyrightAttribute copyrightAttribute in copyrights)
            {
                _writer.WriteLine(copyrightAttribute.Copyright);
            }

            if (copyrights.Length > 0)
                _writer.WriteLine("All Rights Reserved.");
        }

        void ITextOutput.WriteSeparator()
        {
            _writer.WriteLine("");
        }

        void ITextOutput.WriteRuntimeEnvironment()
        {
            string runtimeEnv =
                string.Format("Runtime Environment -\r\n   OS Version: {0}\r\n  CLR Version: {1}", Environment.OSVersion,
                              Environment.Version);
            _writer.WriteLine(runtimeEnv);
        }
    }
}