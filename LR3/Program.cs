using System;
using System.Collections.Generic;
using System.IO;
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
            var Goto = new ConstTable(@"Tables/Goto.txt");
            var Labels = new VarTable();

            string TestFile = "test1";

            LexicAnalyzer LA = new LexicAnalyzer(KeyWords, Delimiters, Operations, Constants, Identifiers);
            Token[] LexicAnalysisResult = LA.AnalyzeSource(TestFile);

            SyntacticAnalyzer SA = new SyntacticAnalyzer(KeyWords, Delimiters, Operations, Constants, Identifiers, Goto, Labels);
            try
            {
                SA.AnalyzeTokens(LexicAnalysisResult);
                Console.WriteLine("Синтаксическтй анализ успешно завершен.");

                var Tokens = new List<string>();
                var Text = new List<string>();
                foreach (Token[] Line in SA.Output)
                {
                    string buffer = "";
                    string textBuffer = "";
                    foreach (Token T in Line)
                    {
                        buffer += T;
                        if (T.TableID == KeyWords.ID) textBuffer += KeyWords.Find(T.Index) + " ";
                        if (T.TableID == Operations.ID) textBuffer += Operations.Find(T.Index) + " ";
                        if (T.TableID == Constants.ID) textBuffer += Constants.Find(T.Index) + " ";
                        if (T.TableID == Identifiers.ID) textBuffer += Identifiers.Find(T.Index) + " ";
                        if (T.TableID == Goto.ID) textBuffer += Goto.Find(T.Index) + " ";
                        if (T.TableID == Labels.ID) textBuffer += Labels.Find(T.Index) + " ";
                    }
                    Tokens.Add(buffer);
                    Text.Add(textBuffer);
                }

                File.WriteAllLines(Directory.GetCurrentDirectory() + @"\" + TestFile + ".syn.out.txt", Tokens);
                File.WriteAllLines(Directory.GetCurrentDirectory() + @"\" + TestFile + ".syn.text.out.txt", Text);
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Ошибка: " + Ex.Message);
                File.WriteAllText(Directory.GetCurrentDirectory() + @"\" + TestFile + ".syn.out.txt", "Ошибка: " + Ex.Message);
            }
        }
    }
}
