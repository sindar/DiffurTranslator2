using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiffurTranslator2
{
    public partial class WaitForm : Form
    {
        public WaitForm()
        {
            InitializeComponent();
            timer1.Start();
            for (int i = 0; i < 100; i++)
                progressBar1.Value++;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
