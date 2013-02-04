using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace DiffurTranslator2
{
    public class DFile
    {
        public static string CurrentFileName = "";
        
        //Очистка и заполнение текстбокса из фалйа
        public static void FillCodeTextBox(string filename, ref RichTextBox CodeRTextBox)
        {
            StreamReader FileIn;

            try
            {
                FileIn = new StreamReader(filename);
            }
            catch (IOException exc)
            {
                MessageBox.Show("Ошибка открытия файла!\n" + exc.Message);
                return;
            }

            CodeRTextBox.Clear();

            try
            {
                while (!FileIn.EndOfStream)
                {
                    CodeRTextBox.Text += FileIn.ReadLine() + '\n';
                }
            }
            catch (IOException exc)
            {
                MessageBox.Show("Ошибка чтения файла:\n" + exc.Message);
            }
            finally
            {
                FileIn.Close();
                CurrentFileName = filename;
            }
        }

        //Сохранение файла
        public static void SaveFile(string filename, ref RichTextBox CodeRTextBox)
        {
            StreamWriter FileOut = null;

            try
            {
                FileOut = new StreamWriter(filename);

                foreach (string str in CodeRTextBox.Lines)
                {
                    FileOut.WriteLine(str);
                }
            }
            catch (IOException exc)
            {
                MessageBox.Show("Ошибка записи файла:\n" + exc.Message);
            }
            finally
            {
                if (FileOut != null)
                    FileOut.Close();
                CurrentFileName = filename;
            }
        }
    }
}
