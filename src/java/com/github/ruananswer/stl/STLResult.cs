//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

namespace com.github.ruananswer.stl
{
	/// <summary>
	/// The STL decomposition of a time series.
	/// <para>
	/// getData() == getTrend() + getSeasonal() + getRemainder()
	/// </para>
	/// </summary>
	public class STLResult
	{
		private readonly double[] trend;
		private readonly double[] seasonal;
		private readonly double[] remainder;

		public STLResult(double[] trend, double[] seasonal, double[] remainder)
		{
			this.trend = trend;
			this.seasonal = seasonal;
			this.remainder = remainder;
		}

		public virtual double[] Trend
		{
			get
			{
				return trend;
			}
		}

		public virtual double[] Seasonal
		{
			get
			{
				return seasonal;
			}
		}

		public virtual double[] Remainder
		{
			get
			{
				return remainder;
			}
		}
	}
}