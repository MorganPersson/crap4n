using Codeblast;

namespace Crap4n.Console
{
    public class ConsoleOptions : CommandLineOptions
    {
        private bool _isInvalid;

        [Option(Short = "c", Description = "")]
        public int crapThreshold = 30;

        [Option(Short = "cc", Description = "CodeCoverage file to use. Ex: CodeCoverage:PartCoverResult.xml")]
        public string codeCoverage;

        [Option(Short = "x", Description = "Xml file to store crap output")]
        public string xml;

        [Option(Short = "cm", Description = "CodeMetrics file to use. Ex: CodeMetrics:SourceMonitorResult.xml")]
        public string codeMetrics;

        [Option(Short = "?", Description = "Display help")]
        public bool help;

        [Option(Description = "Do not display the logo")]
        public bool nologo;

        public ConsoleOptions(params string[] args)
            : base(args)
        {
            _isInvalid = false;
        }

        public bool HasCodeMetrics
        {
            get
            {
                return !string.IsNullOrEmpty(codeMetrics);
            }
        }

        public bool HasCodeCodeCoverage
        {
            get
            {
                return !string.IsNullOrEmpty(codeCoverage);
            }
        }

        public bool HasXmlOutput
        {
            get
            {
                return !string.IsNullOrEmpty(xml);
            }
        }

        public bool IsInvalid
        {
            get { return _isInvalid; }
        }

        public override void Help()
        {

            System.Console.WriteLine();
            System.Console.WriteLine("Crap4n-Console [inputfiles] [options]");
            System.Console.WriteLine();
            System.Console.WriteLine("Calculate CRAP values from the console.");
            System.Console.WriteLine();
            System.Console.WriteLine("Options:");
            base.Help();
            System.Console.WriteLine();
            System.Console.WriteLine("Options that take values may use an equal sign, a colon");
            System.Console.WriteLine("or a space to separate the option from its value.");
            System.Console.WriteLine();
        }

        public bool Validate()
        {
            if (_isInvalid)
                return false;

            if (!HasCodeCodeCoverage && !HasCodeMetrics)
                return false;

            return ParameterCount == 0;
        }

        protected override void InvalidOption(string name)
        {
            _isInvalid = true;
        }
    }
}