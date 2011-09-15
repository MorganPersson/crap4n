namespace Crap4n
{
    public static class PercentExtension
    {
        public static Percent Percent(this int value)
        {
            return new Percent { Value = value };
        }

        public static Percent ToPercent(this double value)
        {
            return new Percent { Value = 100 * value };
        }
    }
}