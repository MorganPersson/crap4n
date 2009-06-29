using NBehave.Spec.NUnit;

namespace Crap4n.Specs
{
    public abstract class CrapSpec : SpecBase
    {
        private Crap _crap;
        private double _expected = -1;

        protected override void Establish_context()
        {
            _crap = new Crap { Class = "Foo", Method = "Bar", CodeCoverage = 100.Percent(), CyclomaticComplexity = 5 };
        }

        [Specification]
        public void Should_have_a_crap_value()
        {
            _crap.Value.ShouldEqual(_expected);
        }

        public class When_having_100_percent_coverage : CrapSpec
        {
            protected override void Establish_context()
            {
                base.Establish_context();
                _crap.CodeCoverage = 100.Percent();
                _expected = 5;
            }
    
            [Specification]
            public void Should_get_formatted_result_from_ToString()
            {
                _crap.ToString().ShouldEqual("Foo.Bar : CRAP: 5,0 (CC: 5, COV: 100,0%)");
            }
        }

        public class When_having_50_percent_coverage : CrapSpec
        {
            protected override void Establish_context()
            {
                base.Establish_context();
                _crap.CodeCoverage = 50.Percent();
                _expected = 8.125;
            }
        }

        public class When_having_0_percent_coverage : CrapSpec
        {
            protected override void Establish_context()
            {
                base.Establish_context();
                _crap.CodeCoverage = 0.Percent();
                _expected = 30;
            }
        }
    }
}