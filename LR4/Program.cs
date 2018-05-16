using System;
using System.IO;
using LR1;
using LR2;
using LR3;

namespace LR4
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
            var Goto = new ConstTable(@"Tables/Goto.txt");
            var Labels = new VarTable();

            string TestFile = "test3";

            LexicAnalyzer LA = new LexicAnalyzer(KeyWords, Delimiters, Operations, Constants, Identifiers);
            Token[] LexicAnalysisResult = LA.AnalyzeSource(TestFile);

            SyntacticAnalyzer SA = new SyntacticAnalyzer(KeyWords, Delimiters, Operations, Constants, Identifiers, Goto, Labels);
            try
            {
                SA.AnalyzeTokens(LexicAnalysisResult);
                Console.WriteLine("Синтаксическтй анализ успешно завершен.");
                
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Ошибка: " + Ex.Message);
                File.WriteAllText(Directory.GetCurrentDirectory() + @"\" + TestFile + ".syn.out.txt", "Ошибка: " + Ex.Message);
            }

            CodeBuilder CB = new CodeBuilder(KeyWords, Operations, Constants, Identifiers, Goto, Labels);
            CB.BuildCode(SA.Output);
            File.WriteAllText(Directory.GetCurrentDirectory() + @"\" + TestFile + ".asm", CB.ToString());
            Console.WriteLine($"Построение кода успешно завершено. Выходной файл: {TestFile}.asm");
        }
    }
}
