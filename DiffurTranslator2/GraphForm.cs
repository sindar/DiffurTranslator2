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
    public partial class GraphForm : Form
    {
        public GraphForm()
        {
            InitializeComponent();
            pictureBox1.ImageLocation = MainForm.StartPath + "\\result\\graf_eu.bmp";
            pictureBox2.ImageLocation = MainForm.StartPath + "\\result\\graf_ode23.bmp";
            pictureBox3.ImageLocation = MainForm.StartPath + "\\result\\graf_ode45.bmp";
            pictureBox4.ImageLocation = MainForm.StartPath + "\\result\\graf_heun.bmp";
            int i = 0;
        }

    }
}
