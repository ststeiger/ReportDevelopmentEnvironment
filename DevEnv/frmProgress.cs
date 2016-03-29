using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DevEnv
{
    public partial class frmProgress : Form
    {
        public frmProgress()
        {
            InitializeComponent();
        }

        public void StartDisplay()
        {
            pgInstallation.Maximum = 100;
            pgInstallation.Minimum = 0;
            pgInstallation.Value = 0;
        }

        public void SetValue(double value)
        {
            pgInstallation.Maximum = 100;
            pgInstallation.Minimum = 0;
            pgInstallation.Value = (int)value;
        }


    }
}
