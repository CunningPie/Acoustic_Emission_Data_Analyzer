using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AEDataAnalyzer
{
    public partial class CorrelationOptions : Form
    {
        public List<string> Params, CorrelationTypes;
        public string Op;

        public CorrelationOptions()
        {
            InitializeComponent();

            Params = new List<string>();
            CorrelationTypes = new List<string>();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (!Amplitude.Checked && !Energy.Checked && !Time.Checked && !CountsDuration.Checked ||
                !PearsonCoeff.Checked && !FechnerCoeff.Checked ||
                !Sum.Checked && !Mult.Checked)
            {
                MessageBox.Show("Выберите опции для корреляции!");
            }
            else
            {
                if (Amplitude.Checked)
                    Params.Add("Amplitude");

                if (Energy.Checked)
                    Params.Add("Energy");

                if (Time.Checked)
                    Params.Add("Time");

                if (CountsDuration.Checked)
                    Params.Add("CountsDuration");

                if (PearsonCoeff.Checked)
                    CorrelationTypes.Add("Pearson");

                if (FechnerCoeff.Checked)
                    CorrelationTypes.Add("Fechner");

                if (Sum.Checked)
                    Op = "Sum";
                else
                    Op = "Mult";

                DialogResult = DialogResult.OK;
            }
        }

        private void Sum_CheckedChanged(object sender, EventArgs e)
        {
            Mult.Checked = !Mult.Checked;
        }

        private void Mult_CheckedChanged(object sender, EventArgs e)
        {
            Sum.Checked = !Sum.Checked;
        }

        private void Counts_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
