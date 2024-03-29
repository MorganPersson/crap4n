﻿using System.Diagnostics;
using Autofac;

namespace Crap4n.Console
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var p = new Program();
            var options = new ConsoleOptions(args);
            var output = new PlainTextOutput(System.Console.Out);
            int returnVal = p.HandleInput(options, output);
            if (returnVal != 0)
                return returnVal;

            if (!options.help)
                p.Run(options, output);
            return 0;
        }

        private int HandleInput(ConsoleOptions options, ITextOutput output)
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

        private void Run(ConsoleOptions options, ITextOutput output)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var crapRunner = GetContainer().Resolve<CrapRunner>();
            crapRunner.Run(options.codeCoverage, options.codeMetrics, options.crapThreshold);
            crapRunner.WriteOutput(output);
            if (options.HasXmlOutput)
                crapRunner.WriteXmlOutput(options.xml, output);
            stopWatch.Stop();
            output.WriteLine(string.Format("Took {0:0.00}s to execute", stopWatch.Elapsed.TotalSeconds));
        }

        private IContainer GetContainer()
        {
            var iocBuilder = new IoCBuilder();
            var container = iocBuilder.BuildContainer();
            return container;
        }
    }
}