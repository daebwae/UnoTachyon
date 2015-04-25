using System;
using System.Globalization;
using System.IO.Ports;

namespace HLLFeeder
{
    class ArduinoCom: IDisposable
    {
        private readonly string _comPort;
        private readonly int _baudRate;
        private SerialPort _serialPort;
        private double _currentEstimate; 

        public ArduinoCom(string comPort, int baudRate)
        {
            _comPort = comPort;
            _baudRate = baudRate;
        }


        public void AddUniqueItem(string item)
        {
            var comPort = GetPort(); 
            comPort.Write(item);
            comPort.Write("\n");
            string received = comPort.ReadTo("\n");

            _currentEstimate = double.Parse(received, CultureInfo.InvariantCulture);
        }

        public double GetEstimate()
        {
            return _currentEstimate; 
        }

        public void Dispose()
        {
            if(_serialPort == null)
                return;
            
            _serialPort.Close();
            _serialPort = null; 
        }

        private SerialPort GetPort()
        {
            if (_serialPort == null)
            {
                _serialPort = new SerialPort(_comPort, _baudRate);
                _serialPort.Open();
            }

            return _serialPort; 
        }
    }
}
