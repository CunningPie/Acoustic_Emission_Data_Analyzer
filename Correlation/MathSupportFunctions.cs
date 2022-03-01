using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEDataAnalyzer.Correlation
{
    static class SupportFunctions
    {
        static public double MeanValue(IEnumerable<double> Values)
        {
            if (Values.Count() > 0)
            {
                double meanValue = 0;

                foreach (double v in Values)
                    meanValue += v;

                return meanValue / Values.Count();
            }

            return 0;
        }

        static public SensorInfo DeltaTime(SensorInfo A, SensorInfo B)
        {
            SensorInfo copyA = new SensorInfo(A);

            copyA.MSec = copyA.Time.Subtract(B.Time).TotalMilliseconds - B.MSec + copyA.MSec;
            return copyA;
        }

        //WaveA - выборка с меньшим числом элементов
        static public KeyValuePair<Wave, Wave> PointSelection(Wave WaveA, Wave WaveB)
        {
            List<SensorInfo> EventsA = new List<SensorInfo>(), EventsB = new List<SensorInfo>(), ResultPoints = new List<SensorInfo>();
            int pointBNum = 0;

            // Создаем копии точек, чтобы не редактировать оригинальные волны
            // EventsA - набор точек волны с меньшей длиной
            // EventsB - набор точек волны с большей длиной, которую надо уменьшить
            foreach (SensorInfo si in WaveA.Events)
                EventsA.Add(DeltaTime(si, WaveA.Events[0]));
            foreach (SensorInfo si in WaveB.Events)
                EventsB.Add(DeltaTime(si, WaveB.Events[0]));

            // Для каждой точки волны А
            // сложность < O(NA * NB)
            for (int i = 0; i < EventsA.Count - 1; i++)
            {
                // Находим разницу между текущей точкой и следующей
                // сложность О(NA)
                double dA = EventsA[i + 1].MSec - EventsA[i].MSec;

                // Словарь разниц между точками волны Б, где ключ - это пара точек, а значение - разница расстояний
                Dictionary<KeyValuePair<SensorInfo, SensorInfo>, double> deltaB = new Dictionary<KeyValuePair<SensorInfo, SensorInfo>, double>();

                // Каждую точку волны Б начиная с последней, внесенной в результрующую волну С, добавляем в словарь
                // сложность < O(NB)
                for (int j = pointBNum; j < EventsB.Count() - EventsA.Count() + i + 1; j++)
                //for (int j = pointBNum; j < EventsB.Count() - 1; j++)
                    deltaB.Add(new KeyValuePair<SensorInfo, SensorInfo>(EventsB[pointBNum], EventsB[j + 1]), EventsB[j + 1].MSec - EventsB[pointBNum].MSec);

                // Список разниц между выбранной точкой волны А и точками волны Б из словаря
                Dictionary<KeyValuePair<SensorInfo, SensorInfo>, double> Deltas = new Dictionary<KeyValuePair<SensorInfo, SensorInfo>, double>();

                // Заполняем список разницами d1, d2 и т.д.
                // сложность < O(NB)
                foreach (var pair in deltaB)
                   Deltas.Add(pair.Key, Math.Abs(dA - pair.Value));

                KeyValuePair<SensorInfo, SensorInfo> SensorPair = new KeyValuePair<SensorInfo, SensorInfo>();

                // Из полученного списка выделяем точки с минимальной разностью di
                // сложность < O(NB)
                if (deltaB.Count() > 0)
                    SensorPair = (from KeyValuePair<KeyValuePair<SensorInfo, SensorInfo>, double> pair in Deltas where pair.Value == Deltas.Values.Min() select pair.Key).First();
                else
                    SensorPair = new KeyValuePair<SensorInfo, SensorInfo>(EventsB[pointBNum], null);
                // Проверяем есть ли наши точки уже в результирующей волне и добавляем если нет
                if (!ResultPoints.Contains(WaveB.Events[EventsB.IndexOf(SensorPair.Key)]))
                    ResultPoints.Add(WaveB.Events[EventsB.IndexOf(SensorPair.Key)]);

                if (!ResultPoints.Contains(WaveB.Events[EventsB.IndexOf(SensorPair.Value)]) && SensorPair.Value != null)
                    ResultPoints.Add(WaveB.Events[EventsB.IndexOf(SensorPair.Value)]);

                pointBNum = WaveB.Events.IndexOf(ResultPoints.Last());

                if (pointBNum == WaveB.Events.Count() - 1)
                    return new KeyValuePair<Wave, Wave>(new Wave(ResultPoints, WaveB.Number), new Wave(WaveA.Events.Take(ResultPoints.Count), WaveA.Number));
            }

            return new KeyValuePair<Wave, Wave>(WaveA, new Wave(ResultPoints, WaveB.Number));
        }
    }
}
