using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEDataAnalyzer
{
    class Wave
    {
        public List<SensorInfo> Events;
        public int Number;

        public Wave(IEnumerable<SensorInfo> NewWave, int  NewNumber)
        {
            Events = NewWave.ToList();
            Number = NewNumber;
        }

        public Wave(Wave NewWave)
        {
            Number = NewWave.Number;
            Events = new List<SensorInfo>(NewWave.Events);
        }
    }
}
