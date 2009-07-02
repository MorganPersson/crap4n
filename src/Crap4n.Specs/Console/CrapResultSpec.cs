using System;
using System.Collections.Generic;
using System.Linq;
using Crap4n.Console.DataContract;
using Crap4n.Specs.Helper;
using NBehave.Spec.NUnit;

namespace Crap4n.Specs.Console
{
    [Context]
    public class CrapResultSpec : SpecBase<CrapResult>
    {
        protected override CrapResult Establish_context()
        {
            var crappyMethod = CrapBuilder.CrappyMethod();
            var nonCrappyMethod = CrapBuilder.GoodMethod();

            var crap = new List<Crap> { crappyMethod, nonCrappyMethod };
            const int crapThreshold = 20;
            Func<int, IEnumerable<Crap>> aboveThresHold = f => new List<Crap> { crappyMethod };
            return CrapResult.Build(crap, aboveThresHold, crapThreshold);
        }

        [Specification]
        public void Should_map_to_Summary()
        {
            Sut.Summary.ShouldNotBeNull();
            Sut.Summary.CrappyMethods.ShouldEqual(1);
            Sut.Summary.CrapThreshold.ShouldEqual(20);
            Sut.Summary.TotalMethods.ShouldEqual(2);
            Sut.Summary.CrapLoad.ShouldBeGreaterThan(0);
        }

        [Specification]
        public void Should_have_2_methods()
        {
            Sut.Methods.ShouldNotBeNull();
            Sut.Methods.Count.ShouldNotBeNull();
        }

        [Specification]
        public void should_map_crap_result_to_method_result()
        {
            CrapMethod method = Sut.Methods.First();
            method.Namespace.ShouldNotBeEmpty();
            method.Class.ShouldNotBeEmpty();
            method.MethodName.ShouldNotBeEmpty();
            method.MethodSignature.ShouldNotBeEmpty();
            method.CodeCoverage.ShouldBeGreaterThan(0);
            method.CyclomaticComplexity.ShouldBeGreaterThan(0);
            method.CrapValue.ShouldBeGreaterThan(0);
        }

        [Specification]
        public void should_map_source_file_info_result()
        {
            CrapMethod method = Sut.Methods.First();

            method.SourceFile.ShouldNotBeNull();
            method.SourceFile.FileName.ShouldNotBeEmpty();
            method.SourceFile.LineNumber.ShouldBeGreaterThan(0);
        }
    }
}