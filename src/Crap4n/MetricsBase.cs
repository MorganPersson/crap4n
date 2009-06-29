using System;

namespace Crap4n
{
    public abstract class MetricsBase
    {
        public string Class { get; set; }
        public string Method { get; set; }
        public string NameSpace { get; set; }
    }

    public interface IFoo
    {
        string Bar { get; set; }
    }

    public class Foo : IFoo
    {
        string IFoo.Bar { get; set; }        
    }
}