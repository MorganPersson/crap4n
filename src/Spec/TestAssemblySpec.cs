using NBehave.Spec.NUnit;
using NUnit.Framework;
using TestAssembly;

namespace Spec
{
    [TestFixture]
    public abstract class TestAssemblySpec : SpecBase<Tested>
    {
        private int _a;
        private int _b;
        private int _c;
        private int _returnValue;

        protected override void Because_of()
        {
            _returnValue = Sut.CompleteCoverage(_a, _b, _c);
        }

        public class When_a_is_bigger_than_b_and_c : TestAssemblySpec
        {
            protected override Tested Establish_context()
            {
                _a = 3;
                _b = 2;
                _c = 1;
                return new Tested();
            }

            [Specification]
            public void should_get_value_a()
            {
                _returnValue.ShouldEqual(_a);
            }
        }

        public class When_b_is_bigger_than_a_and_c : TestAssemblySpec
        {
            protected override Tested Establish_context()
            {
                _a = 3;
                _b = 4;
                _c = 1;
                return new Tested();
            }

            [Specification]
            public void should_get_value_b()
            {
                _returnValue.ShouldEqual(_b);
            }
        }

        public class When_c_is_bigger_than_a_and_a_is_bigger_than_c : TestAssemblySpec
        {
            protected override Tested Establish_context()
            {
                _a = 3;
                _b = 2;
                _c = 5;
                return new Tested();
            }

            [Specification]
            public void should_get_value_c()
            {
                _returnValue.ShouldEqual(_c);
            }
        }

        public class When_c_is_bigger_than_b_and_b_is_bigger_than_a : TestAssemblySpec
        {
            protected override Tested Establish_context()
            {
                _a = 3;
                _b = 4;
                _c = 5;
                return new Tested();
            }

            [Specification]
            public void should_get_value_c()
            {
                _returnValue.ShouldEqual(_c);
            }
        }

        public class When_calling_SemiCoverage_with_a_larger_than_b : TestAssemblySpec
        {
            protected override Tested Establish_context()
            {
                _a = 3;
                _b = 1;
                _c = 0;
                return new Tested();
            }

            protected override void Because_of()
            {
                _returnValue = Sut.SemiCoverage(_a, _b, _c);
            }


            [Specification]
            public void should_get_value_a()
            {
                _returnValue.ShouldEqual(_a);
            }
        }
    }
}
