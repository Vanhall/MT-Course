using LR1;
using System;

namespace LR2
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
            LexicAnalyzer La = new LexicAnalyzer(KeyWords, Delimiters, Operations, Constants, Identifiers);
            Token[] res = La.AnalyzeSource("test2");
            foreach(string s in La.Output)
                Console.WriteLine(s);
            Console.WriteLine(Constants);
            Console.WriteLine(Identifiers);
        }
    }
}
