using AEDataAnalyzer.Correlation;
using AEDataAnalyzer.Data_File;
using AEDataAnalyzer.User_Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AEDataAnalyzer
{
    public partial class MainView : Form
    {
        DataAnalysis Analyser;
        List<Wave> Waves;
        DataGridView DataGrid;
        TabControl Tabs;
        RFunctions RFunc;
        PearsonCriticalValuesTable pValues = new PearsonCriticalValuesTable("tCriteria.txt");

        List<Dictionary<KeyValuePair<Wave, Wave>, double>> CorrelationResults;
        List<double> Thresholds;
        List<SuperWave> SuperWaves;

        string CurrentFileName;

        public MainView()
        {
            InitializeComponent();

            RFunc = new RFunctions();
            CorrelationResults = new List<Dictionary<KeyValuePair<Wave, Wave>, double>>();
            Thresholds = new List<double>();
            SuperWaves = new List<SuperWave>();
        }

        public void ConstructTable(ref DataGridView DataGrid, Dictionary<string, int> Columns)
        {
            var Number = new DataGridViewColumn();
            Number.HeaderText = "№";
            Number.ReadOnly = true;
            Number.Frozen = true;
            Number.CellTemplate = new DataGridViewTextBoxCell();

            DataGrid.Columns.Add(Number);

            List<string> Titles = (from KeyValuePair<string, int> pair in Columns orderby pair.Value select pair.Key).ToList();

            foreach (string title in Titles)
            {
                var column = new DataGridViewColumn();
                column.HeaderText = title;
                column.ReadOnly = true;
                column.Frozen = true;
                column.CellTemplate = new DataGridViewTextBoxCell();

                DataGrid.Columns.Add(column);
            }

        }

        private void Menu_File_Open_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.Text = ofd.FileName;
                CurrentFileName = Path.GetFileNameWithoutExtension(ofd.FileName);

                if (Tabs == null)
                    Tabs = new TabControl();

                TabPage page = new TabPage(CurrentFileName);

                DataGrid = new DataGridView();
                DataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                DataGrid.Dock = DockStyle.Fill;

                Analyser = new DataAnalysis(ofd.FileName);

                ConstructTable(ref DataGrid, Analyser.Columns);

                int i = 1;

                foreach (SensorInfo si in Analyser.Data)
                {
                    var ColumnsData = new List<string>();
                    ColumnsData.Add((i++).ToString());
                    ColumnsData.AddRange(si.Params);

                    DataGrid.Rows.Add(ColumnsData.ToArray());
                }

                page.Controls.Add(DataGrid);

                Tabs.Controls.Add(page);
                Tabs.Dock = DockStyle.Fill;
                Tabs.Anchor = AnchorStyles.Top | AnchorStyles.Left;

                ContextMenu contextMenu = new ContextMenu();
                contextMenu.MenuItems.Add(new MenuItem("Закрыть вкладку", ContextMenu_Close_Click));
                contextMenu.MenuItems.Add(new MenuItem("Закрыть все вкладки", ContextMenu_CloseAll_Click));
                contextMenu.MenuItems.Add("-");
                contextMenu.MenuItems.Add(new MenuItem("Поиск волн", Menu_Tools_FindWaves_Click));
                contextMenu.MenuItems.Add(new MenuItem("Построить график", Menu_Tools_Plot_Click));
                contextMenu.MenuItems.Add(new MenuItem("Корреляция", Menu_Tools_Correlation_Click));

                Tabs.ContextMenu = contextMenu;

                this.Controls.Add(Tabs);
                Tabs.BringToFront();

                Menu_Tools_Plot.Enabled = false;
                Menu_Tools_Correlation.Enabled = false;
                Menu_Tools_Show_Waves.Enabled = false;
                Menu_Tools_Create_Super_Waves.Enabled = false;
                Menu_Tools_STF.Enabled = false;
                Menu_Tool_SW_Set_Correlation.Enabled = false;

                Menu_Tools_FindWaves.Enabled = true;

                CorrelationResults.Clear();
                Thresholds.Clear();
                SuperWaves.Clear();
            }
        }

        private void ContextMenu_Close_Click(object sender, EventArgs e)
        {
            Tabs.Controls.Remove(Tabs.SelectedTab);
        }

        private void ContextMenu_CloseAll_Click(object sender, EventArgs e)
        {
            Tabs.Controls.Clear();
        }


        private void Menu_Tools_FindWaves_Click(object sender, EventArgs e)
        {
            if (Analyser != null && Analyser.Data.Count > 0)
            {
                Waves = new List<Wave>();
                int wave_num = 1;

                for (int i = 0; i < Analyser.Data.Count; i++)
                {
                    if (Analyser.Data[i].SensorType == "LE")
                    {
                        var NewWave = new List<SensorInfo>();

                        NewWave.Add(Analyser.Data[i++]);

                        while (i < Analyser.Data.Count() && Analyser.Data[i].SensorType == "Ht" )//Analyser.Data[i].SensorType != "LE" && (double)(Analyser.Data[i].Time.Seconds - NewWave[0].Time.Seconds) * 1000 + Analyser.Data[i].MSec - NewWave[0].MSec < 1000)
                            NewWave.Add(Analyser.Data[i++]);

                        if (NewWave.Count > 3)
                            Waves.Add(new Wave(NewWave, wave_num++));
                        --i;
                    }
                }

                DataGridView WavesDataGrid = new DataGridView();
                WavesDataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                WavesDataGrid.Dock = DockStyle.Fill;

                Dictionary<string, int> Titles = new Dictionary<string, int>() { { "Id", 1 }, { "Channel", 2 }, { "Time", 3 }, { "MSec", 4 }, { "Amplitude", 5 }, { "Energy", 6 }, { "Duration", 7 }, { "Counts", 8 } };
                ConstructTable(ref WavesDataGrid, Titles);

                int k = 0;

                foreach (Wave w in Waves)
                {
                    if (w.Events.Count > 3)
                    {
                        foreach (SensorInfo si in w.Events)
                        {
                            var ColumnsData = new List<string>();
                            ColumnsData.Add((k).ToString());
                            ColumnsData.AddRange(si.Params);

                            var row = (DataGridViewRow)WavesDataGrid.Rows[0].Clone();

                            int cell_num = 0;
                            foreach (string cellData in ColumnsData)
                                row.Cells[cell_num++].Value = cellData;

                            if (si.SensorType == "LE")
                                row.DefaultCellStyle.BackColor = Color.LightGray;
                            else
                                row.DefaultCellStyle.BackColor = Color.White;

                            WavesDataGrid.Rows.Add(row);
                        }
                    }

                    k++;
                }

                TabPage page = new TabPage(CurrentFileName + "_Waves");
                page.Controls.Add(WavesDataGrid);
                Tabs.Controls.Add(page);

                Menu_Tools_Correlation.Enabled = true;
                Menu_Tools_Show_Waves.Enabled = true;
                Menu_Tools_STF.Enabled = true;
            }
        }

        private void Menu_Tools_Plot_Click(object sender, EventArgs e)
        {
            PlotAttributes pa = new PlotAttributes();

            if (pa.ShowDialog() == DialogResult.OK)
            {
                if (Waves != null)
                {
                    string DirectoryName = "F:/Научная Статья/Графики/Waves_" + CurrentFileName;

                    if (!Directory.Exists(DirectoryName))
                        Directory.CreateDirectory(DirectoryName);
                    else
                    {
                        DirectoryName = DirectoryName + "_" + (DateTime.Now.ToShortDateString() + "_" + DateTime.Now.TimeOfDay.ToString()).Replace(":", "-");
                        Directory.CreateDirectory(DirectoryName);
                    }

                    for (int i = 0; i < Waves.Count(); i++)
                    {
                        int threshold_num = 0;

                        foreach (var coeffs in CorrelationResults)
                        {
                            var WavesPairs = (from KeyValuePair<KeyValuePair<Wave, Wave>, double> pair in coeffs where ((Waves.FindIndex(w => w == pair.Key.Value) == i || Waves.FindIndex(w => w == pair.Key.Key) == i) && pair.Value >= Thresholds[threshold_num]) select pair.Key).ToList();
                            List<Wave> CorrelatedWaves = new List<Wave>();

                            CorrelatedWaves.Add(WavesPairs.Find(w => w.Key.Number == i).Key);

                            foreach (var pair in WavesPairs)
                            {
                                if (pair.Key.Number == i && !CorrelatedWaves.Contains(pair.Value))
                                    CorrelatedWaves.Add(pair.Value);
                                else if (pair.Value.Number == i && !CorrelatedWaves.Contains(pair.Key))
                                    CorrelatedWaves.Add(pair.Key);
                            }

                            if (CorrelatedWaves.Count() > 1)
                            {
                                foreach (string Param in pa.Params)
                                {
                                    Directory.CreateDirectory(DirectoryName + "/" + "CorrelatedWaves_" + Param);

                                    RFunc.PlotWavesCollection(CorrelatedWaves, DirectoryName + "/" + "CorrelatedWaves_" + Param + "/Wave" + Param + i + ".png", Param);

                                    TabPage page = new TabPage("Wave" + Param + i);

                                    PictureBox Plot = new PictureBox();
                                    Plot.Load(DirectoryName + "/" + "CorrelatedWaves_" + Param + "/Wave" + Param + i + ".png");
                                    Plot.Dock = DockStyle.Fill;
                                    Plot.SizeMode = PictureBoxSizeMode.Zoom;
                                    page.Controls.Add(Plot);
                                    Tabs.Controls.Add(page);
                                }
                            }

                            threshold_num++;
                        }
                    }
                }
            }
        }

        private void Menu_Tools_Correlation_Click(object sender, EventArgs e)
        {
            CorrelationOptions co = new CorrelationOptions();

            if (co.ShowDialog() == DialogResult.OK)
            {
                Thresholds.Clear();
                CorrelationResults = CorrelationFunction(Waves, co.Params, co.Op, co.CorrelationTypes);

                string FileNameModifier = "";

                foreach (string s in co.Params)
                    FileNameModifier += s;

                foreach (string s in co.CorrelationTypes)
                    FileNameModifier += s;

                FileNameModifier += co.Op;

                ConstructCorrelationPairTable(FileNameModifier);

                foreach (var CorrTable in CorrelationResults)
                    ConstructCorrelationMatrix(CorrTable, FileNameModifier);

                Menu_Tools_Create_Super_Waves.Enabled = true;
            }
        }

        private void ConstructCorrelationPairTable(string FileNameModifier)
        {
            int thresholdNum = 0;

            foreach (Dictionary<KeyValuePair<Wave, Wave>, double> CorrTable in CorrelationResults)
            {
                TabPage page = new TabPage(CurrentFileName + "_CorrelationCoeffs" + FileNameModifier);
                DataGridView dataGrid = new DataGridView();
                dataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                dataGrid.Dock = DockStyle.Fill;

                ConstructTable(ref dataGrid, new Dictionary<string, int>() { { "Пара", 1 }, { "Коэффициент", 2 } });

                int k = 1;
                int rowNum = 0;

                foreach (KeyValuePair<KeyValuePair<Wave, Wave>, double> pair in CorrTable)
                {
                    int i = Waves.FindIndex(w => w == pair.Key.Key), j = Waves.FindIndex(w => w == pair.Key.Value);

                    dataGrid.Rows.Add(k++, i + ", " + j, Math.Abs(Math.Round(pair.Value, 4)));

                    if (pair.Value > Thresholds[thresholdNum])
                        dataGrid.Rows[rowNum].DefaultCellStyle.BackColor = Color.LightGreen;
                    else
                        dataGrid.Rows[rowNum].DefaultCellStyle.BackColor = Color.White;

                    rowNum++;
                }

                page.Controls.Add(dataGrid);
                Tabs.Controls.Add(page);

                Menu_Tools_Plot.Enabled = true;
            }
        }

        private void ConstructCorrelationMatrix(Dictionary<KeyValuePair<Wave, Wave>, double> CorrTable, string FileNameModifier)
        {
            TabPage page = new TabPage(CurrentFileName + "_CorrelationCoeffs" + FileNameModifier);
            DataGridView dataGrid = new DataGridView();
            dataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            dataGrid.Dock = DockStyle.Fill;
            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            int w_num = 0;

            var Number = new DataGridViewColumn();
            Number.HeaderText = "№";
            Number.ReadOnly = true;
            Number.Frozen = false;
            Number.CellTemplate = new DataGridViewTextBoxCell();

            dataGrid.Columns.Add(Number);

            var Waves = new List<Wave>();

            Waves = (from KeyValuePair<Wave, Wave> w in CorrTable.Keys select w.Key).Distinct().ToList();

            foreach (Wave w in Waves)
            {
                var WaveColumn = new DataGridViewColumn();
                WaveColumn.HeaderText = w_num.ToString();
                WaveColumn.ReadOnly = true;
                WaveColumn.Frozen = false;
                WaveColumn.CellTemplate = new DataGridViewTextBoxCell();

                dataGrid.Columns.Add(WaveColumn);
                w_num++;
            }

            for (int i = 0; i < Waves.Count(); i++)
            {
                double[] coeffs = (from KeyValuePair<KeyValuePair<Wave, Wave>, double> pair in CorrTable where pair.Key.Value == Waves[i] || pair.Key.Key == Waves[i] select pair.Value).ToArray();

                DataGridViewRow row = (DataGridViewRow)dataGrid.Rows[0].Clone();

                int cell = 1;
                row.Cells[0].Value = i;

                foreach (double d in coeffs)
                {
                    if (d > Thresholds[0])
                        row.Cells[cell].Style.BackColor = Color.LightGreen;
                    else
                        row.Cells[cell].Style.BackColor = Color.White;

                    row.Cells[cell++].Value = Math.Abs(Math.Round(d, 2));
                }

                dataGrid.Rows.Add(row);
            }

            page.Controls.Add(dataGrid);
            Tabs.Controls.Add(page);

            Menu_Tools_Plot.Enabled = true;
        }

        private List<Dictionary<KeyValuePair<Wave, Wave>, double>> CorrelationFunction(List<Wave> Waves, IEnumerable<String> ParamTypes, string Op, IEnumerable<String> CorrCoeffs)
        {
            var ListCoeffs = new List<Dictionary<KeyValuePair<Wave, Wave>, double>>();
            var ResultList = new List<Dictionary<KeyValuePair<Wave, Wave>, double>>();

            foreach (string Coeff in CorrCoeffs)
            {
                var Result = new Dictionary<KeyValuePair<Wave, Wave>, double>();
                double threshold = 0;

                if (Op == "Mult")
                    threshold = 1;

                foreach (string param in ParamTypes)
                {
                    Dictionary<KeyValuePair<Wave, Wave>, double> CorrelatedCoeffs = new Dictionary<KeyValuePair<Wave, Wave>, double>();

                    List<double> ValuesX = new List<double>(), ValuesY = new List<double>();

                    if (Coeff == "Pearson")
                    {
                        if (Op == "Mult")
                            switch (param)
                            {
                                case "Time":
                                    threshold *= PearsonThresholds.Time;
                                    break;
                                case "Amplitude":
                                    threshold *= PearsonThresholds.Amplitude;
                                    break;
                                case "Energy":
                                    threshold *= PearsonThresholds.Energy;
                                    break;
                                default:
                                    threshold *= PearsonThresholds.Default;
                                    break;
                            }
                        else if (Op == "Sum")
                            switch (param)
                            {
                                case "Time":
                                    threshold += PearsonThresholds.Time;
                                    break;
                                case "Amplitude":
                                    threshold += PearsonThresholds.Amplitude;
                                    break;
                                case "Energy":
                                    threshold += PearsonThresholds.Energy;
                                    break;
                                default:
                                    threshold += PearsonThresholds.Default;
                                    break;
                            }
                    }
                    else if (Coeff == "Fechner")
                    {
                        if (Op == "Mult")
                            threshold *= 0.7;
                        else if (Op == "Sum")
                            threshold += 0.7;
                    }

                    for (int i = 0; i < Waves.Count(); i++)
                    {


                        foreach (Wave w in Waves.Skip(i))
                        {
                            var pair = new KeyValuePair<Wave, Wave>(Waves[i], w);

                            switch (Coeff)
                            {
                                case "Pearson":
                                    CorrelatedCoeffs.Add(pair, PearsonCorrelation.Coefficient(Waves[i], w, param, pValues));
                                    break;
                                
                                case "Fechner":
                                    CorrelatedCoeffs.Add(pair, FechnerCorrelation.Coefficient(Waves[i], w, param));
                                    break;
                                    /*
                                case "Fechner":
                                    CorrelatedCoeffs.Add(pair, WilcoxonCorrelation.Criteria(Waves[i], w, param));
                                    break;*/
                            }
                        }
                    }

                    ListCoeffs.Add(CorrelatedCoeffs);
                }

                for (int i = 0; i < Waves.Count(); i++)
                    for (int j = i; j < Waves.Count(); j++)
                    {
                        double r = 0;

                        switch (Op)
                        {
                            case "Mult":
                                r = 1;

                                foreach (var coeffs in ListCoeffs)
                                    r *= coeffs[new KeyValuePair<Wave, Wave>(Waves[i], Waves[j])];

                                Result.Add(new KeyValuePair<Wave, Wave>(Waves[i], Waves[j]), r);
                                break;
                            case "Sum":
                                foreach (var coeffs in ListCoeffs)
                                    r += coeffs[new KeyValuePair<Wave, Wave>(Waves[i], Waves[j])];

                                break;
                        }

                        Result.Add(new KeyValuePair<Wave, Wave>(Waves[i], Waves[j]), r);
                    }

                Thresholds.Add(threshold);
                ResultList.Add(Result);
            }

            return ResultList;
        }

        private void CreateWavePlot(Wave Wave, string num, string DirectoryName)
        {
            RFunc.PlotWave(Wave, DirectoryName + "/Wave" + num + ".png");

            TabPage page = new TabPage("Wave" + num);

            PictureBox Plot = new PictureBox();
            Plot.Load(DirectoryName + "/Wave" + num + ".png");
            Plot.Dock = DockStyle.Fill;
            Plot.SizeMode = PictureBoxSizeMode.Zoom;
            page.Controls.Add(Plot);
            Tabs.Controls.Add(page);
        }

        private void Menu_Tools_Show_Waves_Click(object sender, EventArgs e)
        {
            string DirectoryName = "F:/Научная Статья/Графики/Waves_" + CurrentFileName;

            if (!Directory.Exists(DirectoryName))
                Directory.CreateDirectory(DirectoryName);
            else
            {
                DirectoryName = DirectoryName + "_" + (DateTime.Now.ToShortDateString() + "_" + DateTime.Now.TimeOfDay.ToString()).Replace(":", "-");
                Directory.CreateDirectory(DirectoryName);
            }

            if (Waves != null)
            {
                for (int i = 0; i < Waves.Count; i++)
                    CreateWavePlot(Waves[i], i.ToString(), DirectoryName);
            }
        }

        private void Menu_Tools_Create_Super_Waves_Click(object sender, EventArgs e)
        {
            string DirectoryName = "F:/Научная Статья/Графики/Waves_" + CurrentFileName + "_" + (DateTime.Now.ToShortDateString() + "_" + DateTime.Now.TimeOfDay.ToString()).Replace(":", "-") + "/" + "SuperWaves";

            Dictionary<SuperWave, List<Wave>> AllSuperWaves = new Dictionary<SuperWave, List<Wave>>();
            Dictionary<KeyValuePair<Wave, Wave>, double> SWCorrTable = new Dictionary<KeyValuePair<Wave, Wave>, double>();

            Directory.CreateDirectory(DirectoryName);

            for (int i = 0; i < Waves.Count(); i++)
            {
                int threshold_num = 0;
                foreach (var coeffs in CorrelationResults)
                {
                    var WavesPairs = (from KeyValuePair<KeyValuePair<Wave, Wave>, double> pair in coeffs where ((Waves.FindIndex(w => w == pair.Key.Value) == i || Waves.FindIndex(w => w == pair.Key.Key) == i) && pair.Value >= Thresholds[threshold_num]) select pair.Key).ToList();
                    List<Wave> CorrelatedWaves = new List<Wave>();

                    CorrelatedWaves.Add(WavesPairs.Find(w => w.Key.Number == i).Key);

                    foreach (var pair in WavesPairs)
                    {
                        if (pair.Key.Number == i && !CorrelatedWaves.Contains(pair.Value))
                            CorrelatedWaves.Add(pair.Value);
                        else if (pair.Value.Number == i && !CorrelatedWaves.Contains(pair.Key))
                            CorrelatedWaves.Add(pair.Key);
                    }

                    List<Wave> CorrectWaves = (from Wave wave in CorrelatedWaves group wave by wave.Events.Count into waveGroup orderby waveGroup.Key select waveGroup.ToList()).LastOrDefault();

                    if (CorrectWaves.Count >= 4)
                    {
                        SuperWave superWave = new SuperWave(CorrectWaves, i);

                        if (superWave.Powerset < 12)
                            AllSuperWaves.Add(superWave, CorrectWaves);
                    }
                }

                threshold_num++;
            }

            foreach (SuperWave w1 in AllSuperWaves.Keys)
                foreach (SuperWave w2 in AllSuperWaves.Keys)
                {
                    if (w1 != w2)
                    {
                        var corrCoeff = Math.Abs(PearsonCorrelation.Coefficient(w1.Wave, w2.Wave, "Amplitude", pValues) * PearsonCorrelation.Coefficient(w1.Wave, w2.Wave, "Time", pValues));

                        SWCorrTable.Add(new KeyValuePair<Wave, Wave>(w1.Wave, w2.Wave), corrCoeff);

                        if (corrCoeff >= PearsonThresholds.SuperSignalsThresholds)
                        {
                            if (w1.Powerset > w2.Powerset && w1.CoeffSum < double.PositiveInfinity)
                                w2.CoeffSum = double.PositiveInfinity;
                            else if (w2.CoeffSum < double.PositiveInfinity)
                            {
                                w1.CoeffSum = double.PositiveInfinity;
                                break;
                            }

                        }

                        w1.CoeffSum *= corrCoeff;
                    }
                }

            var SelectedClusters = AllSuperWaves.Keys.ToList();

            SelectedClusters = (from SuperWave sw in SelectedClusters where sw.CoeffSum < Double.PositiveInfinity orderby sw.CoeffSum select sw).ToList().Take(5).ToList();

            SelectedClusters = (from SuperWave sw in SelectedClusters orderby sw.Powerset descending select sw).Take(2).ToList();

            int swNum = 0;

            foreach (SuperWave sw in SelectedClusters)
            {
                RFunc.PlotSuperWavesCollection(AllSuperWaves[sw], sw, DirectoryName + "/SuperWave" + swNum + ".png", "Amplitude");

                TabPage page = new TabPage("SuperWave" + swNum);

                PictureBox Plot = new PictureBox();
                Plot.Load(DirectoryName + "/SuperWave" + swNum++ + ".png");
                Plot.Dock = DockStyle.Fill;
                Plot.SizeMode = PictureBoxSizeMode.Zoom;
                page.Controls.Add(Plot);
                Tabs.Controls.Add(page);
            }


            DataWriter dw = new DataWriter();


            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                dw.WriteNewSuperSignalsFile(ofd.FileName,  CurrentFileName, SelectedClusters);
            }
            //ConstructCorrelationMatrix(SWCorrTable, "SWCorrTable");
        }

        private void Menu_Tool_SW_Set_Correlation_Click(object sender, EventArgs e)
        {
        }

        private void Menu_Tools_STF_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(sfd.FileName))
                {
                    //int i = 0;

                    sw.WriteLine("Num Event Time Channel Amplitude");
                    foreach (Wave w in Waves)
                    {
                        if (w.Events.Count > 3)
                        {
                            if (((w.Events[3].Time - w.Events[1].Time).Milliseconds + (w.Events[3].MSec - w.Events[1].MSec)) > 0.2)
                            {
                                var firstSITime = w.Events[0].Time;
                                var firstSIMsec = w.Events[0].MSec;

                                foreach (SensorInfo si in w.Events)
                                {
                                    sw.WriteLine((w.Number + " " + si.SensorType + " " + ((si.Time - firstSITime).Milliseconds + (si.MSec - firstSIMsec)) + " " + si.Channel + " " + si.Amplitude).Replace(",", "."));
                                    //sw.WriteLine((i + " " + si.SensorType + " " + ((si.Time - firstSITime).Milliseconds + (si.MSec - firstSIMsec)) + " " + si.Channel + " " + si.Amplitude).Replace(",", "."));
                                }

                                //i++;
                            }
                        }
                    }
                }
            }
        }
    }
}
