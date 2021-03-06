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
   
    public partial class MainForm : Form
    {

        public static string StartPath { get; set; }
        public static bool bLegendChecked { get; set; }
        
        public MainForm(string startpath)
        {
            InitializeComponent();
            DError.SetOutput(ref this.CodeRTextBox, ref this.DebugRTextBox);
            StartPath = startpath;
            DScan.InitKW();
            bLegendChecked = LegendToolStripMenuItem.Checked;
        }

        public void Translate()
        {
            DebugRTextBox.Clear();

            if (DFile.CurrentFileName == "")
            {
                MessageBox.Show("Нечего транслировать! Сначала сохраните файл!");
                return;
            }

            DFile.SaveFile(OpenFileDialog1.FileName, ref CodeRTextBox);
            DText.ResetText();
            DScan.InitScan();
            DPars.Compile(ref LexRTextBox, ref CodeRTextBox);
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog1.ShowDialog();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog1.ShowDialog();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OpenFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            DFile.FillCodeTextBox(OpenFileDialog1.FileName, ref CodeRTextBox);
        }

        private void SaveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            DFile.SaveFile(SaveFileDialog1.FileName, ref CodeRTextBox);
        }

       

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(DText.TrFile != null)
                DText.CloseText();    
        }

        private void TranslateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Translate();
        }

        private void LegendToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            bLegendChecked = LegendToolStripMenuItem.Checked;
        }

    }
}
