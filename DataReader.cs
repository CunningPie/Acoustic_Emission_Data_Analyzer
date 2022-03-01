using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEDataAnalyzer
{
    class DataReader : IDataReader
    {
        public Dictionary<string, KeyValuePair<int, int>> Columns = new Dictionary<string, KeyValuePair<int, int>>();

        public IEnumerable<SensorInfo> ReadFile(string FileName)
        {
            List<SensorInfo> Data = new List<SensorInfo>();

            using (StreamReader reader = new StreamReader(FileName))
            {
                string Params = reader.ReadLine();
                ParseParams(Params);

                while (!reader.EndOfStream)
                {
                    SensorInfo info = ParseString(reader.ReadLine());

                    if (info != null)
                        Data.Add(info);
                }
            }

            return Data;
        }

        public SensorInfo ParseString(string String)
        {
            SensorInfo sensor = null;

            String = String.Insert(2, " ");

            string type = string.Concat(String.Take(2));

            if (type == "Ht" || type == "LE" || type == "Ev")
            {
                List<string> Values = String.Split(" ".ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

                sensor = new SensorInfo();

                if (Values.Count() < Columns.Count && Columns.ContainsKey("TRAI"))
                    Values.Insert(Columns["TRAI"].Key, "0");

                for (int i = 0; i < Values.Count(); i++)
                    switch (Columns.Keys.ToArray()[i].ToLower())
                    {
                        case "id":
                            sensor.SensorType = type;
                            break;
                        case "channel":
                            sensor.Channel = Convert.ToInt32(Values[i]);
                            break;
                        case "amplitude":
                            sensor.Amplitude = Convert.ToDouble(Values[i]);
                            break;
                        case "time":
                            sensor.Time = TimeSpan.Parse(Values[i]);
                            break;
                        case "msec":
                            sensor.MSec = Convert.ToDouble(Values[i]);
                            break;
                        case "energy":
                            sensor.Energy = Convert.ToDouble(Values[i]);
                            break;
                        case "duration":
                            sensor.Duration = Convert.ToDouble(Values[i]);
                            break;
                        case "risetime":
                            break;
                        case "threshold":
                            break;
                        case "counts":
                            break;
                        case "rms":
                            break;
                        case "linear amplitude":
                            break;
                        case "trai":
                            break;

                    }
            }

            return sensor;
        }

        public void ParseParams(string String)
        {
            string[] Labels = String.Split(" ".ToArray(), StringSplitOptions.RemoveEmptyEntries);

            int i = 0;

            foreach (string label in Labels)
            {
                switch (label.ToLower())
                {
                    case "id":
                        Columns.Add("Id", new KeyValuePair<int, int>(i, 1));
                        break;
                    case "chan":
                        Columns.Add("Channel", new KeyValuePair<int, int>(i, 2));
                        break;
                    case "a":
                        Columns.Add("Amplitude", new KeyValuePair<int, int>(i, 5));
                        break;
                    case "hhmmss":
                        Columns.Add("Time", new KeyValuePair<int, int>(i, 3));
                        break;
                    case "msec":
                        Columns.Add("MSec", new KeyValuePair<int, int>(i, 4));
                        break;
                    case "e(te)":
                        Columns.Add("Energy", new KeyValuePair<int, int>(i, 6));
                        break;
                    case "r":
                        Columns.Add("RiseTime", new KeyValuePair<int, int>(i, 8));
                        break;
                    case "d":
                        Columns.Add("Duration", new KeyValuePair<int, int>(i, 7));
                        break;
                    case "thr":
                        Columns.Add("Threshold", new KeyValuePair<int, int>(i, 9));
                        break;
                    case "cnts":
                        Columns.Add("Counts", new KeyValuePair<int, int>(i, 10));
                        break;
                    case "rms":
                        Columns.Add("Rms", new KeyValuePair<int, int>(i, 11));
                        break;
                    case "alin":
                        Columns.Add("Linear Amplitude", new KeyValuePair<int, int>(i, 12));
                        break;
                    case "trai":
                        Columns.Add("TRAI", new KeyValuePair<int, int>(i, 13));
                        break;
                }
                i++;
            }
        }
    }
}
