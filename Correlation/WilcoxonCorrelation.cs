using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEDataAnalyzer.Correlation
{
    class WilcoxonCorrelation
    {
        static public double WilcoxonCriteria(List<double> ValuesX, List<double> ValuesY)
        {
            List<double> Res = new List<double>();
            Dictionary<double, double> Ranks = new Dictionary<double, double>();

            for (int i = 0; i < Math.Min(ValuesX.Count, ValuesY.Count); i++)
                Res.Add(ValuesX[i] - ValuesY[i]);

            Res.RemoveAll(x => x == 0);

            if (Res.Count == 0)
                return 1;

            Res.Sort();

            double curRank = 1;

            foreach (double number in Res)
            {
                double absNumber = Math.Abs(number);
                if (!Ranks.ContainsKey(absNumber))
                {
                    int numberCount = Res.FindAll(x => Math.Abs(x) == absNumber).Count;

                    if (numberCount == 2)
                        Ranks.Add(absNumber, curRank++ + 0.5);
                    else
                        Ranks.Add(absNumber, curRank);

                    curRank++;
                }
            }

            List<double> neg = Res.FindAll(x => x < 0), pos = Res.FindAll(x => x > 0);

            double sumRank = 0;

            if (neg.Count > pos.Count)
                foreach (double number in pos)
                    sumRank += Ranks[number];
            else
                foreach (double number in neg)
                    sumRank += Ranks[Math.Abs(number)];

            Dictionary<int, int> WilcoxonTable = new Dictionary<int, int> { { 5, 0 }, { 6, 2 }, { 7, 3 }, { 8, 5 }, { 9, 8 }, { 10, 10 } };

            if (!WilcoxonTable.ContainsKey(ValuesX.Count))
                return 0;

            int p = WilcoxonTable[ValuesX.Count];

            if (sumRank < p)
                return 0;
            else return 1;
        }

        static public double Criteria(Wave WaveA, Wave WaveB, string param)
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

            return WilcoxonCriteria(ValuesX, ValuesY);
        }
    }
}
