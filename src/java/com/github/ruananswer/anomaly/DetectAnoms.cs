//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System;
using System.Collections.Generic;

namespace com.github.ruananswer.anomaly
{

    using OnlineNormalStatistics = com.github.ruananswer.statistics.OnlineNormalStatistics;
    using QuickMedians = com.github.ruananswer.statistics.QuickMedians;
    using STLDecomposition = com.github.ruananswer.stl.STLDecomposition;
    using STLResult = com.github.ruananswer.stl.STLResult;
    //using TDistribution = org.apache.commons.math3.distribution.TDistribution;
    using TDistribution = Math3.distribution.TDistribution;


    /// <summary>
    /// Implementation of the underlying algorithm – referred to as Seasonal Hybrid ESD (S-H-ESD) builds upon the Generalized ESD test for detecting anomalies.
    /// Created by on 16-4-6.
    /// </summary>
    public class DetectAnoms
    {
        private readonly Config config;

        public DetectAnoms(Config config)
        {
            this.config = config;
        }

        /// <summary>
        /// The main parameter of function anomalyDetection
        /// </summary>
        public class Config
        {
            /// <summary>
            /// Maximum number of anomalies that S-H-ESD will detect as a percentage of the data. </summary>
            internal double maxAnoms = 0.49;
            /// <summary>
            /// Defines the number of observations in a single period, and used during seasonal decomposition. </summary>
            internal int numObsPerPeriod = 1440;
            /// <summary>
            /// noms_threshold  use the threshold to filter the anoms. such as if anoms_threshold = 1.05,
            /// #' then we will filter the anoms that exceed the exceptional critical value 100%-105% 
            /// </summary>
            internal double anomsThreshold = 1.05;
            /// <summary>
            /// The level of statistical significance with which to accept or reject anomalies. </summary>
            internal double alpha = 0.05;

            public Config()
            {
            }

            public virtual double MaxAnoms
            {
                get
                {
                    return maxAnoms;
                }
                set
                {
                    this.maxAnoms = value;
                }
            }

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

            public virtual double AnomsThreshold
            {
                get
                {
                    return anomsThreshold;
                }
                set
                {
                    this.anomsThreshold = value;
                }
            }

            public virtual double Alpha
            {
                get
                {
                    return alpha;
                }
                set
                {
                    this.alpha = value;
                }
            }




        }

        /// <summary>
        /// The detected anomalies in a time series using S-H-ESD
        /// A list containing the anomalies (anoms) and decomposition components (stl).
        /// </summary>
        public class ANOMSResult
        {
            private readonly DetectAnoms outerInstance;

            internal readonly long[] anomsIndex;
            internal readonly double[] anomsScore;
            internal readonly double[] dataDecomp;

            internal ANOMSResult(DetectAnoms outerInstance, long[] anomsIdx, double[] anomsSc, double[] dataDe)
            {
                this.outerInstance = outerInstance;
                this.anomsIndex = anomsIdx;
                this.anomsScore = anomsSc;
                this.dataDecomp = dataDe;
            }

            public virtual long[] AnomsIndex
            {
                get
                {
                    return anomsIndex;
                }
            }

            public virtual double[] AnomsScore
            {
                get
                {
                    return anomsScore;
                }
            }

            public virtual double[] DataDecomp
            {
                get
                {
                    return dataDecomp;
                }
            }
        }

