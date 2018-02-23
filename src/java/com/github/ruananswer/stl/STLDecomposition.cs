//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System;
using System.Collections.Generic;

namespace com.github.ruananswer.stl
{

	/// <summary>
	/// Implementation of STL: A Seasonal-Trend Decomposition Procedure based on Loess.
	/// <para>
	/// Robert B. Cleveland et al., "STL: A Seasonal-Trend Decomposition Procedure based on Loess,"
	/// in Journal of Official Statistics Vol. 6 No. 1, 1990, pp. 3-73
	/// </para>
	/// <para>
	/// Hafen, R. P. "Local regression models: Advancements, applications, and new methods." (2010).
	/// </para>
	/// Created by on 16-4-14.
	/// </summary>
	public class STLDecomposition
	{
		private readonly Config config;

		public STLDecomposition(Config config)
		{
			config.check();
			this.config = config;
		}

		public class Config
		{
			/// <summary>
			/// The number of observations in each cycle of the seasonal component, n_p </summary>
			internal int numObsPerPeriod = -1;
			/// <summary>
			/// s.window either the character string \code{"periodic"} or the span (in lags) of the loess window for seasonal extraction,
			/// which should be odd.  This has no default. 
			/// </summary>
			internal int sWindow = -1;
			/// <summary>
			/// s.degree degree of locally-fitted polynomial in seasonal extraction.  Should be 0, 1, or 2. </summary>
			internal int sDegree = 1;
			/// <summary>
			/// t.window the span (in lags) of the loess window for trend extraction, which should be odd.
			///  If \code{NULL}, the default, \code{nextodd(ceiling((1.5*period) / (1-(1.5/s.window))))}, is taken.
			/// </summary>
			internal int tWindow = -1;
			/// <summary>
			/// t.degree degree of locally-fitted polynomial in trend extraction.  Should be 0, 1, or 2. </summary>
			internal int tDegree = 1;
			/// <summary>
			/// l.window the span (in lags) of the loess window of the low-pass filter used for each subseries.
			/// Defaults to the smallest odd integer greater than or equal to \code{n.p}
			/// which is recommended since it prevents competition between the trend and seasonal components.
			/// If not an odd integer its given value is increased to the next odd one.
			/// </summary>
			internal int lWindow = -1;
			/// <summary>
			/// l.degree degree of locally-fitted polynomial for the subseries low-pass filter.  Should be 0, 1, or 2. </summary>
			internal int lDegree = 1;
			/// <summary>
			/// s.jump s.jump,t.jump,l.jump,fc.jump integers at least one to increase speed of the respective smoother.
			/// Linear interpolation happens between every \code{*.jump}th value. 
			/// </summary>
			internal int sJump = -1;
			/// <summary>
			/// t.jump </summary>
			internal int tJump = -1;
			/// <summary>
			/// l.jump </summary>
			internal int lJump = -1;
			/// <summary>
			/// critfreq the critical frequency to use for automatic calculation of smoothing windows for the trend and high-pass filter. </summary>
			internal double critFreq = 0.05;
			/// <summary>
			/// The number of passes through the inner loop, n_i </summary>
			internal int numberOfInnerLoopPasses = 2;
			/// <summary>
			/// The number of robustness iterations of the outer loop, n_o </summary>
			internal int numberOfRobustnessIterations = 1;
			/// <summary>
			/// sub.labels optional vector of length n.p that contains the labels of the subseries in their natural order (such as month name, day of week, etc.),
			/// used for strip labels when plotting.  All entries must be unique. 
			/// </summary>
			internal int[] subLabels = null;
			/// <summary>
			/// the number of series to decompose </summary>
			internal int numberOfDataPoints = -1;

			public virtual int NumObsPerPeriod
			{
				get
				{
					return numObsPerPeriod;
				}
				set
				{
					this.numObsPerPeriod = value;
				}
			}


			public virtual int getsWindow()
			{
				return sWindow;
			}

			public virtual void setsWindow(int sWindow)
			{
				this.sWindow = sWindow;
			}

			public virtual int getsDegree()
			{
				return sDegree;
			}

			public virtual void setsDegree(int sDegree)
			{
				this.sDegree = sDegree;
			}

			public virtual int gettWindow()
			{
				return tWindow;
			}

			public virtual void settWindow(int tWindow)
			{
				this.tWindow = tWindow;
			}

			public virtual int gettDegree()
			{
				return tDegree;
			}

			public virtual void settDegree(int tDegree)
			{
				this.tDegree = tDegree;
			}

			public virtual int getlWindow()
			{
				return lWindow;
			}

			public virtual void setlWindow(int lWindow)
			{
				this.lWindow = lWindow;
			}

			public virtual int getlDegree()
			{
				return lDegree;
			}

			public virtual void setlDegree(int lDegree)
			{
				this.lDegree = lDegree;
			}

			public virtual int getsJump()
			{
				return sJump;
			}

			public virtual void setsJump(int sJump)
			{
				this.sJump = sJump;
			}

			public virtual int gettJump()
			{
				return tJump;
			}

