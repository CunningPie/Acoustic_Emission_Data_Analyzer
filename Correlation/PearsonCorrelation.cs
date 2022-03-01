using AEDataAnalyzer.Correlation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEDataAnalyzer
{
    public static class PearsonThresholds
    {
        public const double Amplitude = 0.97;
        public const double Time = 0.99;
        public const double Energy = 0.99;
        public const double Default = 0.85;
        public const double SuperSignalsThresholds = 0.9;
    }

    class PearsonCriticalValuesTable
    {
        Dictionary<KeyValuePair<int, double>, double> Values;

        public PearsonCriticalValuesTable(string FileName)
        {
            Values = new Dictionary<KeyValuePair<int, double>, double>();

            using (StreamReader reader = new StreamReader(FileName))
            {
                List<double> pValues = new List<double>();

                foreach (string str in reader.ReadLine().Split(' '))
                    pValues.Add(double.Parse(str));

                while (!reader.EndOfStream)
                {
                    string[] Numbers = reader.ReadLine().Split(' ');

                    for (int i = 1; i < Numbers.Count(); i++)
                        Values.Add(new KeyValuePair<int, double>(int.Parse(Numbers[0]), pValues[i - 1]), double.Parse(Numbers[i]));
                }
            }
        }

        public double FindValue(int StatesAmount, double pValue)
        {
            int correctState = StatesAmount, min = int.MaxValue;

            if (!Values.ContainsKey(new KeyValuePair<int, double>(correctState, pValue)))
                foreach (int state in (from KeyValuePair<int, double> pair in Values.Keys select pair.Key))
                {
                    if (Math.Abs(StatesAmount - state) < min)
                    {
                        min = Math.Abs(StatesAmount - state);
                        correctState = state;
                    }
                }

            return Values[new KeyValuePair<int, double>(correctState, pValue)];
        }
    }

    static class PearsonCorrelation
    {
        // ссылки на сайты с t-критерием
        //https://tehtab.ru/Guide/GuideMathematics/TheTheoryOfProbabilityAndStatistics/PirsonVsSignificance/
        //http://medstatistic.ru/theory/pirson.html
        static public bool TCriteria(double pearsonCoeff, int StatesAmount, double pValue)
        {
            double t = pearsonCoeff * Math.Sqrt(StatesAmount - 2) / Math.Sqrt(Math.Abs(1 - Math.Pow(pearsonCoeff, 2)));
            double tCrit = pValue * Math.Sqrt(StatesAmount - 2) / Math.Sqrt(Math.Abs(1 - Math.Pow(pValue, 2)));

            if (t > tCrit)
                return true;

            return false;
        }

        static public double Coefficient(Wave WaveA, Wave WaveB, string param, PearsonCriticalValuesTable pValues)
        {
            Wave copyA = new Wave(WaveA), copyB = new Wave(WaveB);
            KeyValuePair<Wave, Wave> tPair = new KeyValuePair<Wave, Wave>(copyA, copyB);
            List<double> ValuesX = new List<double>(), ValuesY = new List<double>();

            if (WaveA.Events.Count > WaveB.Events.Count)
                tPair = SupportFunctions.PointSelection(WaveB, WaveA);
            else if (WaveB.Events.Count > WaveA.Events.Count)
                tPair = SupportFunctions.PointSelection(WaveA, WaveB);

            switch (param)
            {
                case "Time":
                    ValuesX = (from SensorInfo si in tPair.Key.Events select si.MSec).ToList();
                    ValuesY = (from SensorInfo si in tPair.Value.Events select si.MSec).ToList();
                    break;
                case "Energy":
                    ValuesX = (from SensorInfo si in tPair.Key.Events select si.Energy).ToList();
                    ValuesY = (from SensorInfo si in tPair.Value.Events select si.Energy).ToList();
                    break;
                case "Amplitude":
                    ValuesX = (from SensorInfo si in tPair.Key.Events select si.Amplitude).ToList();
                    ValuesY = (from SensorInfo si in tPair.Value.Events select si.Amplitude).ToList();
                    break;
                case "CountsDuration":
                    ValuesX = (from SensorInfo si in tPair.Key.Events select si.Counts / si.Duration).ToList();
                    ValuesY = (from SensorInfo si in tPair.Value.Events select si.Counts / si.Duration).ToList();
                    break;
            }

            int i = 0;
            double SumNumerator = 0, SumDenominatorX = 0, SumDenominatorY = 0;

            double meanValueX = SupportFunctions.MeanValue(ValuesX),
                   meanValueY = SupportFunctions.MeanValue(ValuesY);


            while (i < ValuesX.Count() && i < ValuesY.Count())
            {
                SumNumerator += (ValuesX.ElementAt(i) - meanValueX) * (ValuesY.ElementAt(i) - meanValueY);
                SumDenominatorX += Math.Pow((ValuesX.ElementAt(i) - meanValueX), 2);
                SumDenominatorY += Math.Pow((ValuesY.ElementAt(i) - meanValueY), 2);

                i++;
            }

            double rCoeff = SumNumerator / Math.Sqrt(SumDenominatorX * SumDenominatorY);
            
            if (!TCriteria(rCoeff, ValuesX.Count + ValuesY.Count, pValues.FindValue(ValuesX.Count + ValuesY.Count, 0.05)))
                rCoeff = 0;

            return rCoeff;
        }

        // Чистая корреляционная функция без наворотов
        /*
        static public Dictionary<KeyValuePair<Wave, Wave>, double> CorrelationFunction(List<Wave> Waves, string ParamType = "")
        {
            Dictionary<KeyValuePair<Wave, Wave>, double> CorrelatedCoeffs = new Dictionary<KeyValuePair<Wave, Wave>, double>();
            List<double> ValuesX = new List<double>(), ValuesY = new List<double>();

            for (int i = 0; i < Waves.Count(); i++)
            {
                switch (ParamType)
                {
                    case "Time":
                        ValuesX = (from SensorInfo si in Waves[i].Events select si.MSec).ToList();
                        break;
                    case "Energy":
                        ValuesX = (from SensorInfo si in Waves[i].Events select si.Energy).ToList();
                        break;
                    case "Amplitude":
                        ValuesX = (from SensorInfo si in Waves[i].Events select si.Amplitude).ToList();
                        break;
                    case "CountsDuration":
                        ValuesX = (from SensorInfo si in Waves[i].Events select si.Counts / si.Duration).ToList();
                        break;
                }

                foreach (Wave w in Waves.Skip(i))
                {
                    var pair = new KeyValuePair<Wave, Wave>(Waves[i], w);

                    switch (ParamType)
                    {
                        case "Time":
                            ValuesY = (from SensorInfo si in w.Events select si.MSec).ToList();
                            break;
                        case "Energy":
                            ValuesY = (from SensorInfo si in w.Events select si.Energy).ToList();
                            break;
                        case "Amplitude":
                            ValuesY = (from SensorInfo si in w.Events select si.Amplitude).ToList();
                            break;
                        case "CountsDuration":
                            ValuesY = (from SensorInfo si in w.Events select si.Counts / si.Duration).ToList();
                            break;
                    }

                    CorrelatedCoeffs.Add(pair, Coefficient(ValuesX, ValuesY));
                }
            }

            return CorrelatedCoeffs;
        }

        Dictionary<KeyValuePair<Wave, Wave>, double> TimeAmplitudeCorrelationFunction( List<Wave> Waves )
        {
            Dictionary<KeyValuePair<Wave, Wave>, double> CorrelatedCoeffs = new Dictionary<KeyValuePair<Wave, Wave>, double>();
            List<double> TimeValuesX, TimeValuesY, AmplitudeValuesX, AmplitudeValuesY;

            for (int i = 0; i < Waves.Count(); i++)
            {
                TimeValuesX = (from SensorInfo si in Waves[i].Events select si.MSec).ToList();
                AmplitudeValuesX = (from SensorInfo si in Waves[i].Events select si.Amplitude).ToList();

                foreach (Wave w in Waves.Skip(i))
                {
                    var pair = new KeyValuePair<Wave, Wave>(Waves[i], w);

                    TimeValuesY = (from SensorInfo si in w.Events select si.MSec).ToList();
                    AmplitudeValuesY = (from SensorInfo si in w.Events select si.Amplitude).ToList();

                    CorrelatedCoeffs.Add(pair, PearsonCoefficient(TimeValuesX, TimeValuesY) *
                        PearsonCoefficient(AmplitudeValuesX, AmplitudeValuesY));
                }
            }

            return CorrelatedCoeffs;
        }

        private Dictionary<KeyValuePair<Wave, Wave>, double> MultMixedCorrelation( List<Wave> Waves)
        {
            Dictionary<KeyValuePair<Wave, Wave>, double> CorrelatedCoeffs = new Dictionary<KeyValuePair<Wave, Wave>, double>();

            for (int i = 0; i < Waves.Count(); i++)
                for (int j = i; j < Waves.Count(); j++)
                {
                    double r = 1;

                    foreach (var coeffs in ListCoeffs.Values)
                    {
                        r *= coeffs[new KeyValuePair<Wave, Wave>(Waves[i], Waves[j])];
                    }

                    CorrelatedCoeffs.Add(new KeyValuePair<Wave, Wave>(Waves[i], Waves[j]), r);
                }

            return CorrelatedCoeffs;
        }
        
        private Dictionary<KeyValuePair<Wave, Wave>, double> SumMixedCorrelation( List<Wave> Waves )
        {
            Dictionary<KeyValuePair<Wave, Wave>, double> CorrelatedCoeffs = new Dictionary<KeyValuePair<Wave, Wave>, double>();

            for (int i = 0; i < Waves.Count(); i++)
                for (int j = i; j < Waves.Count(); j++)
                {
                    double r = 0;

                    foreach (var coeffs in ListCoeffs.Values)
                    {
                        r += Math.Abs(coeffs[new KeyValuePair<Wave, Wave>(Waves[i], Waves[j])]);
                    }

                    CorrelatedCoeffs.Add(new KeyValuePair<Wave, Wave>(Waves[i], Waves[j]), r);
                }

            return CorrelatedCoeffs;
        }
        */
    }
}
