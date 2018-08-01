using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UARTTest
{
    class MeasurementHeadersCollectionClass
    {
        List<MeasurementHeaderClass> headers = new List<MeasurementHeaderClass>();
        int count = 0;
        int currentFilledHeader = 0;

        public void Add(int addr)
        {
            headers.Add(new MeasurementHeaderClass(addr));
            count++;
        }


        /// <summary>
        /// Get first empty header in list and throw if none is found
        /// </summary>
        /// <returns></returns>
        public MeasurementHeaderClass GetFirstEmpty()
        {
            foreach (var item in headers)
            {
                if (item.ready)
                {
                    continue;
                }
                return item;
            }
            throw new Exception("No more empty headers");
        }


        public MeasurementHeaderClass GetNextFilled()
        {
            if (headers[currentFilledHeader].ready != true)
            {
                throw new Exception("Fetching empty header");
            }
            return headers[currentFilledHeader++];
        }
    }
}
