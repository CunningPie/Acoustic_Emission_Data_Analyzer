using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AEDataAnalyzer.User_Interface
{
    public partial class PlotAttributes : Form
    {
        public List<string> Params;

        public PlotAttributes()
        {
            InitializeComponent();

            Params = new List<string>();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (!Amplitude.Checked && !Energy.Checked && !Time.Checked)
                MessageBox.Show("Select Attributes!");
            else
            {
                DialogResult = DialogResult.OK;

                if (Amplitude.Checked)
                    Params.Add("Amplitude");
                if (Energy.Checked)
                    Params.Add("Energy");
                if (Time.Checked)
                    Params.Add("Time");

                Close();
            }

        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
