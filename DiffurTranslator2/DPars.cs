using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Collections;
using MLApp;

namespace DiffurTranslator2
{
    class DPars
    {
        static string sCurName = "";
        static int iCurIndex = 0;
        static double dCurNum = 0;
        static int EqCounter = 0;
        static int KoefCounter = 0;
        static char CurCh = '0';
        static string plotstring="";
        static ArrayList PlotParams = new ArrayList();
        
        static double min;
        static double step;
        static double max;

        public static Hashtable Koefs = new Hashtable();

        public static void Compile(ref RichTextBox Lexems, ref RichTextBox Code)
        {
            Lexems.Clear();

            DError.ErrorCounter = 0;

            Check(tLex.lexBegin, " 'Начало' ");
            Check(tLex.lexName, " имя программы ");
            Check(tLex.lexGiven, " 'Дано' ");
            DeclEq();
            Check(tLex.lexKoef, " 'Коэффициенты' ");
            DeclKoef();
            Check(tLex.lexCauchy, " 'Коши' ");
            DeclCauchy();
            Check(tLex.lexMethod, " 'Метод' ");
            DeclMethod();
            Check(tLex.lexGet, " 'Вывести' ");
            DeclPlot();
            Check(tLex.lexEnd, " 'Конец' ");

            
            if (DError.ErrorCounter == 0)
            {
                string sTemp = "";
                int j = 0;

                #region//----------------funsys-------------------
                Lexems.Text += "function dxdt=t_funsys(t,x);\n";
                                
                //коэффициенты
                ICollection KoefKeys = Koefs.Keys;
                
                foreach(string koef in KoefKeys)
                {
                    sTemp = koef + "=" + Koefs[koef];
                    Lexems.Text += sTemp + ";\n";
                }

                Lexems.Text += "\ndxdt=zeros(" + EqCounter + ", 1);\n\n";

                //уравнения
                for (int i = 1; i <= EqCounter; i++)
                {
                    sTemp = "";
                    j = Code.Text.IndexOf("dxdt(" + i);
                    do
                    {
                        sTemp += Code.Text[j];
                        j++;
                    } while (!Code.Text[j].Equals(';'));
                    Lexems.Text += sTemp + ";\n";
                }

                DFile.SaveFile(MainForm.StartPath +  "\\result\\t_funsys.m", ref Lexems);

                //-----------------funsys-------------------
                #endregion

                #region //-----------------ode-------------------
                sTemp = "";

                Lexems.Clear();
                Lexems.Text += "function t_ode()\n";
                Lexems.Text += "tspan=[" + min + ':' + step.ToString().Replace(',','.') + ':' + max + "]\n";
                
                j = Code.Text.IndexOf("x0");
                do
                {
                    sTemp += Code.Text[j];
                    j++;
                } while (!Code.Text[j].Equals(']'));

                Lexems.Text += sTemp + "];\n";
                Lexems.Text += "[t,x]=ode45(@t_funsys,tspan,x0);\n";
                Lexems.Text += "f = figure('Visible','off')\n";

                sTemp = "";
                j = 1;
                foreach (object p in PlotParams)
                {
                    sTemp += p.ToString();
                    if (j < PlotParams.Count)
                        sTemp += ',';
                    j++;
                }

                Lexems.Text += "plot (t,x(:,[" + sTemp + "]),'lineWidth',3);\n";

                Lexems.Text += "grid on\n";
                Lexems.Text += "legend('x`1','x`2','x`3')\n";
                Lexems.Text += "print('-dbmp','-r80','graf_ode.bmp')\n";

                DFile.SaveFile(MainForm.StartPath +  "\\result\\t_ode.m", ref Lexems);
                //-----------------ode-------------------
                #endregion
                
                #region//----------------euler-------------------
                Lexems.Clear();
                sTemp = "";
                j = 0;

                Lexems.Text += "function [ts,data]=t_euler()\n";

                j = Code.Text.IndexOf("x0");
                do
                {
                    sTemp += Code.Text[j];
                    j++;
                } while (!Code.Text[j].Equals(']'));

                Lexems.Text += sTemp + "];\n";
                Lexems.Text += "t0 =" + min + "; dt = " + step.ToString().Replace(',','.') + "; tn = " + max + ";\n";
                Lexems.Text += "Nsteps = round(tn/dt)\n";
                Lexems.Text += "ts = zeros(Nsteps,1)\n";
                Lexems.Text += "data = zeros(Nsteps,length(x0))\n";
                Lexems.Text += "ts(1) = t0\n"; 
                Lexems.Text += "data(1,:) = x0'\n";
                Lexems.Text += "for i =1:Nsteps\n";
                Lexems.Text += "dxdt= feval(@t_funsys,t0,x0)\n";
                Lexems.Text += "x0=x0+dxdt*dt\n";
                Lexems.Text += "t0 = t0+dt\n";
                Lexems.Text += "ts(i+1) = t0\n";
                Lexems.Text += "data(i+1,:) = x0'\n";
                Lexems.Text += "end\n";
                Lexems.Text += "f = figure('Visible','off')\n";
                
                sTemp = "";
                j = 1;
                foreach (object p in PlotParams)
                {
                    sTemp += p.ToString();
                    if (j < PlotParams.Count)
                        sTemp += ',';
                    j++;
                }
                
                Lexems.Text += "plot (ts,data(:,[" + sTemp + "]),'lineWidth',3);\n";

                Lexems.Text += "grid on\n";
                Lexems.Text += "legend('x`1','x`2','x`3')\n";
                Lexems.Text += "print('-dbmp','-r80','graf_eu.bmp')\n";
                Lexems.Text += "end\n";

                DFile.SaveFile(MainForm.StartPath + "\\result\\t_euler.m", ref Lexems);
                #endregion
                
                MLApp.MLApp MatLabApp = new MLApp.MLApp();
                
                MatLabApp.Execute("cd " + MainForm.StartPath + "\\result") ;
                MatLabApp.Execute("t_ode");
                MatLabApp.Execute("t_euler");
                
                WaitForm waitwin = new WaitForm();
                waitwin.Show();
                
                MatLabApp.Quit();

            }

            DText.CloseText();
        }
          
