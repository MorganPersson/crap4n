namespace Crap4n
{
    public class NullTextOutput : ITextOutput
    {
        void ITextOutput.WriteLine(string text)
        {
        }

        void ITextOutput.WriteHeader()
        {
        }

        void ITextOutput.WriteSeparator()
        {
        }

        void ITextOutput.WriteRuntimeEnvironment()
        {
        }
    }
}