namespace Crap4n
{
    public class Percent
    {
        public double Value { get; set; }

        public static implicit operator double(Percent percent)
        {
            if (percent == null)
                return double.NaN;
            return percent.Value;
        }

        public override string ToString()
        {
            return string.Format("{0:0.0}", Value);
        }
    }
}