        public static void Check(tLex ExpLex, string sWord)
        {

            if (DScan.Lex == tLex.lexInt && ExpLex == tLex.lexNum)
            {
                dCurNum = DScan.Num;
                DScan.NextLex();
                return;
            }
            
            if (DScan.Lex != ExpLex)
            {
                DError.Expected(sWord);
            }

            else
            {
                DText.PrevLexPos = DText.CodePos;

                if (DScan.Lex == tLex.lexName || DScan.Lex == tLex.lexdxdt)
                    sCurName = DScan.Name + '\n';

                if (DScan.Lex == tLex.lexInt)
                    iCurIndex = (int)DScan.Num;

                if (DScan.Lex == tLex.lexNum)
                    dCurNum = DScan.Num;

                DScan.NextLex();
            }
        }

        public static bool CheckEotLex()
        {
            if (DScan.Lex == tLex.lexEot)
            {
                DScan.NextLex();
                return true;
            }
            else
                return false;
        }

        public static void DeclEq()
        {
            
            EqCounter = 0;
            
            while (DScan.Lex == tLex.lexdxdt)
            {
                EqCounter++;
                
                DScan.NextLex();
                Check(tLex.lexLpar, " ( ");
                
                Check(tLex.lexInt, " число ");
                if (iCurIndex != EqCounter)
                {
                    DError.Expected(" индекс i = 1, 2, 3, ..., n");
                }
                
                Check(tLex.lexRpar, " ) ");
                
                Check(tLex.lexAss, " = ");
                
                if (DScan.Lex != tLex.lexNum && DScan.Lex != tLex.lexInt && DScan.Lex != tLex.lexName &&
                    DScan.Lex != tLex.lexOper && DScan.Lex != tLex.lexLpar)
                    DError.Expected(" число, идентификатор, математическая операция(+, -, *, /,) или левая скобка ");

                Expression();
                DScan.NextLex();
            }

            if (EqCounter == 0)
                DError.Expected(" dxdt ");       
        }

