using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDotNet;

namespace AEDataAnalyzer
{
    class RFunctions
    {
        REngine engine;
        
        public RFunctions()
        {
            StartupParameter rinit = new StartupParameter();
            rinit.Quiet = true;
            rinit.RHome = "F:/R-3.4.4";
            rinit.Interactive = true;

            REngine.SetEnvironmentVariables();
            engine = REngine.GetInstance(null, true, rinit);
            engine.Initialize();

            //var execution = "source('" + "F:/Научная статья/test.R" + "')";
           // engine.Evaluate(execution);
        }

        public CharacterVector CreateCharacterVector(IEnumerable<string> vector)
        {
            return new CharacterVector(engine, vector);
        }

        public void ExecuteScriptFile(string scriptFilePath, List<string[]> TimeList, List<string[]> AmplitudeList)
        {
            CharacterVector fileNameVector = engine.CreateCharacterVector(new[] { "F:\\Научная Статья\\test.png" });
            engine.SetSymbol("fileName", fileNameVector);
            engine.Evaluate("png(fileName,  width=20, height=20, units='in', res = 200)");

            engine.SetSymbol("time" + 0, CreateCharacterVector(TimeList[0]));
            engine.SetSymbol("amplitude" + 0, CreateCharacterVector(AmplitudeList[0]));
            engine.Evaluate("plot(amplitude" + 0 + "~time" + 0 + ", type = 'b', pch = " + 0 + ",col=rgb(0, 0, 0), xlab=\"time\", ylab=\"amplitude\", xlim = c(0, 1000), ylim = c(40, 100))");

            for (int i = 1; i < 10; i++)
            {
                engine.SetSymbol("time" + i, CreateCharacterVector(TimeList[i]));
                engine.SetSymbol("amplitude" + i, CreateCharacterVector(AmplitudeList[i]));
                engine.Evaluate("lines(amplitude" + i + "~time" + i + ", type = 'b', pch = " + i + ", col=rgb(" + i + ", 0, 0, max = " + 10 + "))");
            }

            engine.Evaluate("dev.off()");
        }

        public void PlotWave(Wave Wave, string FileName)
        {
            List<string[]> TimeList = new List<string[]>();
            List<string[]> ValuesYList = new List<string[]>();

            TimeList.Add((from SensorInfo si in Wave.Events select ((int)((((si.Time.Seconds - Wave.Events[0].Time.Seconds) * 1000) - Wave.Events[0].MSec + si.MSec) * 1000)).ToString()).ToArray());
            ValuesYList.Add((from SensorInfo si in Wave.Events select (si.Amplitude).ToString().Replace(",", ".")).ToArray());

            CharacterVector fileNameVector = engine.CreateCharacterVector(new[] { FileName });
            engine.SetSymbol("fileName", fileNameVector);
            engine.Evaluate("png(fileName,  width=20, height=20, units='in', res = 200)");
            engine.SetSymbol("time" + 0, CreateCharacterVector(TimeList[0]));
            engine.SetSymbol("amplitude" + 0, CreateCharacterVector(ValuesYList[0]));
            engine.Evaluate("plot(time" + 0 + ",amplitude" + 0 + ", type = 'b',pch = " + 0 + ",cex = 1.5, cex.lab = 1.8, cex.axis = 2, lwd.axis = 2, lwd = 4, col=rgb(1, 0, 0), adj.lab = 2, xlab=\"Time, µs\", ylab=\"Amplitude, dB\", xlim = c(0, 1000), ylim = c(40, 100))");

//            engine.SetSymbol("numbers", CreateCharacterVector((from Wave w in Waves select w.Number.ToString()).ToArray()));
            engine.Evaluate("legend(\"topright\", legend = c(" + Wave.Number + "), pch = 0,title = \"Events\", cex = 2)");

            engine.Evaluate("dev.off()");
        }

        public void PlotWavesCollection(List<Wave> Waves, string FileName, string Param)
        {
            List<string[]> TimeList = new List<string[]>();
            List<string[]> ValuesYList = new List<string[]>();

            foreach (Wave w in Waves)
            {
                TimeList.Add((from SensorInfo si in w.Events select ((int)((((si.Time.Seconds - w.Events[0].Time.Seconds) * 1000) - w.Events[0].MSec + si.MSec) * 1000)).ToString()).ToArray());

                switch (Param)
                {
                    case "Amplitude":
                        ValuesYList.Add((from SensorInfo si in w.Events select (si.Amplitude).ToString().Replace(",", ".")).ToArray());
                        break;
                    case "Energy":
                        ValuesYList.Add((from SensorInfo si in w.Events select (si.Energy).ToString().Replace(",", ".")).ToArray());
                        break;
                    case "Time":
                        ValuesYList.Add((from SensorInfo si in w.Events select (si.Time).ToString().Replace(",", ".")).ToArray());
                        break;
                }
            }

            CharacterVector fileNameVector = engine.CreateCharacterVector(new[] { FileName });
            engine.SetSymbol("fileName", fileNameVector);
            engine.Evaluate("png(fileName,  width=20, height=20, units='in', res = 200)");
            engine.SetSymbol("time" + 0, CreateCharacterVector(TimeList[0]));
            engine.SetSymbol("amplitude" + 0, CreateCharacterVector(ValuesYList[0]));
            engine.Evaluate("plot(time" + 0 + ",amplitude" + 0 + ", type = 'b',pch = " + 0 + ",cex = 1.5, cex.lab = 1.8, cex.axis = 2, lwd.axis = 2, lwd = 4, col=rgb(1, 0, 0), adj.lab = 2, xlab=\"Time, µs\", ylab=\"Amplitude, dB\", xlim = c(0, 1000), ylim = c(40, 100))");

            for (int i = 1; i < Waves.Count(); i++)
            {
                engine.SetSymbol("time" + i, CreateCharacterVector(TimeList[i]));
                engine.SetSymbol("amplitude" + i, CreateCharacterVector(ValuesYList[i]));
                engine.Evaluate("lines(time" + i + ",amplitude" + i + ",pch = " + i + ",cex = 1.5, type = 'b', lwd = 3)");
            }

            engine.SetSymbol("numbers", CreateCharacterVector((from Wave w in Waves select w.Number.ToString()).ToArray()));
            engine.Evaluate("legend(\"topright\", legend = c(numbers), pch = 0:" + Waves.Count + ",title = \"Events\", cex = 2)");

            engine.Evaluate("dev.off()");
        }

