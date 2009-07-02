namespace Crap4n
{
    public class CodeMetrics : MetricsBase
    {
        public int CyclomaticComplexity { get; set; }
        public string SourceFile { get; set; }
        public int SourceFileLineNumber { get; set; }
    }
}