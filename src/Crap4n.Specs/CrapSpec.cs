using NBehave.Spec.NUnit;
using System.Globalization;
using System.Threading;
using NUnit.Framework;


namespace Crap4n.Specs
{
    public abstract class CrapSpec : SpecBase
    {
        private Crap _crap;

        protected override void Establish_context()
        {
            _crap = new Crap(30) { Class = "Foo", Method = "Bar", CodeCoverage = 100.Percent(), CyclomaticComplexity = 5 };
        }

        public class When_having_100_percent_coverage : CrapSpec
        {
            protected override void Establish_context()
            {
                base.Establish_context();
                _crap.CodeCoverage = 100.Percent();
            }
    
            [Specification, Test]
            public void Should_get_formatted_result_from_ToString()
            {
            	string decimalPoint  = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            	string expected = string.Format("Foo.Bar : CRAP: 5{0}0 (CC: 5, COV: 100{0}0%)", decimalPoint);
                _crap.ToString().ShouldEqual(expected);
            }

            [Specification]
            public void Should_have_a_crap_value()
            {
                _crap.Value.ShouldEqual(5);
            }
        }

        public class When_having_50_percent_coverage : CrapSpec
        {
            protected override void Establish_context()
            {
                base.Establish_context();
                _crap.CodeCoverage = 50.Percent();
            }

            [Specification]
            public void Should_have_a_crap_value()
            {
                _crap.Value.ShouldEqual(8.125);
            }
        }

        public class When_having_0_percent_coverage : CrapSpec
        {
            protected override void Establish_context()
            {
                base.Establish_context();
                _crap.CodeCoverage = 0.Percent();
            }

            [Specification]
            public void Should_have_a_crap_value()
            {
                _crap.Value.ShouldEqual(30);
            }
        }

        public class When_calculating_CrapLoad : CrapSpec
        {
            
            [Specification]
            public void CrapLoad_should_be_zero_if_crap_is_less_than_crap_threshold()
            {
                var crap = new Crap(30) {CodeCoverage = 100.Percent(), CyclomaticComplexity = 1};
                crap.CrapLoad().ShouldEqual(0);
            }

            [Specification]
            public void CrapLoad_should_be_x_if_crap_is_less_than_crap_threshold()
            {
                var crap = new Crap(30) { CodeCoverage = 0.Percent(), CyclomaticComplexity = 6 };
                crap.CrapLoad().ShouldEqual(6.2);
            }
        }
    }
}