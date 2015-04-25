using System;
using System.Collections.Generic;

namespace HLLFeeder
{
    class MedianError
    {
        private bool _isSorted = true; 
        private readonly List<double> _errors = new List<double>();

        public void Add(double actual, double estimate)
        {
            var error = Math.Abs(actual - estimate) / actual; 
            _errors.Add(error);
            _isSorted = false; 
        }

        public double GetMedian()
        {
            if (!_isSorted)
            {
                _errors.Sort();
            }

            return _errors[_errors.Count/2];
        }

    }
}