        public static void DeclKoef()
        {
            KoefCounter = 0;
            string sTemp = "";

            while (DScan.Lex == tLex.lexName)
            {
                sCurName = DScan.Name;
                KoefCounter++;

                if (!Koefs.ContainsKey(sCurName))
                {
                    DError.Errors(" данного коэффициента нет в уравнениях ");
                    break;
                }
                DScan.NextLex();
                Check(tLex.lexAss, " = ");
                Check(tLex.lexNum, " число ");
                sTemp = dCurNum.ToString().Replace(',', '.');
                
                /*if (DScan.Lex == tLex.lexNum)
                {
                    sTemp = dCurNum.ToString().Replace(',', '.');
                }
                else if (DScan.Lex == tLex.lexInt)
                {
                    sTemp = iCurIndex.ToString();
                }
                else
                    DError.Expected(" целое или вещественнное число ");*/

                //DScan.NextLex();
                Koefs[sCurName] = sTemp;
                Check(tLex.lexSemi, " ';' ");
                
            }

            if (KoefCounter == 0)
            {
                DError.Expected(" коэффициент ");
            }
        }

        public static void DeclCauchy()
        {
            Check(tLex.lexTspan, " tspan(промежуток интегрирования ");
            Check(tLex.lexAss, " = ");
            Check(tLex.lexQLpar, " [ ");
            Check(tLex.lexNum, " число ");
            min = dCurNum;
            Check(tLex.lexComma, " ',' ");
            Check(tLex.lexNum, " число ");
            max = dCurNum;
            Check(tLex.lexQRpar, " ] ");
            Check(tLex.lexSemi, " ; ");
            
            Check(tLex.lexStep, " step(шаг интегрирования) ");
            Check(tLex.lexAss, " = ");
            Check(tLex.lexNum, " число ");
            step = dCurNum;
            Check(tLex.lexSemi, " ; ");
            
            Check(tLex.lexX0, " x0(начальные условия) ");
            Check(tLex.lexAss, " = ");
            Check(tLex.lexQLpar, " [ ");

            for (int i = 1; i <= EqCounter; i++)
            {
                Check(tLex.lexNum, " число ");
                if(i < EqCounter)
                    Check(tLex.lexSemi, " ; ");
            }

            Check(tLex.lexQRpar, " ] ");
            Check(tLex.lexSemi, " ; ");

        }

        public static void DeclMethod()
        {
         
            if (DScan.Lex != tLex.lexEuler && DScan.Lex != tLex.lexOde45)
                DError.Expected(" 1 или 2 численных метода - ode45(Рунге-Кутта), euler(Эйлера)");
            else if (DScan.Lex == tLex.lexEuler)
            {
                CheckEuler();
            }
            else if (DScan.Lex == tLex.lexOde45)
            {
                CheckOde45();
            }

        }

        public static void DeclPlot()
        {
            PlotParams.Clear();
            Check(tLex.lexPlot, " plot ");
            Check(tLex.lexQLpar, " [ ");
            
            for (int i = 1; i <= EqCounter; i++)
            {
                Check(tLex.lexInt, " число ");
                PlotParams.Add(iCurIndex);
                if (i < EqCounter)
                {
                    Check(tLex.lexComma, " , ");
                }
            }

            Check(tLex.lexQRpar, " ] ");
            Check(tLex.lexSemi, " ; ");
        }

        public static void CheckEuler()
        {
            DScan.NextLex();
            if (DScan.Lex == tLex.lexSemi)
                return;
            else if (DScan.Lex == tLex.lexComma)
            {
                DScan.NextLex();
                Check(tLex.lexOde45, " ode45(метод Рунге-Кутта) ");
                Check(tLex.lexSemi, " ';' ");
                return;
            }
            else
                DError.Expected(" ',' или ';' ");
        }

