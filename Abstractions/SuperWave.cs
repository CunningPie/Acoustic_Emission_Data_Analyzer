using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEDataAnalyzer
{
    class SuperWave
    {
        public Wave Wave;
        public int Powerset;
        public double CoeffSum = 1;
        public string Number;

        public SuperWave( Wave newWave )
        {
            Wave = newWave;
        }

        public SuperWave( IEnumerable<Wave> Waves, int NewNumber)
        {
            Wave = CreateSuperWave(Waves, NewNumber);
        }

        Wave CreateSuperWave( IEnumerable<Wave> Waves, int NewNumber)
        {

            Powerset = Waves.Count();

            List<SensorInfo> Points = new List<SensorInfo>();

            foreach (Wave w in Waves)
            {
                TimeSpan oldTime = TimeSpan.Zero;

                for (int i = 0; i < w.Events.Count; i++)
                {
                    if (Points.Count < w.Events.Count)
                        Points.Add(new SensorInfo(w.Events[i]));
                    else
                    {
                        Points[i].Channel = 0;
                        Points[i].SensorType = w.Events[i].SensorType;
                        Points[i].Amplitude += w.Events[i].Amplitude;
                        Points[i].Counts += w.Events[i].Counts;
                        Points[i].Duration += w.Events[i].Duration;
                        Points[i].Energy += w.Events[i].Energy;
                        Points[i].MSec += w.Events[i].MSec + (Points[i].Time.Subtract(oldTime).Milliseconds);
                        Points[i].Time = w.Events[i].Time;
                        oldTime = Points[i].Time;
                    }
                }
            }

            foreach (SensorInfo si in Points)
            {
                si.Amplitude /= Waves.Count();
                si.Counts /= Waves.Count();
                si.Duration /= Waves.Count();
                si.Energy /= Waves.Count();
                si.MSec /= Waves.Count();
                si.Time = TimeSpan.Zero;
            }

            return new Wave(Points, NewNumber);
        }
    }
}
