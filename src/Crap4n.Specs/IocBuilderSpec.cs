using System.Collections.Generic;
using System.Linq;
using NBehave.Spec.NUnit;
using Context = NUnit.Framework.TestFixtureAttribute;

namespace Crap4n.Specs
{
    [Context]
    public class IocBuilderSpec
    {
        [Specification]
        public void Should_register_services()
        {
            var p = new IoCBuilder();
            var ioc = p.BuildContainer();
            ioc.Resolve<ICrapService>().ShouldNotBeNull();
        }

        [Specification]
        public void Should_resolve_all_IFileParsers_for_CodeMetrics()
        {
            var p = new IoCBuilder();
            var ioc = p.BuildContainer();
            var parsers = ioc.Resolve<IEnumerable<IFileParser<CodeMetrics>>>();
            parsers.ShouldNotBeNull();
            parsers.Count().ShouldBeGreaterThan(0);
        }
    }
}