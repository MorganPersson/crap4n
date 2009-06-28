using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Crap4n.Console;
using NBehave.Spec.NUnit;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Crap4n.Specs.Console
{
    [Context]
    public class ProgamSpec
    {
        private TextWriter _original;
        private StringBuilder _output;

        [SetUp]
        public void Redirect_console_out()
        {
            _original = System.Console.Out;

            _output = new StringBuilder();
            TextWriter writer = new StringWriter(_output);
            System.Console.SetOut(writer);
        }

        [TearDown]
        public void Restore_console_out()
        {
            if (_original != null)
                System.Console.SetOut(_original);
        }

        [Specification]
        public void Should_not_display_header_when_nologo_argument_set()
        {
            Program.Main(new[] { "/nologo" });

            Assert.That(_output.ToString(), Text.DoesNotContain("Copyright"));
        }

        [Specification]
        public void Should_register_services()
        {
            var p = new Program();
            var ioc = p.GetContainer();
            ioc.Resolve<CrapService>().ShouldNotBeNull();
        }

        [Specification]
        public void Should_resolve_all_IFileParsers_for_CodeMetrics()
        {
            var p = new Program();
            var ioc = p.GetContainer();
            var parsers = ioc.Resolve<IEnumerable<IFileParser<CodeMetrics>>>();
            parsers.ShouldNotBeNull();
            parsers.Count().ShouldBeGreaterThan(0);
        }
    }
}
