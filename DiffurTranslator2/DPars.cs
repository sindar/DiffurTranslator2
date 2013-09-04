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
        public static ArrayList Methods = new ArrayList();
        
        static string min;
        static string step;
        static string max;
        static string RKF5Tol;
        
        static int debug_conuter;

        public static Hashtable Koefs = new Hashtable();

        //Главная функция компиляции, запускающая все остальные
        public static void Compile(ref RichTextBox Lexems, ref RichTextBox Code)
        {
            Lexems.Clear();

            DError.ErrorCounter = 0;

            Check(tLex.lexBegin, " 'Начало' ");
            Check(tLex.lexName, " имя программы ");
            Check(tLex.lexSemi, " ; ");
            Check(tLex.lexGiven, " 'Уравнения' ");
            DeclEq();
            Check(tLex.lexKoef, " 'Коэффициенты' ");
            DeclKoef();
            Check(tLex.lexCauchy, " 'Условия' ");
            DeclCauchy();
            Check(tLex.lexMethod, " 'Метод' ");
            DeclMethod();
            Check(tLex.lexGet, " 'Вывести' ");
            DeclPlot();
            Check(tLex.lexEnd, " 'Конец' ");

            
            if (DError.ErrorCounter == 0)
            {
                GenFunSys(ref Lexems, ref Code);
                
                MLApp.MLApp MatLabApp = new MLApp.MLApp();
                
                MatLabApp.Execute("cd " + MainForm.StartPath + "\\result") ;
                MatLabApp.Visible = 1;

                if (Methods.Contains("euler"))
                {
                    GenEuler(ref Lexems, ref Code);
                    MatLabApp.Execute("t_euler");
                }

                if (Methods.Contains("heun"))
                {
                    GenHeun(ref Lexems, ref Code);
                    MatLabApp.Execute("t_heun");
                }

                if (Methods.Contains("RK2"))
                {
                    GenRK2(ref Lexems, ref Code);
                    MatLabApp.Execute("t_RK2");
                }

                if (Methods.Contains("RK4"))
                {
                    GenRK4(ref Lexems, ref Code);
                    MatLabApp.Execute("t_RK4");
                }

                if (Methods.Contains("RKF5"))
                {
                    GenRKF5(ref Lexems, ref Code);
                    MatLabApp.Execute("t_RKF5");
                }

                WaitForm waitwin = new WaitForm();
                waitwin.Show();
                
                MatLabApp.Quit();

            }

            DText.CloseText();
        }
          
        //Проверка ожидаемой лексемы
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

        //Проверка лексемы "конец текста"
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

        //Объявление уравнений
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
                    DScan.Lex != tLex.lexOper && DScan.Lex != tLex.lexLpar && DScan.Lex != tLex.lexFun)
                    DError.Expected(" число, идентификатор, математическая операция(+, -, *, /,) или левая скобка ");

                Expression(0);
                DScan.NextLex();
            }

            if (EqCounter == 0)
                DError.Expected(" dxdt ");       
        }

        //Объявление коэффициентов
        public static void DeclKoef()
        {
            KoefCounter = 0;
            string sTemp = "";

            while (DScan.Lex == tLex.lexName && KoefCounter <= Koefs.Count)
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

                sTemp = "";

                if (CheckMinus())
                    sTemp += "-";
                
                Check(tLex.lexNum, " число ");
                sTemp += dCurNum.ToString().Replace(',', '.');
                
                Koefs[sCurName] = sTemp;
                Check(tLex.lexSemi, " ';' ");
                
            }

            if (KoefCounter == 0)
            {
                DError.Expected(" коэффициент ");
            }

            if (KoefCounter > Koefs.Count)
            {
                DError.Errors(" коэффициентов не может быть больше чем в уравнениях " );
            }
        }

        //Объявление начальный условий задачи Коши
        public static void DeclCauchy()
        {
            string sTemp = "";
            
            Check(tLex.lexTspan, " tspan(промежуток интегрирования ");
            Check(tLex.lexAss, " = ");
            Check(tLex.lexQLpar, " [ ");

            sTemp = "";
            if (CheckMinus())
                sTemp += "-";

            Check(tLex.lexNum, " число ");
            sTemp += dCurNum.ToString().Replace(',', '.');
            min = sTemp;

            Check(tLex.lexComma, " ',' ");

            sTemp = "";
            if (CheckMinus())
                sTemp += "-";
                        
            Check(tLex.lexNum, " число ");
            sTemp += dCurNum.ToString().Replace(',', '.');
            max = sTemp;

            Check(tLex.lexQRpar, " ] ");
            Check(tLex.lexSemi, " ; ");
            
            Check(tLex.lexStep, " step(шаг интегрирования) ");
            Check(tLex.lexAss, " = ");

            sTemp = "";
            if (CheckMinus())
                sTemp += "-";

            Check(tLex.lexNum, " число ");
            sTemp += dCurNum.ToString().Replace(',', '.');
            step = sTemp;
            Check(tLex.lexSemi, " ; ");
            
            Check(tLex.lexX0, " x0(начальные условия) ");
            Check(tLex.lexAss, " = ");
            Check(tLex.lexQLpar, " [ ");

            for (int i = 1; i <= EqCounter; i++)
            {
                sTemp = "";
                if (CheckMinus())
                    sTemp += "-";
                
                Check(tLex.lexNum, " число ");
                if(i < EqCounter)
                    Check(tLex.lexSemi, " ; ");
            }

            Check(tLex.lexQRpar, " ] ");
            Check(tLex.lexSemi, " ; ");

        }

        //Объявление численных методов интегрирования
        public static void DeclMethod()
        {
            string sTemp = "";
            Methods.Clear();

            int i;

            for (i = 1; i < 6; i++)
            {
                if (DScan.Lex != tLex.lexEuler && DScan.Lex != tLex.lexHeun
                  && DScan.Lex != tLex.lexRK2 && DScan.Lex != tLex.lexRK4 && DScan.Lex != tLex.lexRKF5
                  && DScan.Lex != tLex.lexSemi)
                {
                    DError.Expected(" численный метод - euler(Эйлера), heun(Гюна), RK2(Рунге-Кутта 2-го порядка, RK4(Рунге-Кутта 4-го порядка) или ';' ");
                    break;
                }
                else if (DScan.Lex == tLex.lexSemi)
                    break;
                else
                {
                    if (CheckMethod(tLex.lexEuler, "euler"))
                    {
                        if ((i < 5) && (DScan.Lex != tLex.lexComma && DScan.Lex != tLex.lexSemi))
                        {
                            DError.Expected(" ',' или ';' ");
                            break;
                        }

                        if (i < 5 && DScan.Lex != tLex.lexSemi)
                            DScan.NextLex();

                        continue;
                    }

                    if (CheckMethod(tLex.lexHeun, "heun"))
                    {
                        if ((i < 5) && (DScan.Lex != tLex.lexComma && DScan.Lex != tLex.lexSemi))
                        {
                            DError.Expected(" ',' или ';' ");
                            break;
                        }

                        if (i < 5 && DScan.Lex != tLex.lexSemi)
                            DScan.NextLex();

                        continue;
                    }

                    if (CheckMethod(tLex.lexRK2, "RK2"))
                    {
                        if ((i < 5) && (DScan.Lex != tLex.lexComma && DScan.Lex != tLex.lexSemi))
                        {
                            DError.Expected(" ',' или ';' ");
                            break;
                        }

                        if (i < 5 && DScan.Lex != tLex.lexSemi)
                            DScan.NextLex();

                        continue;
                    }

                    if (CheckMethod(tLex.lexRK4, "RK4"))
                    {
                        if ((i < 5) && (DScan.Lex != tLex.lexComma && DScan.Lex != tLex.lexSemi))
                        {
                            DError.Expected(" ',' или ';' ");
                            break;
                        }

                        if (i < 5 && DScan.Lex != tLex.lexSemi)
                            DScan.NextLex();

                        continue;
                    }

                    if (CheckMethod(tLex.lexRKF5, "RKF5"))
                    {
                        Check(tLex.lexQLpar, " [ ");

                        sTemp = "";
                        Check(tLex.lexNum, " число(точность) ");
                        sTemp += dCurNum.ToString().Replace(',', '.');
                        RKF5Tol = sTemp;
                        Check(tLex.lexQRpar, " ] ");
                        
                        if ((i < 5) && (DScan.Lex != tLex.lexComma && DScan.Lex != tLex.lexSemi))
                        {
                            DError.Expected(" ',' или ';' ");
                            break;
                        }

                        if (i < 5 && DScan.Lex != tLex.lexSemi)
                            DScan.NextLex();

                        continue;
                    }
                }
            }
            
            Check(tLex.lexSemi, " ';' ");

        }
            
        //Добавление численного метода в список и проверка на повторное добавлене 
        public static bool CheckMethod(tLex tMethod, string sMethod)
        {
            if (DScan.Lex == tMethod && !Methods.Contains(sMethod))
            {
                Methods.Add(sMethod);
                DScan.NextLex();
                return true;
            }
            else if (DScan.Lex == tMethod && Methods.Contains(sMethod))
                DError.Errors(" данный метод уже есть в списке ");
            return false;
        }

        //Объявление графиков функций, которые будут выводиться на экран
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

        //Синтаксический разбор выражения
        public static void Expression(int exp_type)//exp_type - тип выражения; 0 - корневое выражение; 1 - выражение внутри скобок мат. функции
        {
            debug_conuter++;
            
            int ParCounter = 0;

            if (exp_type == 1)
            {
                DScan.NextLex();
                Check(tLex.lexLpar, " ( ");//Ожидаем левую скобку, если нет - вываливаемся в ошибку через Check
                ParCounter++;
            }
    
            while (DScan.Lex == tLex.lexNum || DScan.Lex == tLex.lexInt || DScan.Lex == tLex.lexName ||
                  DScan.Lex == tLex.lexOper || DScan.Lex == tLex.lexLpar || DScan.Lex == tLex.lexRpar || DScan.Lex == tLex.lexFun)
            {

                if (DScan.Lex == tLex.lexFun)
                {
                    Expression(1);
                    continue;
                }

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

                        //Пока есть проблемы с вложенными функциями
                        /*if (exp_type == 1 && ParCounter == 1)
                        {
                            Check(tLex.lexRpar, " ) ");
                            DScan.NextLex();
                            return;
                        }*/
                    }
                }
            }
            
            if (ParCounter != 0)
            {
                DError.Expected(" ) ");
            }

            //Если выражение корневое, ожидается ';'
            if (exp_type == 0)
            {
                if (DScan.Lex != tLex.lexSemi)
                    DError.Expected(" ; ");
            }
        }

        //Проверка является ли следующая лексема числом
        public static void Number()
        {
            DScan.NextLex();
            if (DScan.Lex != tLex.lexOper && DScan.Lex != tLex.lexRpar && DScan.Lex != tLex.lexSemi)
                DError.Expected(" математическая операция(+, -, *, /,), правая скобка или ';' ");
        }

        //Проверка идентификаторов
        public static void Name()
        {
            if (sCurName == "x")
                Variable_x();
            else if (sCurName == "t")
                Variable_t();
            else
                Koef();
        }

        //Проверка идентификатора функции от времени x = f(t)
        public static void Variable_x()
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

        //Проверка идентификатора переменной времени - t
        public static void Variable_t()
        {
            DScan.NextLex();
            if (DScan.Lex != tLex.lexOper && DScan.Lex != tLex.lexRpar && DScan.Lex != tLex.lexSemi)
                DError.Expected(" математическая операция(+, -, *, /,), правая скобка или ; ");
        }

        //Проверка коэффициента
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

        //Проверка является ли следующая лексема математической операцией
        public static void Oper()
        {
            DScan.NextLex();
            if (DScan.Lex != tLex.lexName && DScan.Lex != tLex.lexLpar &&
                DScan.Lex != tLex.lexNum && DScan.Lex != tLex.lexInt && DScan.Lex != tLex.lexFun)
                DError.Expected(" переменная, коэффициент, число или левая скобка) ");
        }

        //Проверка минуса(унарный или бинарный)
        public static bool CheckMinus()
        {
            if (DScan.Lex == tLex.lexOper)
            {
                if (DScan.Oper == tOper.Minus)
                {
                    DScan.NextLex();
                    return true;
                }
                else
                    DError.Expected(" положительное или отрицательное число ");
            }
            return false;
        }

        //Генератор matlab-файла, описывающего систему уравнений
        public static void GenFunSys(ref RichTextBox Lexems, ref RichTextBox Code)
        {
            string sTemp = "";
            int j = 0;

            Lexems.Clear();
            Lexems.Text += "function dxdt=t_funsys(t,x);\n";

            //коэффициенты
            ICollection KoefKeys = Koefs.Keys;

            foreach (string koef in KoefKeys)
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

            DFile.SaveFile(MainForm.StartPath + "\\result\\t_funsys.m", ref Lexems);

        }

        //Генератор matlab-файла для решения системы уравнений методом Эйлера 1-го порядка
        public static void GenEuler(ref RichTextBox Lexems, ref RichTextBox Code)
        {
            string sTemp = "";
            int j = 0;
            
            Lexems.Clear();
            Lexems.Text += "function [ts,data]=t_euler()\n";

            j = Code.Text.IndexOf("x0");
            do
            {
                sTemp += Code.Text[j];
                j++;
            } while (!Code.Text[j].Equals(']'));

            Lexems.Text += sTemp + "];\n";
            Lexems.Text += "t0 =" + min + "; dt = " + step.ToString().Replace(',', '.') + "; tn = " + max + ";\n";
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
            
            if(MainForm.bLegendChecked)
                Lexems.Text += "legend('x`1','x`2','x`3')\n";

            Lexems.Text += "print('-dbmp','-r80','graf_eu.bmp')\n";
            Lexems.Text += "end\n";

            DFile.SaveFile(MainForm.StartPath + "\\result\\t_euler.m", ref Lexems);

        }

        //Генератор matlab-файла для решения системы уравнений методом Гюна 2-го порядка
        public static void GenHeun(ref RichTextBox Lexems, ref RichTextBox Code)
        {
            string sTemp = "";
            int j = 0;

            Lexems.Clear();
            Lexems.Text += "function [ts,data]=t_heun()\n";

            j = Code.Text.IndexOf("x0");
            do
            {
                sTemp += Code.Text[j];
                j++;
            } while (!Code.Text[j].Equals(']'));

            Lexems.Text += sTemp + "];\n";
            Lexems.Text += "t0 =" + min + "; dt = " + step.ToString().Replace(',', '.') + "; tn = " + max + ";\n";
            Lexems.Text += "Nsteps = round(tn/dt)\n";
            Lexems.Text += "ts = zeros(Nsteps,1)\n";
            Lexems.Text += "data = zeros(Nsteps,length(x0))\n";
            Lexems.Text += "ts(1) = t0\n";
            Lexems.Text += "data(1,:) = x0'\n";
            Lexems.Text += "t1 = 0'\n";
            Lexems.Text += "for i =1:Nsteps\n";
                        
            Lexems.Text += "dxdt= feval(@t_funsys,t0,x0)\n";
            Lexems.Text += "dxdt1= feval(@t_funsys,t1,x0+dt*dxdt)\n";
            
            Lexems.Text += "x0=x0+(dt/2)*(dxdt+dxdt1);\n";
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
            
            if (MainForm.bLegendChecked)
                Lexems.Text += "legend('x`1','x`2','x`3')\n";
            
            Lexems.Text += "print('-dbmp','-r80','graf_heun.bmp')\n";
            Lexems.Text += "end\n";

            DFile.SaveFile(MainForm.StartPath + "\\result\\t_heun.m", ref Lexems);

        }

        //Генератор matlab-файла для решения системы уравнений методом Рунге-Кутты 2-го порядка
        public static void GenRK2(ref RichTextBox Lexems, ref RichTextBox Code)
        {
            string sTemp = "";
            int j = 0;

            Lexems.Clear();
            Lexems.Text += "function t_RK2()\n";
            Lexems.Text += "tspan=[" + min + ':' + step.ToString().Replace(',', '.') + ':' + max + "]\n";

            j = Code.Text.IndexOf("x0");
            do
            {
                sTemp += Code.Text[j];
                j++;
            } while (!Code.Text[j].Equals(']'));

            Lexems.Text += sTemp + "];\n";
            Lexems.Text += "[t,x]=ode23(@t_funsys,tspan,x0);\n";
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

            if (MainForm.bLegendChecked)
                Lexems.Text += "legend('x`1','x`2','x`3')\n";

            Lexems.Text += "print('-dbmp','-r80','graf_RK2.bmp')\n";

            DFile.SaveFile(MainForm.StartPath + "\\result\\t_RK2.m", ref Lexems);
            
        }

        //Генератор matlab-файла для решения системы уравнений методом Рунге-Кутты 4-го порядка
        public static void GenRK4(ref RichTextBox Lexems, ref RichTextBox Code)
        {
            string sTemp = "";
            int j = 0;

            Lexems.Clear();
            Lexems.Text += "function t_RK4()\n";
            Lexems.Text += "tspan=[" + min + ':' + step.ToString().Replace(',', '.') + ':' + max + "]\n";

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
            
            if (MainForm.bLegendChecked)
                Lexems.Text += "legend('x`1','x`2','x`3')\n";

            Lexems.Text += "print('-dbmp','-r80','graf_RK4.bmp')\n";

            DFile.SaveFile(MainForm.StartPath + "\\result\\t_RK4.m", ref Lexems);
        }

        //Генератор matlab-файла для решения системы уравнений методом Рунге-Кутты-Фехлберга 5-го порядка
        public static void GenRKF5(ref RichTextBox Lexems, ref RichTextBox Code)
        {
            string sTemp = "";
            int j = 0;

            Lexems.Clear();
            Lexems.Text += "function t_RKF5()\n";
            Lexems.Text += "tspan=[" + min + ':' + step.ToString().Replace(',', '.') + ':' + max + "]\n";

            j = Code.Text.IndexOf("x0");
            do
            {
                sTemp += Code.Text[j];
                j++;
            } while (!Code.Text[j].Equals(']'));

            Lexems.Text += sTemp + "];\n";
            Lexems.Text += "[t,x]=ode45(@t_funsys,tspan,x0, " + RKF5Tol + ");\n";
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

            if (MainForm.bLegendChecked)
                Lexems.Text += "legend('x`1','x`2','x`3')\n";

            Lexems.Text += "print('-dbmp','-r80','graf_RKF5.bmp')\n";

            DFile.SaveFile(MainForm.StartPath + "\\result\\t_RKF5.m", ref Lexems);
        }
    }
}