        /// <summary>
        /// Args:
        /// #'	series: Time series to perform anomaly detection on.
        /// #'	max_Anoms: Maximum number of anomalies that S-H-ESD will detect as a percentage of the data.
        /// #'	alpha: The level of statistical significance with which to accept or reject anomalies.
        /// #'	num_obs_per_period: Defines the number of observations in a single period, and used during seasonal decomposition.
        /// #'  Returns:
        /// #'  A list containing the anomalies (anoms) and decomposition components (stl).
        /// </summary>
        private ANOMSResult detectAnoms(long[] timestamps, double[] series)
        {
            if (series == null || series.Length < 1)
            {
                throw new System.ArgumentException("must supply period length for time series decomposition");
            }
            int numberOfObservations = series.Length;
            /// <summary>
            /// use StlDec function
            /// first interpolation to solve problem data to much
            /// </summary>
            // -- Step 1: Decompose data. This returns a univariate remainder which will be used for anomaly detection. Optionally, we might NOT decompose.
            STLResult data = removeSeasonality(timestamps, series, config.NumObsPerPeriod);
            double[] data_trend = data.Trend;
            double[] data_seasonal = data.Seasonal;

    //        Mean mean = new Mean();
    //        Variance variance = new Variance();
    //        Median median = new Median();

            // Remove the seasonal component, and the median of the data to create the univariate remainder
            double[] dataForSHESD = new double[numberOfObservations];
            double[] dataDecomp = new double[numberOfObservations];

            QuickMedians quickMedian = new QuickMedians(series);
            double medianOfSeries = quickMedian.Median; //median.evaluate(series);

            // if the data of has no seasonality, directed use the raw_data into function S-H-ESD !!!
            for (int i = 0; i < numberOfObservations; ++i)
            {
                dataForSHESD[i] = series[i] - data_seasonal[i] - medianOfSeries;
                dataDecomp[i] = data_trend[i] + data_seasonal[i];
            }
            // Maximum number of outliers that S-H-ESD can detect (e.g. 49% of data)
            int maxOutliers = (int)Math.Round(numberOfObservations * config.MaxAnoms);
            if (maxOutliers == 0)
            {
                throw new System.ArgumentException("You have " + numberOfObservations + " observations in a period, which is too few. Set a higher value");
            }

            long[] anomsIdx = new long[maxOutliers];
            double[] anomsSc = new double[maxOutliers];
            int numAnoms = 0;

            OnlineNormalStatistics stat = new OnlineNormalStatistics(dataForSHESD);
            QuickMedians quickMedian1 = new QuickMedians(dataForSHESD);
            double dataMean = stat.Mean; //mean.evaluate(dataForSHESD);
            double dataMedian = quickMedian1.Median; //median.evaluate(dataForSHESD);
            // use mad replace the variance
            // double dataStd = Math.sqrt(stat.getPopulationVariance());//Math.sqrt(variance.evaluate(dataForSHESD));
            double[] tempDataForMad = new double[numberOfObservations];
            for (int i = 0; i < numberOfObservations; ++i)
            {
                tempDataForMad[i] = Math.Abs(dataForSHESD[i] - dataMedian);
            }
            QuickMedians quickMedian2 = new QuickMedians(tempDataForMad);
            double dataStd = quickMedian2.Median;

            if (Math.Abs(dataStd) <= 1e-10)
            {
                //return null;
                throw new System.ArgumentException("The variance of the series data is zero");
            }

            double[] ares = new double[numberOfObservations];
            for (int i = 0; i < numberOfObservations; ++i)
            {
                ares[i] = Math.Abs(dataForSHESD[i] - dataMedian);
                ares[i] /= dataStd;
            }

            // here use std for the following iterative calculate datastd
            dataStd = Math.Sqrt(stat.PopulationVariance);

            int[] aresOrder = getOrder(ares);
            int medianIndex = numberOfObservations / 2;
            int left = 0, right = numberOfObservations - 1;
            int currentLen = numberOfObservations, tempMaxIdx = 0;
            double R = 0.0, p = 0.0;
            for (int outlierIdx = 1; outlierIdx <= maxOutliers; ++outlierIdx)
            {
                p = 1.0 - config.Alpha / (2 * (numberOfObservations - outlierIdx + 1));
                TDistribution tDistribution = new TDistribution(numberOfObservations - outlierIdx - 1);
                double t = tDistribution.inverseCumulativeProbability(p);
                double lambdaCritical = t * (numberOfObservations - outlierIdx) / Math.Sqrt((numberOfObservations - outlierIdx - 1 + t * t) * (numberOfObservations - outlierIdx + 1));
                if (left >= right)
                {
                    break;
                }
                if (currentLen < 1)
                {
                    break;
                }

                // remove the largest
                if (Math.Abs(dataForSHESD[aresOrder[left]] - dataMedian) > Math.Abs(dataForSHESD[aresOrder[right]] - dataMedian))
                {
                    tempMaxIdx = aresOrder[left];
                    ++left;
                    ++medianIndex;
                }
                else
                {
                    tempMaxIdx = aresOrder[right];
                    --right;
                    --medianIndex;
                }
                // get the R
                R = Math.Abs((dataForSHESD[tempMaxIdx] - dataMedian) / dataStd);
                // recalculate the dataMean and dataStd
                dataStd = Math.Sqrt(((currentLen - 1) * (dataStd * dataStd + dataMean * dataMean) - dataForSHESD[tempMaxIdx] * dataForSHESD[tempMaxIdx] - ((currentLen - 1) * dataMean - dataForSHESD[tempMaxIdx]) * ((currentLen - 1) * dataMean - dataForSHESD[tempMaxIdx]) / (currentLen - 2)) / (currentLen - 2));
                dataMean = (dataMean * currentLen - dataForSHESD[tempMaxIdx]) / (currentLen - 1);
                dataMedian = dataForSHESD[aresOrder[medianIndex]];
                --currentLen;

                // record the index
                anomsIdx[outlierIdx - 1] = tempMaxIdx;
                anomsSc[outlierIdx - 1] = R;
                if (R < lambdaCritical * config.AnomsThreshold || double.IsNaN(dataStd) || Math.Abs(dataStd) <= 1e-10)
                {
                    break;
                }
                numAnoms = outlierIdx;
            }
            if (numAnoms > 0)
            {
                List<Pair> map = new List<Pair>();
                for (int i = 0; i < numAnoms; ++i)
                {
                    map.Add(new Pair(this, (int)anomsIdx[i], anomsSc[i]));
                }
                map.Sort(new PairKeyComparator(this));
                long[] idx = new long[numAnoms];
                double[] anoms = new double[numAnoms];
                for (int i = 0; i < numAnoms; ++i)
                {
                    idx[i] = map[i].key;
                    anoms[i] = map[i].value;
                }
                return new ANOMSResult(this, idx, anoms, dataDecomp);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// #' @name AnomalyDetectionTs </summary>
        /// #' <param name="timestamps"> & series Time series as a two column data frame where the first column consists of the
        /// #' timestamps and the second column consists of the observations. </param>
        /// #' <param name="max_anoms"> Maximum number of anomalies that S-H-ESD will detect as a percentage of the
        /// #' data. </param>
        /// #' <param name="numObsPerPeriod"> the numbers point in one period </param>
        /// #' <param name="anoms_threshold">  use the threshold to filter the anoms. such as if anoms_threshold = 1.05,
        /// #' then we will filter the anoms that exceed the exceptional critical value 100%-105% </param>
        /// #' <param name="alpha"> The level of statistical significance with which to accept or reject anomalies. </param>
        public virtual ANOMSResult anomalyDetection(long[] timestamps, double[] series)
        {
            // Sanity check all input parameters
            if (timestamps == null || timestamps.Length < 1 || series == null || series.Length < 1 || timestamps.Length != series.Length)
            {
                throw new System.ArgumentException("The data is empty or has no equal length.");
            }
            if (config.MaxAnoms > 0.49)
            {
                throw new System.ArgumentException("max_anoms must be less than 50% of the data points.");
            }
            else if (config.MaxAnoms <= 0)
            {
                throw new System.ArgumentException("max_anoms must be positive.");
            }
            /// <summary>
            /// Main analysis: perform S-H-ESD
            /// </summary>
            int numberOfObservations = series.Length;
            if (config.MaxAnoms < 1.0 / numberOfObservations)
            {
                config.MaxAnoms = 1.0 / numberOfObservations;
            }
            removeMissingValuesByAveragingNeighbors(series);

            return detectAnoms(timestamps, series);
        }

        private STLResult removeSeasonality(long[] timestamps, double[] series, int seasonality)
        {
            STLDecomposition.Config config = new STLDecomposition.Config();
            config.NumObsPerPeriod = seasonality;
            config.NumberOfDataPoints = timestamps.Length;
            // if robust
            config.NumberOfInnerLoopPasses = 1;
            config.NumberOfRobustnessIterations = 15;

            STLDecomposition stl = new STLDecomposition(config);
            STLResult res = stl.decompose(timestamps, series);

            return res;
        }

        public static void removeMissingValuesByAveragingNeighbors(double[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (double.IsNaN(arr[i]))
                {
                    double sum = 0.0;
                    int count = 0;
                    if (i - 1 >= 0 && !double.IsNaN(arr[i - 1]))
                    {
                        sum += arr[i - 1];
                        count++;
                    }
                    if (i + 1 < arr.Length && !double.IsNaN(arr[i + 1]))
                    {
                        sum += arr[i + 1];
                        count++;
                    }
                    if (count != 0)
                    {
                        arr[i] = sum / count;
                    }
                    else
                    {
                        arr[i] = 0.0;
                    }
                }
            }
        }

        internal class Pair
        {
            private readonly DetectAnoms outerInstance;

            internal int key;
            internal double value;
            public Pair(DetectAnoms outerInstance, int k, double v)
            {
                this.outerInstance = outerInstance;
                key = k;
                value = v;
            }
        }

        internal class PairKeyComparator : IComparer<Pair>
        {
            private readonly DetectAnoms outerInstance;

            public PairKeyComparator(DetectAnoms outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public virtual int Compare(Pair a, Pair b)
            {
                if (a.key != b.key)
                {
                    return a.key - b.key;
                }
                return (a.value - b.value) > 0.0 ? 1 : -1;
            }
        }

        internal class PairValueComparator : IComparer<Pair>
        {
            private readonly DetectAnoms outerInstance;

            public PairValueComparator(DetectAnoms outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public virtual int Compare(Pair a, Pair b)
            {
                if (a.value != b.value)
                {
                    return (a.value - b.value) > 0 ? -1 : 1;
                }
                return b.key - a.key;
            }
        }

        private int[] getOrder(double[] data)
        {
            if (data == null || data.Length < 1)
            {
                return null;
            }
            int len = data.Length;
            List<Pair> map = new List<Pair>();
            for (int i = 0; i < len; ++i)
            {
                map.Add(new Pair(this, i, data[i]));
            }
            map.Sort(new PairValueComparator(this));
            int[] returnOrder = new int[len];
            for (int i = 0; i < len; ++i)
            {
                returnOrder[i] = map[i].key;
            }
            return returnOrder;
        }
    }

}