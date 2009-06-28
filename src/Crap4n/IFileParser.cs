using System.Collections.Generic;

namespace Crap4n
{
    public interface IFileParser<T>
    {   
        bool CanParseFile(string fileName);
        IEnumerable<T> ParseFile(string fileName);        
    }
}
