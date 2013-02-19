using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace DiffurTranslator2
{
    public class DError
    {
        public static int ErrorCounter { get; set; }
        public static RichTextBox CodeInput{ get; set; }
        public static RichTextBox ErrorOuptut{get; set;}
        
        public static void SetOutput(ref RichTextBox CodeTextBox, ref RichTextBox DebugTextBox)
        {
            CodeInput = CodeTextBox;
            ErrorOuptut = DebugTextBox;
        }

        public static void Errors(string msg)
        {

            ErrorCounter++;

            if (ErrorCounter < 2)
            {
                ErrorOuptut.Text += (msg + ", cтрока = " + DText.Line + ", cтолбец = " + DText.Pos + '\n');
                CodeInput.Focus();
                CodeInput.Select(DText.PrevLexPos, 20);
            }
            
        }

        public static void Expected(string msg)
        {
            Errors(" Ожидается " + msg);
        }

        public static void Warning()
        {

        }
    }
}