        public static void CheckOde45()
        {
            DScan.NextLex();
            if (DScan.Lex == tLex.lexSemi)
                return;
            else if (DScan.Lex == tLex.lexComma)
            {
                DScan.NextLex();
                Check(tLex.lexEuler, " ode45(метод Рунге-Кутта) ");
                Check(tLex.lexSemi, " ';' ");
                return;
            }
            else
                DError.Expected(" ',' или ';' ");
        }

        public static void Expression()
        {
            int ParCounter = 0;

            while (DScan.Lex == tLex.lexNum || DScan.Lex == tLex.lexInt || DScan.Lex == tLex.lexName ||
                  DScan.Lex == tLex.lexOper || DScan.Lex == tLex.lexLpar || DScan.Lex == tLex.lexRpar)
            {
                if (DScan.Lex == tLex.lexNum || DScan.Lex == tLex.lexInt)
                {
                    Number();
                    continue;
                }

                if (DScan.Lex == tLex.lexName)
                {
                    sCurName = DScan.Name;
                    Name();
                    continue;
                }

                if (DScan.Lex == tLex.lexOper)
                {
                    Oper();
                    continue;
                }

                if (DScan.Lex == tLex.lexLpar)
                {
                    ParCounter++;
                    DScan.NextLex();
                }

                if (DScan.Lex == tLex.lexRpar)
                {
                    if (ParCounter == 0)
                    {
                        DError.Errors(" лишняя ) ");
                        return;
                    }
                    else
                    {
                        ParCounter--;
                        DScan.NextLex();
                    }
                }
            }
            
            if (ParCounter != 0)
            {
                DError.Expected(" ) ");
            }

            if(DScan.Lex != tLex.lexSemi)
                DError.Expected(" ; ");
        }

        public static void Number()
        {
            DScan.NextLex();
            if (DScan.Lex != tLex.lexOper && DScan.Lex != tLex.lexRpar && DScan.Lex != tLex.lexSemi)
                DError.Expected(" математическая операция(+, -, *, /,), правая скобка или ';' ");
        }

        public static void Name()
        {
            if (sCurName == "x")
            {
                Variable();
            }
            else
            {
                Koef();
            }
            
        }

        public static void Variable()
        {
            DScan.NextLex();
            if (DScan.Lex != tLex.lexOper && DScan.Lex != tLex.lexPow && DScan.Lex != tLex.lexLpar)
                DError.Expected(" математическая операция(+, -, *, /,) или левая скобка ");
            else if (DScan.Lex == tLex.lexLpar)
            {
                //проверяем индекс
                DScan.NextLex();
                if (DScan.Lex != tLex.lexInt)
                {
                    DError.Expected(" целое число ");
                    return;
                }
                //проверям закрывающую скобку
                DScan.NextLex();
                if (DScan.Lex != tLex.lexRpar)
                {
                    DError.Expected(" ) ");
                    return;
                }

                DScan.NextLex();
                if (DScan.Lex != tLex.lexOper && DScan.Lex != tLex.lexRpar && DScan.Lex != tLex.lexSemi)
                    DError.Expected(" математическая операция(+, -, *, /,), правая скобка или ;");
            }

        }

        public static void Koef()
        {
            if (sCurName.Length > 1)
            {
                DError.Errors(" коэффициент должен состоять из одного символва! ");
                DScan.NextLex();
            }
            else
            {
                if(!Koefs.ContainsKey(sCurName))
                    Koefs.Add(sCurName, "");
                DScan.NextLex();
                if (DScan.Lex != tLex.lexOper && DScan.Lex != tLex.lexRpar && DScan.Lex != tLex.lexSemi)
                    DError.Expected(" математическая операция(+, -, *, /,), правая скобка или ;");
            }
        }

        public static void Oper()
        {
            DScan.NextLex();
            if (DScan.Lex != tLex.lexName && DScan.Lex != tLex.lexLpar &&
                DScan.Lex != tLex.lexNum && DScan.Lex != tLex.lexInt)
                DError.Expected(" переменная, коэффициент, число или левая скобка) ");
        }
       
    }
}
