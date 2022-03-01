using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEDataAnalyzer.Correlation
{
    static class FechnerCorrelation
    {
        static public double Coefficient(Wave WaveA, Wave WaveB, string param)
        {
            int i = 0;
            double nA = 0, nB = 0; // nA - отклонение от среднего в одну сторону, nB - отклонение от среднего в разные стороны

            List<double> ValuesX = new List<double>(), ValuesY = new List<double>();

            switch (param)
            {
                case "Time":
                    ValuesX = (from SensorInfo si in WaveA.Events select si.MSec).ToList();
                    ValuesY = (from SensorInfo si in WaveB.Events select si.MSec).ToList();
                    break;
                case "Energy":
                    ValuesX = (from SensorInfo si in WaveA.Events select si.Energy).ToList();
                    ValuesY = (from SensorInfo si in WaveB.Events select si.Energy).ToList();
                    break;
                case "Amplitude":
                    ValuesX = (from SensorInfo si in WaveA.Events select si.Amplitude).ToList();
                    ValuesY = (from SensorInfo si in WaveB.Events select si.Amplitude).ToList();
                    break;
                case "CountsDuration":
                    ValuesX = (from SensorInfo si in WaveA.Events select si.Counts / si.Duration).ToList();
                    ValuesY = (from SensorInfo si in WaveB.Events select si.Counts / si.Duration).ToList();
                    break;
            }

            double meanValueX = SupportFunctions.MeanValue(ValuesX),
            meanValueY = SupportFunctions.MeanValue(ValuesY);

            while (i < ValuesX.Count() && i < ValuesY.Count())
            {
                if ((ValuesX.ElementAt(i) > meanValueX && ValuesY.ElementAt(i) > meanValueY) || (ValuesX.ElementAt(i) < meanValueX && ValuesY.ElementAt(i) < meanValueY))
                    nA++;
                else
                    nB++;

                i++;
            }

            return (nA - nB) / (nA + nB);
        }
        /*
        static public Dictionary<KeyValuePair<Wave, Wave>, double> CorrelationFunction(List<Wave> Waves, string ParamType = "")
        {
            Dictionary<KeyValuePair<Wave, Wave>, double> CorrelatedCoeffs = new Dictionary<KeyValuePair<Wave, Wave>, double>();
            List<double> ValuesX = new List<double>(), ValuesY = new List<double>();

            for (int i = 0; i < Waves.Count(); i++)
            {
                ValuesX = (from SensorInfo si in Waves[i].Events select si.Amplitude).ToList();

                foreach (Wave w in Waves.Skip(i))
                {
                    var pair = new KeyValuePair<Wave, Wave>(Waves[i], w);

                    ValuesY = (from SensorInfo si in w.Events select si.Amplitude).ToList();

                    CorrelatedCoeffs.Add(pair, Coefficient(ValuesX, ValuesY));
                }
            }

            return CorrelatedCoeffs;
        }*/
    }
}
