using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEDataAnalyzer.Correlation
{
    class ClusteringAnalysis
    {
        KeyValuePair<List<SuperWave>, List<double>>[] Table;
        int ClustersCount;

        public ClusteringAnalysis(List<SuperWave> SuperWaves)
        {
            Table = new KeyValuePair<List<SuperWave>, List<double>>[SuperWaves.Count];
            ClustersCount = SuperWaves.Count;

            for (int i = 0; i < SuperWaves.Count; i++)
                foreach (SuperWave sw in SuperWaves)
                {
                    Table[i].Key.Add(SuperWaves[i]);
                    Table[i].Value.Add(EuclideanDistance(SuperWaves[i].Wave, sw.Wave));
                }
        }

        double EuclideanDistance(Wave W1, Wave W2)
        {
            double Sum = 0; 

            if (W1.Events.Count != W2.Events.Count)
                return double.NegativeInfinity;

            for (int i = 0; i < W1.Events.Count; i++)
                Sum += Math.Pow(W1.Events[i].Amplitude - W2.Events[i].Amplitude, 2);

            return Math.Sqrt(Sum);
        }

        public void CreateClusters(int Count)
        {
            while (ClustersCount > Count)
            {
                double MinDistance = double.PositiveInfinity;

                int rawNum = 0, columnNum = 0;

                for (int i = 0; i < Table.Length; i++)
                    for (int j = 0; j < Table[i].Value.Count; j++)
                        if (MinDistance > Table[i].Value[j] && Table[i].Value[j] > 0)
                        {
                            MinDistance = Table[i].Value[j];
                            rawNum = i;
                            columnNum = j;
                        }

                Table[rawNum].Key.AddRange(Table[columnNum].Key);

                for (int i = 0; i < Table[rawNum].Value.Count; i++)
                {
                    if (Table[rawNum].Value[i] > Table[columnNum].Value[i])
                        Table[rawNum].Value[i] = Table[columnNum].Value[i];
                }

                Table[columnNum].Key.Clear();
                Table[columnNum].Value.Clear();

                ClustersCount--;
            }
        }

        public List<List<SuperWave>> SuperWaveClusterization()
        {
            return (from KeyValuePair<List<SuperWave>, List<double>> pair in Table where pair.Key.Count > 0 select pair.Key).ToList();
        }


        // Кусок кода, который надо вставить в корреляцию 
        /*
         * 
            ClusteringAnalysis Analyser = new ClusteringAnalysis(AllSuperWaves.Keys.ToList());
            Analyser.CreateClusters(2);

            List<Wave>[] ClusterWaves = new List<Wave>[2];

            var SuperWavesLists = Analyser.SuperWaveClusterization();

            for (int i = 0; i < 2; i++)
                foreach (SuperWave sw in SuperWavesLists[i])
                {
                        ClusterWaves[i].AddRange(AllSuperWaves[sw]);
                }

            */
        
    }
}
