using System;
using System.Collections;
using System.Collections.Generic;

namespace HLLFeeder.DataSource
{
    class GuidDataSource: IDataSource
    {
        private double _items;
        private double _uniqueItems;

        public IEnumerator<string> GetEnumerator()
        {
            while (true)
            {
                _items++;

                // this is a guess 
                _uniqueItems++;

                yield return Guid.NewGuid().ToString();   
            } 
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public double ItemsCount
        {
            get { return _items;  }
        }

        public double UniqueItemsCount
        {
            get { return _uniqueItems; }
        }
    }
}
