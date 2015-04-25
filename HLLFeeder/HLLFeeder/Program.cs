using System;
using HLLFeeder.DataSource;

namespace HLLFeeder
{
    class Program
    {
        static void Main()
        {
            using (var arduino = new ArduinoCom("COM4", 115200))
            { 
                var guidSource = new GuidDataSource();
                var medianError = new MedianError(); 

                foreach (var guid in guidSource)
                {
                    arduino.AddUniqueItem(guid);

                    if (guidSource.UniqueItemsCount < 500) 
                        continue;

                    var estimate = arduino.GetEstimate();
                    var actual = guidSource.UniqueItemsCount;
                    medianError.Add(actual,estimate);

                    PrintStatus(estimate, actual, medianError.GetMedian());
                }

            }
            Console.ReadKey();
        }

        private static void PrintStatus(double estimate, double actual, double medianError)
        {
            Console.WriteLine(@"HLL: {0}", estimate);
            Console.WriteLine(@"Actual: {0}", actual);
            Console.WriteLine(@"Average Relative Error: {0}", medianError);
            Console.WriteLine();
        }
    }
}
