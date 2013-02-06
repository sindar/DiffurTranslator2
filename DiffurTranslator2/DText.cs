﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace DiffurTranslator2
{
    public class DText
    {
        public const char chSpace = ' ';
        public const char chTab = '\t';
        public const char chRet = '\r';
        public const char chEol = '\n';
        public const char chEot = '\0';
        
        public static StreamReader TrFile { get; set; }
        
        public static char Ch{ get; set;}
        public static int Line { get; set; }
        public static int Pos { get; set; }
        public static int CodePos { get; set; }
        public static int PrevLexPos { get; set; }
        public static bool error = false;
               
        //Открытие файла для трансляции и установка начальной позиции
        public static void ResetText()
        {
            try
            {
                TrFile = new StreamReader(DFile.CurrentFileName);
            }

            catch (IOException exc)
            {
                MessageBox.Show("Ошибка открытия файла!\n" + exc.Message);
                return;
            }

            Pos = 0;
            CodePos = 0;
            Line = 1;
            NextCh();
        }

        //Закрытие текстового файла
        public static void CloseText()
        {
            TrFile.Close();
        }

        //Чтение следующего символа
        public static void NextCh()
        {
            
            if (error)
            {
                TrFile.ReadToEnd();
            }
            
            if (TrFile.EndOfStream)
            {
                Ch = chEot;
                CloseText();
            }
            else
            {
                Ch = (char)TrFile.Read();
                CodePos++;

                if (Ch == '\r' && ((char)TrFile.Read() == '\n'))
                {
                    Pos = 0;
                    Line++;
                }

                if (Ch != '\t')
                    Pos++;
            }
        }

        
    }
}
