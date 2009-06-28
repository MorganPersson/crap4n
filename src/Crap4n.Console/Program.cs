using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;

namespace Crap4n.Console
{
    public class Program
    {
        // params to implement cc (code coverage file), cm (Code Metrics file), xml (xml output)
        public static int Main(string[] args)
        {
            var p = new Program();
            var options = new ConsoleOptions(args);
            var output = new PlainTextOutput(System.Console.Out);
            int returnVal = p.HandleInput(options, output);
            if (returnVal != 0)
                return returnVal;

            p.Run(options, output);
            System.Console.Write("Press any key to continue . . . ");
            System.Console.ReadKey(true);
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

        private void Run(ConsoleOptions options, PlainTextOutput output)
        {
            CrapService crapService = GetCrapService();
            IEnumerable<Crap> result = crapService.BuildResult(options.codeCoverage, options.codeMetrics);
            var sortedResult = from r in result
                               orderby r.Value
                               select r;
            WriteOutput(options, sortedResult, output);
        }

        private void WriteOutput(ConsoleOptions options, IEnumerable<Crap> sortedResult, PlainTextOutput output)
        {
            if (options.HasXmlOutput)
            {
                //TODO: implement
                output.WriteLine("xml output not implemented yet");
            }
            else
            {
                foreach (var crap in sortedResult)
                    output.WriteLine(crap.ToString());
            }
        }

        private CrapService GetCrapService()
        {
            IContainer container = GetContainer();
            return container.Resolve<CrapService>();
        }

        public IContainer GetContainer()
        {
            var builder = new ContainerBuilder();
            RegisterWithContainer(builder);
            return builder.Build();
        }

        private void RegisterWithContainer(ContainerBuilder builder)
        {
            builder.Register<CrapService>().As<CrapService>();
            var asm = typeof (CrapService).Assembly;

            builder.RegisterCollection<IFileParser<CodeMetrics>>().As<IEnumerable<IFileParser<CodeMetrics>>>();
            builder.RegisterCollection<IFileParser<CodeCoverage>>().As<IEnumerable<IFileParser<CodeCoverage>>>();

            IEnumerable<Type> types = GetTypes(asm);
            foreach (var type in types)
            {
                foreach (var @interface in type.GetInterfaces())
                {
                    builder.Register(type).As(@interface);
                    if (@interface == typeof (IFileParser<CodeMetrics>))
                        builder.Register(type).As(@interface).MemberOf(typeof (IEnumerable<IFileParser<CodeMetrics>>));
                    if (@interface == typeof (IFileParser<CodeCoverage>))
                        builder.Register(type).As(@interface).MemberOf<IEnumerable<IFileParser<CodeCoverage>>>();
                }
            }
        }

        private IEnumerable<Type> GetTypes(Assembly asm)
        {
            return from t in asm.GetTypes()
                   where t.IsAbstract == false
                         && t.IsInterface == false
                   select t;
        }
    }
}