			public virtual void settJump(int tJump)
			{
				this.tJump = tJump;
			}

			public virtual int getlJump()
			{
				return lJump;
			}

			public virtual void setlJump(int lJump)
			{
				this.lJump = lJump;
			}

			public virtual double CritFreq
			{
				get
				{
					return critFreq;
				}
				set
				{
					this.critFreq = value;
				}
			}


			public virtual int NumberOfInnerLoopPasses
			{
				get
				{
					return numberOfInnerLoopPasses;
				}
				set
				{
					this.numberOfInnerLoopPasses = value;
				}
			}


			public virtual int NumberOfRobustnessIterations
			{
				get
				{
					return numberOfRobustnessIterations;
				}
				set
				{
					this.numberOfRobustnessIterations = value;
				}
			}


			public virtual int[] SubLabels
			{
				get
				{
					return subLabels;
				}
				set
				{
					this.subLabels = value;
				}
			}


			public virtual int NumberOfDataPoints
			{
				get
				{
					return numberOfDataPoints;
				}
				set
				{
					this.numberOfDataPoints = value;
				}
			}


			public Config()
			{
			}

			public virtual void check()
			{
				checkPeriodicity(numObsPerPeriod, numberOfDataPoints);
			}

			internal virtual bool checkPeriodicity(int numObsPerPeriod, int numberOfDataPoints)
			{
				if (numObsPerPeriod == -1)
				{
					throw new System.ArgumentException("Must specify periodicity of seasonal");
				}
				if (numObsPerPeriod < 4)
				{
					throw new System.ArgumentException("Periodicity (numObsPerPeriod) must be >= 4");
				}
				if (numberOfDataPoints <= 2 * numObsPerPeriod)
				{
					throw new System.ArgumentException("numberOfDataPoints(total length) must contain at least 2 * Periodicity (numObsPerPeriod) points");
				}
				return true;
			}
		}

