//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System;

namespace com.github.ruananswer.statistics
{
	/// <summary>
	/// Simple, fast, online normal statistics object using Welford's algorithm.
	/// </summary>
	public class OnlineNormalStatistics
	{

	  private int _n = 0;
	  private double _mean = 0;
	  private double _sumSqDiff = 0;

	  public OnlineNormalStatistics()
	  {
		// do nothing
	  }

	  public OnlineNormalStatistics(double[] initialValues)
	  {
		foreach (double d in initialValues)
		{
		  addValue(d);
		}
	  }

	  public virtual void addValue(double value)
	  {
		if (double.IsNaN(value))
		{
		  return;
		}
		double old_mean = _mean;
		_n++;
		_mean += (value - old_mean) / _n;
		_sumSqDiff += (value - _mean) * (value - old_mean);
	  }

	  public virtual int N
	  {
		  get
		  {
			return _n;
		  }
	  }

	  public virtual double Mean
	  {
		  get
		  {
			return (_n > 0) ? _mean : Double.NaN;
		  }
	  }

	  public virtual double SumSqDev
	  {
		  get
		  {
			return (_n > 0) ? _sumSqDiff : Double.NaN;
		  }
	  }

	  public virtual double Variance
	  {
		  get
		  {
			return (_n > 1) ? _sumSqDiff / (_n - 1) : Double.NaN;
		  }
	  }

	  public virtual double PopulationVariance
	  {
		  get
		  {
			return (_n > 0) ? _sumSqDiff / _n : Double.NaN;
		  }
	  }

	  public virtual double getStandardScore(double value)
	  {
		return (value - _mean) / Math.Sqrt(Variance);
	  }

	  public virtual void set_n(int _n)
	  {
		this._n = _n;
	  }

	  public virtual void set_mean(double _mean)
	  {
		this._mean = _mean;
	  }

	  public virtual void set_sumSqDiff(double _sumSqDiff)
	  {
		this._sumSqDiff = _sumSqDiff;
	  }

	  public virtual double SumOfSq
	  {
		  get
		  {
			return _sumSqDiff + _n * _mean * _mean;
		  }
	  }
	}

}