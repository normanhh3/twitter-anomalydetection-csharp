﻿//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

namespace com.github.ruananswer.statistics
{

    /// <summary>
    /// Created by on 17-2-28.
    /// </summary>
    public class QuickMedians
    {
        private double[] values;
        private double _median = 0;
        private int _n = 0;

        public QuickMedians(double[] initialValues)
        {
            _n = initialValues.Length;
            //values = Arrays.copyOf(initialValues, _n);
            System.Array.Copy(initialValues, values, _n);
            if (_n > 0)
            {
                if (_n % 2 == 0)
                {
                    _median = (quickSelectK(_n / 2 - 1) + quickSelectK(_n / 2)) / 2.0;
                }
                else
                {
                    _median = quickSelectK(_n / 2);
                }
            }
            else
            {
                _median = double.NaN;
            }
        }

        public virtual int partition(int low, int high)
        {
            int l = low, r = high, i = low - 1;
            double x = values[high];
            for (int j = l; j < r; ++j)
            {
                if (values[j] <= x)
                {
                    ++i;
                    swap(i, j);
                }
            }
            swap(i + 1, r);
            return i + 1;
        }

        private void swap(int i, int j)
        {
            double tmp = values[i];
            values[i] = values[j];
            values[j] = tmp;
        }

        public virtual double quickSelectK(int k)
        {
            int l = 0, r = _n - 1, idx = 0, len = 0;
            while (l < r)
            {
                idx = partition(l, r);
                len = idx - l + 1;
                if (len == k + 1)
                {
                    return values[idx];
                }
                else if (len < k + 1)
                {
                    k = k - len;
                    l = idx + 1;
                }
                else
                {
                    r = idx - 1;
                }
            }
            return values[l];
        }

        public virtual double Median
        {
            get
            {
                return +_median;
            }
        }
    }

}