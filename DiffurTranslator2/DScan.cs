using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace DiffurTranslator2
{
    public enum tLex {lexEot, lexBegin, lexGiven, lexKoef, lexCauchy, lexMethod, lexGet, lexEnd, lexdxdt,
                      lexName, lexNum, lexPow, lexMult, lexPlus, lexMinus, lexDel, lexRpar, lexLpar, lexQRpar, lexQLpar,
                      lexColon, lexSemi, lexComma, lexDot, lexAss};

    struct KWHash
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

        public const int KWNum = 7;
        /*
        typeof 
           tKeyWord=string[9];
         */
        
        private static int nkw;//номер ключевого слова

        public static KWHash[] KWTable = new KWHash[KWNum];

        private static void EnterKW(string nName, tLex nLex)
        {
            KWTable[nkw].Word = nName;
            KWTable[nkw].Lex = nLex;
            nkw++;
        }

       /*  function TestKW:tLex;
           var
            i:integer;
            begin
            i:=nkw;
            while (i>0) and (Name<>KWTable[i].Word) do
            i:=i-1;
            if i>0 then
             TestKW:=KWTable[i].Lex
             else
             TestKW:=lexName;
             end;*/

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

        
        /*procedure Ident;
           var
            i:integer;
            begin
            i:=0;
            repeat
             if i<NameLen then
             begin
              i:=i+1;
              Name[i]:=Ch;
              end
              else
              Errors(' ñëèøêîì äëèííîå èìÿ ');
              NextCh;
             until not (Ch in ['A'..'Z','a'..'z','0'..'9']);
             Name[0]:=chr(i);
             Lex:=TestKW;
             end;*/

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
                     (DText.Ch >= '0' && DText.Ch <= '9'));

            //Name[0]:=chr(i);
            Lex = TestKW();
        }

        
        /* procedure Number;
         var
          d:integer;
          begin
          Lex:=lexNum;
          Num:=0;
          repeat
          d:=ord(Ch)-ord('0');
          if(Maxint-d) div 10>=Num then
          Num:=10*Num+d
          else
          Errors(' ñëèøêîì áîëüøîå ÷èñëî ');
          NextCh;

          until not (Ch in ['0'..'9']);
          end;*/

        public static void Number()
        {
            int dotCounter = 0;
            Lex = tLex.lexNum;
            string sNum = "";
            Num = 0;
            do
            {
                sNum += (int)(DText.Ch);
                DText.NextCh();

                if (DText.Ch == '.' && dotCounter == 0)
                {
                    sNum += (int)(DText.Ch);
                    DText.NextCh();
                    dotCounter = 1;
                }

            }while(DText.Ch >= '0' && DText.Ch <= '9');
            
            Num = Convert.ToDouble(sNum);
        }

        /*  procedure NextLex;
            begin
       while Ch in [chSpace,chTab,chEol] do NextCh;
       LexPos:=Pos;
        case Ch of
        'A'..'Z','a'..'z':
        Ident;
        '0'..'9':
        Number;
        ':':
        begin
         NextCh;
         Lex:=lexColon;
        end;

        '=':
         begin
         NextCh;
         Lex:=lexAss;
         end;

         '(':
          begin
         NextCh;
         Lex:=lexLpar;
        end;

        ')':
          begin
         NextCh;
         Lex:=lexRpar;
        end;

        '+':
          begin
         NextCh;
         Lex:=lexPlus;
        end;

        '-':
          begin
         NextCh;
         Lex:=lexMinus;
        end;

        '*':
          begin
         NextCh;
         Lex:=lexMult;
        end;

        '/':
          begin
         NextCh;
         Lex:=lexDel;
        end;
        '^':
        begin
        NextCh;
        Lex:=lexPow;
        end;

        chEot:
          Lex:=lexEot;
  
    
        else
         Errors(' íåäîïóñòèìûé ñèìâîë ');
         end;
         end;*/
        
        
        public static void NextLex()
        {

            while (DText.Ch == DText.chSpace || DText.Ch == DText.chTab || DText.Ch == DText.chEol || DText.Ch == DText.chRet)
                DText.NextCh();
            
            LexPos = DText.Pos;

            if ((DText.Ch >= 'A' && DText.Ch <= 'Z') || (DText.Ch >= 'a' && DText.Ch <= 'z'))
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
                    Lex = tLex.lexPlus;
                    break;
                case '-':
                    DText.NextCh();
                    Lex = tLex.lexMinus;
                    break;
                case '*':
                    DText.NextCh();
                    Lex = tLex.lexMult;
                    break;
                case '/':
                    DText.NextCh();
                    Lex = tLex.lexDel;
                    break;
                case '^':
                    DText.NextCh();
                    Lex = tLex.lexPow;
                    break;
                case ';':
                    DText.NextCh();
                    Lex = tLex.lexSemi;
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
            nkw = 0;
            EnterKW("begin", tLex.lexBegin);
            EnterKW("given", tLex.lexGiven);
            EnterKW("koef", tLex.lexKoef);
            EnterKW("cauchy", tLex.lexCauchy);
            EnterKW("method", tLex.lexMethod);
            EnterKW("get", tLex.lexGet);
            EnterKW("end", tLex.lexEnd);
            NextLex();
        }

    }
}
