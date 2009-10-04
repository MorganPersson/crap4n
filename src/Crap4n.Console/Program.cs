using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Crap4n.Console
{
    public class Program
    {
        private readonly IoCBuilder _iocBuilder = new IoCBuilder();

        public static int Main(string[] args)
        {
            var p = new Program();
            var options = new ConsoleOptions(args);
            var output = new PlainTextOutput(System.Console.Out);
            int returnVal = p.HandleInput(options, output);
            if (returnVal != 0)
                return returnVal;

            p.Run(options, output);
            return 0;
        }

        private int HandleInput(ConsoleOptions options, PlainTextOutput output)
        {
            if (!options.nologo)
            {
                output.WriteHeader();
                output.WriteSeparator();
                output.WriteRuntimeEnvironment();
                output.WriteSeparator();
            }

            if (options.help)
            {
                options.Help();
                return 0;
            }

            if (!options.Validate())
            {
                System.Console.Error.WriteLine("fatal error: invalid arguments");
                options.Help();
                return 2;
            }
            return 0;
        }

        public void Run(ConsoleOptions options, PlainTextOutput output)
        {
            var stopWatch = new StopWatch();
            stopWatch.Start();
            var container = _iocBuilder.BuildContainer();
            var crapService = container.Resolve<CrapService>();
            IEnumerable<Crap> result = crapService.BuildResult(options.codeCoverage, options.codeMetrics, options.crapThreshold);
            WriteOutput(options, result, output);
            stopWatch.Stop();
            output.WriteLine(string.Format("Took {0:0.00}s to execute", stopWatch.TimeTaken.TotalSeconds));
        }

        private void WriteOutput(ConsoleOptions options, IEnumerable<Crap> crapResult, PlainTextOutput output)
        {
            var report = new OutputReport(crapResult, output);
            if (options.HasXmlOutput)
            {
                string xml = report.GetReportAsXml(options.crapThreshold);
                using (var stream = new StreamWriter(options.xml, false, Encoding.Unicode))
                {
                    stream.Write(xml);
                    stream.Flush();
                }
            }
            else
            {
                report.OutputMethodsAboveCrapThreshold(options.crapThreshold);
                report.OutputCrapSummary(options.crapThreshold);
            }
        }
    }

    public class StopWatch
    {
        private DateTime _started;

        public void Start()
        {
            _started = DateTime.Now;
        }

        public void Stop()
        {
            TimeTaken = DateTime.Now.Subtract(_started);
        }

        public TimeSpan TimeTaken { get; private set; }
    }
}