using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text.RegularExpressions;

namespace HLLFeeder
{
    class Program
    {
        static void Main(string[] args)
        {
            var comPort = new SerialPort("COM12", 115200);

            
            comPort.Open();


            int lastOutput = 0;
            string received = "";

            var words = Texts.dalloway.Split().Select(w => Regex.Replace(w, @"\W", "")).Distinct();
            int actual = 0; 

            var errors = new List<double>();
            int i = 1000; 

            foreach (var word in words)
            {
                if(word == string.Empty)
                    continue;

                i--; 
                actual++; 

                comPort.Write(word);
                comPort.Write("\n");
                received = comPort.ReadTo("\n");

            
            
                

                if(i > 0)
                    continue;

                i++;


                var estimate = double.Parse(received, CultureInfo.InvariantCulture);

                var error = Math.Abs(actual - estimate) / actual;

                errors.Add(error);
                errors.Sort();

                Console.WriteLine("HLL: {0}", received);
                Console.WriteLine("Actual: {0}", actual);
                Console.WriteLine("Average Relative Error: {0}", errors[errors.Count / 2]);
                Console.WriteLine();
                
                
            }

            comPort.Write("#ESTIMATE#\n");
            received = comPort.ReadTo("\n");
            Console.WriteLine("HLL: {0}", received);
            Console.WriteLine("Actual: {0}", actual);
            Console.WriteLine();
            comPort.Close();
            
            Console.ReadKey();
        }
    }
}
