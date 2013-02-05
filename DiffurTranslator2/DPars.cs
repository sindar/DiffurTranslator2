﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace DiffurTranslator2
{
    class DPars
    {
        public static void Compile(ref RichTextBox stream)
        {
            stream.Clear();
            int n = 0;
            while(DScan.Lex != tLex.lexEot)
            {
                n++;
                stream.Text += DScan.Lex.ToString() + '\n' ;
                DScan.NextLex();
            }
            MessageBox.Show("Количество лексем: " + n);
            DText.CloseText();
        }
    }
}