        public void PlotSuperWavesCollection(List<Wave> Waves, SuperWave superWave, string FileName, string Param)
        {
            List<string[]> TimeList = new List<string[]>();
            List<string[]> ValuesYList = new List<string[]>();

            List<string> SuperWaveTimeList = new List<string>();
            List<string> SuperWaveValuesYList = new List<string>();

            SuperWaveTimeList = (from SensorInfo si in superWave.Wave.Events select ((int)((((si.Time.Seconds - superWave.Wave.Events[0].Time.Seconds) * 1000) - superWave.Wave.Events[0].MSec + si.MSec) * 1000)).ToString()).ToList();

            switch (Param)
            {
                case "Amplitude":
                    SuperWaveValuesYList = (from SensorInfo si in superWave.Wave.Events select (si.Amplitude).ToString().Replace(",", ".")).ToList();
                    break;
                case "Energy":
                    SuperWaveValuesYList = (from SensorInfo si in superWave.Wave.Events select (si.Energy).ToString().Replace(",", ".")).ToList();
                    break;
                case "Time":
                    SuperWaveValuesYList = (from SensorInfo si in superWave.Wave.Events select (si.Time).ToString().Replace(",", ".")).ToList();
                    break;
            }

            foreach (Wave w in Waves)
            {
                TimeList.Add((from SensorInfo si in w.Events select ((int)((((si.Time.Seconds - w.Events[0].Time.Seconds) * 1000) - w.Events[0].MSec + si.MSec) * 1000)).ToString()).ToArray());

                switch (Param)
                {
                    case "Amplitude":
                        ValuesYList.Add((from SensorInfo si in w.Events select (si.Amplitude).ToString().Replace(",", ".")).ToArray());
                        break;
                    case "Energy":
                        ValuesYList.Add((from SensorInfo si in w.Events select (si.Energy).ToString().Replace(",", ".")).ToArray());
                        break;
                    case "Time":
                        ValuesYList.Add((from SensorInfo si in w.Events select (si.Time).ToString().Replace(",", ".")).ToArray());
                        break;
                }
            }

            CharacterVector fileNameVector = engine.CreateCharacterVector(new[] { FileName });
            engine.SetSymbol("fileName", fileNameVector);
            engine.Evaluate("png(fileName,  width=20, height=20, units='in', res = 200)");
            engine.SetSymbol("time" + 0, CreateCharacterVector(SuperWaveTimeList));
            engine.SetSymbol("amplitude" + 0, CreateCharacterVector(SuperWaveValuesYList));
            engine.Evaluate("plot(time" + 0 + ",amplitude" + 0 + ", type = 'b',pch = " + 0 + ",cex = 1.5, cex.lab = 1.8, cex.axis = 2, lwd.axis = 2, lwd = 10, col=rgb(0.2, 1, 0.2), adj.lab = 2, xlab=\"Time, µs\", ylab=\"Amplitude, dB\", xlim = c(0, 1000), ylim = c(40, 100))");

            for (int i = 0; i < Waves.Count(); i++)
            {
                engine.SetSymbol("time" + i, CreateCharacterVector(TimeList[i]));
                engine.SetSymbol("amplitude" + i, CreateCharacterVector(ValuesYList[i]));
                engine.Evaluate("lines(time" + i + ",amplitude" + i + ",pch = " + i + ",cex = 1.5, type = 'b', lwd = 3)");
            }

            engine.SetSymbol("numbers", CreateCharacterVector((from Wave w in Waves select w.Number.ToString()).ToArray()));
            engine.Evaluate("legend(\"topright\", legend = c(numbers), pch = 0:" + Waves.Count + ",title = \"Events\", cex = 2)");

            engine.Evaluate("dev.off()");
        }


        public void LinearModel(IEnumerable<Wave> Waves, string FileName)
        {
            List<string[]> TimeList = new List<string[]>();
            List<string[]> AmplitudeList = new List<string[]>();

            foreach (Wave w in Waves)
            {
                TimeList.Add((from SensorInfo si in w.Events select ((int)((((si.Time.Seconds - w.Events[0].Time.Seconds) * 1000) - w.Events[0].MSec + si.MSec) * 1000)).ToString()).ToArray());
                AmplitudeList.Add((from SensorInfo si in w.Events select (si.Amplitude).ToString().Replace(",", ".")).ToArray());
            }

            CharacterVector fileNameVector = engine.CreateCharacterVector(new[] { FileName });
            engine.SetSymbol("fileName", fileNameVector);
            engine.Evaluate("png(fileName,  width=20, height=20, units='in', res = 200)");

            engine.SetSymbol("time" + 0, CreateCharacterVector(TimeList[0]));
            engine.SetSymbol("amplitude" + 0, CreateCharacterVector(AmplitudeList[0]));

            engine.Evaluate("lm.D9 <- lm(amplitude0~time0)");
            engine.Evaluate("plot(lm.D9)");
            engine.Evaluate("dev.off()");
        }


    }

}

