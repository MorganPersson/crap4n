using Autofac;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

		public void Run(ConsoleOptions options, ITextOutput output)
		{
			var stopWatch = new StopWatch();
			stopWatch.Start();
			var crapRunner = GetContainer().Resolve<CrapRunner>();
				crapRunner.Run(options.codeCoverage, options.codeMetrics, options.crapThreshold);
			crapRunner.WriteOutput(output);
			if (options.HasXmlOutput)
				crapRunner.WriteXmlOutput(options.xml, output);
			stopWatch.Stop();
			output.WriteLine(string.Format("Took {0:0.00}s to execute", stopWatch.TimeTaken.TotalSeconds));
		}
		
		private IContainer GetContainer()
		{
			var iocBuilder = new IoCBuilder();
			var container = iocBuilder.BuildContainer();
			return container;
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