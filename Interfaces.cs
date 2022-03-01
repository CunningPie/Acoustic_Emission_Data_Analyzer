using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEDataAnalyzer
{
    interface IDataReader
    {
        IEnumerable<SensorInfo> ReadFile(string FileName);
    }
}
