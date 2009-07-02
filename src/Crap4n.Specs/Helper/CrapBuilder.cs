namespace Crap4n.Specs.Helper
{
    public static class CrapBuilder
    {
        public static Crap GoodMethod()
        {
            return GoodMethod(30);
        }
        public static Crap GoodMethod(int crapThreshold)
        {
            return new Crap(crapThreshold)
                       {
                           Class = "class",
                           CodeCoverage = 100.Percent(),
                           CyclomaticComplexity = 4,
                           Method = "notSoCrappyMethod",
                           MethodSignature = "int (int, string)",
                           NameSpace = "namespace",
                           SourceFile = "source.cs",
                           SourceFileLineNumber = 22
                       };
        }

        public static Crap CrappyMethod()
        {
            return CrappyMethod(30);
        }

        public static Crap CrappyMethod(int crapThreshold)
        {
            return new Crap(crapThreshold)
                       {
                           Class = "class",
                           CodeCoverage = 1.Percent(),
                           CyclomaticComplexity = 10,
                           Method = "aCrappyMethod",
                           MethodSignature = "int (int, string)",
                           NameSpace = "namespace",
                           SourceFile = "source.cs",
                           SourceFileLineNumber = 42
                       };
        }
    }
}