using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR1;
using LR2;

namespace LR3
{
    class Program
    {
        static void Main(string[] args)
        {
            var KeyWords = new ConstTable(@"Tables/KeyWords.txt");
            var Delimiters = new ConstTable(@"Tables/Delimiters.txt");
            var Operations = new ConstTable(@"Tables/Operations.txt");
            var Constants = new VarTable();
            var Identifiers = new VarTable();

            LexicAnalyzer LA = new LexicAnalyzer(KeyWords, Delimiters, Operations, Constants, Identifiers);
            Token[] Tokens = LA.AnalyzeSource("test2");

            SyntacticAnalyzer SA = new SyntacticAnalyzer(KeyWords, Delimiters, Operations, Constants, Identifiers);
            SA.AnalyzeTokens(Tokens);
            Console.WriteLine("Synt OK");
            //try
            //{
            //    SA.AnalyzeTokens(Tokens);
            //    Console.WriteLine("Synt OK");
            //}
            //catch(Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
        }
    }
}
