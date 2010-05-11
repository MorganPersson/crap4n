using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;

namespace Crap4n
{
    public class IoCBuilder
    {
        public IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            RegisterWithContainer(builder);
            return builder.Build();
        }

        private void RegisterWithContainer(ContainerBuilder builder)
        {
            builder.Register<CrapRunner>().As<CrapRunner>();
            builder.Register<CrapService>().As<ICrapService>();
            var asm = typeof(CrapService).Assembly;

            builder.RegisterCollection<IFileParser<CodeMetrics>>().As<IEnumerable<IFileParser<CodeMetrics>>>();
            builder.RegisterCollection<IFileParser<CodeCoverage>>().As<IEnumerable<IFileParser<CodeCoverage>>>();

            IEnumerable<Type> types = GetTypes(asm);
            foreach (var type in types)
            {
                foreach (var @interface in type.GetInterfaces())
                {
                    builder.Register(type).As(@interface);
                    if (@interface == typeof(IFileParser<CodeMetrics>))
                        builder.Register(type).As(@interface).MemberOf(typeof(IEnumerable<IFileParser<CodeMetrics>>));
                    if (@interface == typeof(IFileParser<CodeCoverage>))
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