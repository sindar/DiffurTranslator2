using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace DiffurTranslator2
{
    public enum tLex {lexEot, lexBegin, lexGiven, lexKoef, lexCauchy, lexMethod, lexGet, lexEnd, lexdxdt,
                      lexName, lexNum, lexInt, lexRpar, lexLpar, lexQRpar, lexQLpar, lexOper, lexPow, lexTspan,
                      lexStep, lexX0, lexFRpar, lexFLpar, lexColon, lexSemi, lexComma, lexDot, lexAss, 
                      lexOde45, lexEuler, lexPlot };

    public struct KWHash
    {
        public string Word;
        public tLex Lex;
    }
    
    class DScan
    {
        public const int NameLen = 31;
         
        public static tLex Lex { get; set; }
        public static string Name { get; set; }
        public static double Num { get; set; }
        public static int LexPos { get; set; }

        public const int KWNum = 14;
                
        private static int nkw;//номер ключевого слова

        public static KWHash[] KWTable = new KWHash[KWNum];

        private static void EnterKW(string nName, tLex nLex)
        {
            KWTable[nkw].Word = nName;
            KWTable[nkw].Lex = nLex;
            nkw++;
        }

        private static tLex TestKW()
        {
            int i = nkw - 1;
            while( (i>=0) && (Name != KWTable[i].Word))
                i--;
            if (i >= 0)
                return KWTable[i].Lex;
            else
                return tLex.lexName;
        }

        public static void Ident()
        {
            Name = "";
            int i = 0;
            do
            {
                if (i < NameLen)
                {
                    Name += DText.Ch;
                    i++;
                }
                else
                {
                    DError.Errors("Слишком длинное имя");
                    break;
                }
                DText.NextCh();
              
            } while ((DText.Ch >= 'A' && DText.Ch <= 'Z') || 
                     (DText.Ch >= 'a' && DText.Ch <= 'z') ||
                     (DText.Ch >= 'А' && DText.Ch <= 'Я') ||
                     (DText.Ch >= 'а' && DText.Ch <= 'я') || 
                     (DText.Ch >= '0' && DText.Ch <= '9'));

            Lex = TestKW();
        }

        public static void Number()
        {
            int dotCounter = 0;
            
            string sNum = "";
            Num = 0;
            do
            {
                sNum += (DText.Ch);
                DText.NextCh();

                if (DText.Ch == '.' && dotCounter == 0)
                {
                    sNum += ',';//(DText.Ch);
                    DText.NextCh();
                    dotCounter = 1;
                }

            }while(DText.Ch >= '0' && DText.Ch <= '9');

            if (dotCounter > 0)
            {
                Num = Convert.ToDouble(sNum);
                Lex = tLex.lexNum;
            }
            else
            {
                Num = Convert.ToInt32(sNum);
                Lex = tLex.lexInt;
            }
        }

        public static void NextLex()
        {

            while (DText.Ch == DText.chSpace || DText.Ch == DText.chTab || DText.Ch == DText.chEol || DText.Ch == DText.chRet)
                DText.NextCh();
            
            LexPos = DText.Pos;

            if ((DText.Ch >= 'A' && DText.Ch <= 'Z') || (DText.Ch >= 'a' && DText.Ch <= 'z')
                || (DText.Ch >= 'А' && DText.Ch <= 'Я') || (DText.Ch >= 'а' && DText.Ch <= 'я'))
            {
                Ident();
                return;
            }

            if (DText.Ch >= '0' && DText.Ch <= '9')
            {
                Number();
                return;
            }
                        
            switch(DText.Ch)
            {
                case '=':
                    DText.NextCh();
                    Lex = tLex.lexAss;
                    break;
                case '(':
                    DText.NextCh();
                    Lex = tLex.lexLpar;
                    break;
                case ')':
                    DText.NextCh();
                    Lex = tLex.lexRpar;
                    break;
                case '+':
                    DText.NextCh();
                    Lex = tLex.lexOper;
                    break;
                case '-':
                    DText.NextCh();
                    Lex = tLex.lexOper;
                    break;
                case '*':
                    DText.NextCh();
                    Lex = tLex.lexOper;
                    break;
                case '/':
                    DText.NextCh();
                    Lex = tLex.lexOper;
                    break;
                case '^':
                    DText.NextCh();
                    Lex = tLex.lexPow;
                    break;
                case ';':
                    DText.NextCh();
                    Lex = tLex.lexSemi;
                    break;
                case ',':
                    DText.NextCh();
                    Lex = tLex.lexComma;
                    break;
                case '[':
                    DText.NextCh();
                    Lex = tLex.lexQLpar;
                    break;
                case ']':
                    DText.NextCh();
                    Lex = tLex.lexQRpar;
                    break;    
                case DText.chEot:
                    Lex = tLex.lexEot;
                    break;
                default:
                    DError.Errors("Недопустимый символ, ");
                    Lex = tLex.lexEot;
                    break;
            }

        }
        
        public static void InitScan()
        {
            Name = "";
            NextLex();
        }

        public static void InitKW()
        {
            nkw = 0;
            EnterKW("Начало", tLex.lexBegin);
            EnterKW("Дано", tLex.lexGiven);
            EnterKW("Коэффициенты", tLex.lexKoef);
            EnterKW("Коши", tLex.lexCauchy);
            EnterKW("Метод", tLex.lexMethod);
            EnterKW("Вывести", tLex.lexGet);
            EnterKW("Конец", tLex.lexEnd);
            EnterKW("dxdt", tLex.lexdxdt);
            EnterKW("tspan", tLex.lexTspan);
            EnterKW("step", tLex.lexStep);
            EnterKW("x0", tLex.lexX0);
            EnterKW("ode45", tLex.lexOde45);
            EnterKW("euler", tLex.lexEuler);
            EnterKW("plot", tLex.lexPlot);
        }

    }
}
