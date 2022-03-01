using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEDataAnalyzer.Data_File
{
    class DataWriter
    {
        public void WriteNewSuperSignalsFile(string dstFileName, string srcFileName, List<SuperWave> SuperWaves)
        {
            using (StreamWriter writer = new StreamWriter(dstFileName, append: true))
            {
                writer.WriteLine("Id  HHMMSS     MSEC CHAN    A       TRAI   E(TE)       R        D  THR    RMS  CNTS  ALIN");
                writer.WriteLine(srcFileName);

                foreach (SuperWave sw in SuperWaves)
                {
                    foreach (SensorInfo si in sw.Wave.Events)
                    {
                        writer.WriteLine(si.SensorType.ToString() + si.Time.ToString() + " " + Math.Round(si.MSec, 4) + " " + si.Channel + " " + Math.Round(si.Amplitude, 1) + " " + Math.Round(si.Energy, 0) + " " + 0 + " " + Math.Round(si.Duration, 1) + " " + 40.0 + " " + si.Counts + " " + 0);
                    }
                }
            }
        }
    }
}
