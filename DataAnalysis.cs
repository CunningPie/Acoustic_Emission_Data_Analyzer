using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEDataAnalyzer
{
    class DataAnalysis
    {
        public List<SensorInfo> Data { get; }
        public Dictionary<string, int> Columns { get; }

        public DataAnalysis(string FileName)
        {
            DataReader dr = new DataReader();
            Data = dr.ReadFile(FileName).ToList();
            Columns = (from KeyValuePair<string, KeyValuePair<int, int>> pair in dr.Columns select new KeyValuePair<string, int>(pair.Key, pair.Value.Value)).ToDictionary(t => t.Key, t=> t.Value);
        }

        public List<SensorInfo> GetSensorDataByChannel(int Channel)
        {
            return (from SensorInfo Sensor in Data where Sensor.Channel == Channel select Sensor).ToList();
        }

      
    }
}
