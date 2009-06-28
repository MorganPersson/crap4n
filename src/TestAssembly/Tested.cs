namespace TestAssembly
{
	public class Tested
	{
		public int NoCoverage(int a, int b)
		{
			if (a>b)
				return a;
			return b;
		}
		
		public int SemiCoverage(int a, int b)
		{
			if (a>b)
				return a;
			return b;
		}
		
		public int CompleteCoverage(int a, int b, int c)
		{
			if (a>b)
			{
				if (a>c)
					return a;
				else
					return c;
			}
			else
			{
				if (b>c)
					return b;
				else
					return c;
			}
		}
	}
}
