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
   
    public partial class MainForm : Form
    {
        
        public MainForm()
        {
            InitializeComponent();
            DError.SetOutput(ref this.CodeRTextBox, ref this.DebugRTextBox);
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

        private void button1_Click(object sender, EventArgs e)
        {
            DebugRTextBox.Clear();
            DFile.SaveFile(DFile.CurrentFileName, ref CodeRTextBox);
            DText.ResetText();
            DScan.InitScan();
            DPars.Compile(ref LexRTextBox, ref CodeRTextBox);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(DText.TrFile != null)
                DText.CloseText();    
        }

        private void button2_Click(object sender, EventArgs e)
        {
            process1.Start();
            
        }
                
    }
}