		/// <summary>
		/// Decompose a time series into seasonal, trend and irregular components using \code{loess}, acronym STL.
		/// A new implementation of STL.  Allows for NA values, local quadratic smoothing,  post-trend smoothing, and endpoint blending.
		/// The usage is very similar to that of R's built-in \code{stl()}.
		/// 
		/// </summary>
		public virtual STLResult decompose(long[] times, double[] series)
		{
			if (times == null || series == null || times.Length != series.Length)
			{
				throw new System.ArgumentException("times must be same length as time series");
			}
			int n = series.Length;
			int numObsPerPeriod = config.NumObsPerPeriod;
			double[] trend = new double[n];
			double[] seasonal = new double[n];
			double[] remainder = new double[n];

			for (int i = 0; i < n; i++)
			{
				trend[i] = 0.0;
				seasonal[i] = 0.0;
				remainder[i] = 0.0;
			}

			if (config.getlWindow() == -1)
			{
				config.setlWindow(STLUtility.nextOdd(config.NumObsPerPeriod));
			}
			else
			{
				config.setlWindow(STLUtility.nextOdd(config.getlWindow()));
			}


			if (config.SubLabels == null)
			{
				int[] idx = new int[n];
				for (int i = 0; i < n; ++i)
				{
					idx[i] = i % numObsPerPeriod + 1;
				}
				config.SubLabels = idx;
			}

			config.setsWindow(10 * n + 1);
			config.setsDegree(0);
			config.setsJump((int)Math.Ceiling(config.getsWindow() / 10.0));

			if (config.gettWindow() == -1)
			{
				/// <summary>
				/// Or use  t.window <- nextodd(ceiling(1.5 * n.p/(1 - 1.5 / s.window))) </summary>
				config.settWindow(STLUtility.getTWindow(config.gettDegree(), config.getsDegree(), config.getsWindow(), numObsPerPeriod, config.CritFreq));
			}

			if (config.getsJump() == -1)
			{
				config.setsJump((int)Math.Ceiling((double)config.getsWindow() / 10.0));
			}
			if (config.gettJump() == -1)
			{
				config.settJump((int)Math.Ceiling((double)config.gettWindow() / 10.0));
			}
			if (config.getlJump() == -1)
			{
				config.setlJump((int)Math.Ceiling((double)config.getlWindow() / 10.0));
			}

			/// <summary>
			/// start and end indices for after adding in extra n.p before and after </summary>
			int startIdx = numObsPerPeriod, endIdx = n - 1 + numObsPerPeriod;

			/// <summary>
			/// cycleSubIndices will keep track of what part of the
			/// # seasonal each observation belongs to 
			/// </summary>
			int[] cycleSubIndices = new int[n];
			double[] weight = new double[n];
			for (int i = 0; i < n; ++i)
			{
				cycleSubIndices[i] = i % numObsPerPeriod + 1;
				weight[i] = 1.0;
			}
			// subLabels !!
			int lenC = n + 2 * numObsPerPeriod;
			double[] C = new double[lenC];
			double[] D = new double[n];
			double[] detrend = new double[n];
			int tempSize = (int)Math.Ceiling((double)n / (double)numObsPerPeriod) / 2;
			List<double> cycleSub = new List<double>(tempSize), subWeights = new List<double>(tempSize);
			int[] cs1 = new int[numObsPerPeriod], cs2 = new int[numObsPerPeriod];
			for (int i = 0; i < numObsPerPeriod; ++i)
			{
				cs1[i] = cycleSubIndices[i];
				cs2[i] = cycleSubIndices[n - numObsPerPeriod + i];
			}

			double[] ma3, L = new double[n];
			int ljump = config.getlJump(), tjump = config.gettJump();
			int lenLev = (int)Math.Ceiling((double)n / (double)ljump), lenTev = (int)Math.Ceiling((double)n / (double)tjump);
			int[] lEv = new int[lenLev], tEv = new int[lenTev];
			double weightMeanAns = 0.0;

			for (int oIter = 1; oIter <= config.NumberOfRobustnessIterations; ++oIter)
			{
				for (int iIter = 1; iIter <= config.NumberOfInnerLoopPasses; ++iIter)
				{
					/// <summary>
					/// Step 1: detrending </summary>
					for (int i = 0; i < n; ++i)
					{
						detrend[i] = series[i] - trend[i];
					}

					/// <summary>
					/// Step 2: smoothing of cycle-subseries </summary>
					for (int i = 0; i < numObsPerPeriod; ++i)
					{
						cycleSub.Clear();
						subWeights.Clear();
						for (int j = i; j < n; j += numObsPerPeriod)
						{
							if (cycleSubIndices[j] == i + 1)
							{
								cycleSub.Add(detrend[j]);
								subWeights.Add(weight[j]);
							}
						}
						/// <summary>
						/// C[c(cs1, cycleSubIndices, cs2) == i] <- rep(weighted.mean(cycleSub,
						/// w = w[cycleSubIndices == i], na.rm = TRUE), cycleSub.length + 2)
						/// </summary>
						weightMeanAns = weightMean(cycleSub, subWeights);
						for (int j = i; j < numObsPerPeriod; j += numObsPerPeriod)
						{
							if (cs1[j] == i + 1)
							{
								C[j] = weightMeanAns;
							}
						}
						for (int j = i; j < n; j += numObsPerPeriod)
						{
							if (cycleSubIndices[j] == i + 1)
							{
								C[j + numObsPerPeriod] = weightMeanAns;
							}
						}
						for (int j = 0; j < numObsPerPeriod; ++j)
						{
							if (cs2[j] == i + 1)
							{
								C[j + numObsPerPeriod + n] = weightMeanAns;
							}
						}
					}

					/// <summary>
					/// Step 3: Low-pass filtering of collection of all the cycle-subseries
					/// # moving averages
					/// </summary>
					ma3 = STLUtility.cMa(C, numObsPerPeriod);

					for (int i = 0, j = 0; i < lenLev; ++i, j += ljump)
					{
						lEv[i] = j + 1;
					}
					if (lEv[lenLev - 1] != n)
					{
						int[] tempLev = new int[lenLev + 1];
						Array.Copy(lEv, 0, tempLev, 0, lenLev);
						tempLev[lenLev] = n;
						L = STLUtility.loessSTL(null, ma3, config.getlWindow(), config.getlDegree(), tempLev, weight, config.getlJump());
					}
					else
					{
						L = STLUtility.loessSTL(null, ma3, config.getlWindow(), config.getlDegree(), lEv, weight, config.getlJump());
					}

					/// <summary>
					/// Step 4: Detrend smoothed cycle-subseries </summary>
					/// <summary>
					/// Step 5: Deseasonalize </summary>
					for (int i = 0; i < n; ++i)
					{
						seasonal[i] = C[startIdx + i] - L[i];
						D[i] = series[i] - seasonal[i];
					}

					/// <summary>
					/// Step 6: Trend Smoothing </summary>
					for (int i = 0, j = 0; i < lenTev; ++i, j += tjump)
					{
						tEv[i] = j + 1;
					}
					if (tEv[lenTev - 1] != n)
					{
						int[] tempTev = new int[lenTev + 1];
						Array.Copy(tEv, 0, tempTev, 0, lenTev);
						tempTev[lenTev] = n;
						trend = STLUtility.loessSTL(null, D, config.gettWindow(), config.gettDegree(), tempTev, weight, config.gettJump());
					}
					else
					{
						trend = STLUtility.loessSTL(null, D, config.gettWindow(), config.gettDegree(), tEv, weight, config.gettJump());
					}
				}

			}
			// Calculate remainder
			for (int i = 0; i < n; i++)
			{
				remainder[i] = series[i] - trend[i] - seasonal[i];
			}
			return new STLResult(trend, seasonal, remainder);
		}

		private double weightMean(List<double> x, List<double> w)
		{
			double sum = 0.0, sumW = 0.0;
			int len = x.Count;
			for (int i = 0; i < len; ++i)
			{
				if (!double.IsNaN(x[i]))
				{
					sum += (x[i] * w[i]);
					sumW += w[i];
				}
			}
			return sum / sumW;
		}
	}

}