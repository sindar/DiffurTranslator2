﻿using System;
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

            if (DPars.Methods.Contains("euler"))
            {
                pictureBox1.ImageLocation = MainForm.StartPath + "\\result\\graf_eu.bmp";
            }
            if (DPars.Methods.Contains("heun"))
            {
                pictureBox4.ImageLocation = MainForm.StartPath + "\\result\\graf_heun.bmp";
            }
            if (DPars.Methods.Contains("RK2"))
            {
                pictureBox2.ImageLocation = MainForm.StartPath + "\\result\\graf_RK2.bmp";
            }
            if (DPars.Methods.Contains("RK4"))
            {
                pictureBox3.ImageLocation = MainForm.StartPath + "\\result\\graf_RK4.bmp";
            }
            if (DPars.Methods.Contains("RKF5"))
            {
                pictureBox5.ImageLocation = MainForm.StartPath + "\\result\\graf_RKF5.bmp";
            }
        }

    }
}
