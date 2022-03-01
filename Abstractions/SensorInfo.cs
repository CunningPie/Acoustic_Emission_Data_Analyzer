using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEDataAnalyzer
{
    class SensorInfo
    {
        public string SensorType;
        public int Channel;
        public TimeSpan Time;
        public double MSec;
        public double Amplitude;
        public double Energy;
        public double Duration;
        public int Counts;

        public SensorInfo()
        {
        }

        public SensorInfo(SensorInfo sensorInfo)
        {
            if (sensorInfo != null)
            {
                SensorType = sensorInfo.SensorType;
                Channel = sensorInfo.Channel;
                Time = sensorInfo.Time;
                MSec = sensorInfo.MSec;
                Amplitude = sensorInfo.Amplitude;
                Energy = sensorInfo.Energy;
                Duration = sensorInfo.Duration;
                Counts = sensorInfo.Counts;
            }
        }

        public SensorInfo(string NewSensorType, int NewChannel, TimeSpan NewTime, double NewMSec = 0, double NewAmplitude = 0, double NewEnergy = 0, double NewDuration = 0, int NewCounts = 0)
        {
            SensorType = NewSensorType;
            Channel = NewChannel;
            Time = NewTime;
            MSec = NewMSec;
            Amplitude = NewAmplitude;
            Energy = NewEnergy;
            Duration = NewDuration;
            Counts = NewCounts;
        }

        public string[] Params => new string[] { SensorType, Channel.ToString(), Time.ToString(), MSec.ToString(), Amplitude.ToString(), Energy.ToString(), Duration.ToString(), Counts.ToString()};

    }
}
