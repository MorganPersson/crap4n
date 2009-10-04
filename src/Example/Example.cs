namespace Example
{
	public class Example : IFoo, IBar
	{
		public int NoCoverage(int a, int b, int c)
		{
			if (a > b)
			{
				if (a > c)
					return a;
				return c;
			}
			if (b > c)
				return b;
			return c;
		}

		public int SemiCoverage(int a, int b, int c)
		{
			if (a > b)
			{
				if (a > c)
					return a;
				return c;
			}
			if (b > c)
				return b;
			return c;
		}

		public int CompleteCoverage(int a, int b, int c)
		{
			if (a > b)
			{
				if (a > c)
					return a;
				return c;
			}
			if (b > c)
				return b;
			return c;
		}
		
		int IFoo.FooProp 
		{ 
			get; 
			set; 
		}
		
		int IFoo.FooMethod(int a)
		{
			return a + 1;
		}
		
		private int _barProp = 0;
		public int BarProp
		{
			get { return _barProp; }
			set	{ _barProp = value;	}
		}
		
		public int BarMethod(int a)
		{
			return a-1;
		}
	}
}