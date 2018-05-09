using System;
using System.Collections.Generic;
using System.IO;
using LR1;

namespace LR2
{
    using States = FiniteStateMachine.States;
    public class LexicAnalyzer
    {
        FiniteStateMachine FSM;
        string testFile;
        string[] source;
        public List<string> Output;

        public LexicAnalyzer(
            ConstTable KeyWords,
            ConstTable Delimiters,
            ConstTable Operations,
            VarTable Constants,
            VarTable Identifiers)
        {
            FSM = new FiniteStateMachine(KeyWords, Delimiters, Operations, Constants, Identifiers);
        }
        
        public Token[] AnalyzeSource(string TestFile)
        {
            testFile = TestFile;                                            // Имя входного файла
            source = File.ReadAllLines(@"Tests/" + TestFile + ".cpp");      // Читаем исходник
            Output = new List<string>();                                    // Результат

            List<Token> result = new List<Token>();
            int lineNumber = 0;                 // Счетчик строк
            foreach (string line in source)     // Пробегаем по всем строкам исходника
            {
                lineNumber++;
                string tokens = "";             // Токены текущей строки исходника

                for (int i = 0; i < line.Length; i++)   // Пробегаем строку посимвольно
                {
                    // Пробуем распознать очередной символ
                    try
                    {
                        FSM.ProcessSymbol(line[i]);
                    }
                    catch (Exception Ex)
                    {
                        string errMsg = "Ошибка в строке " + lineNumber + ": " + Ex.Message;
                        Console.WriteLine(errMsg);
                        File.WriteAllText(Directory.GetCurrentDirectory() + @"\" + testFile + ".lex.out.txt", errMsg);
                        //Console.ReadKey();
                        return null;
                    }

                    // Если обнаружен строчный комментарий - пропускаем остаток строки
                    if (FSM.State == States.LineComm)
                    {
                        FSM.State = States.Start;
                        break;
                    }

                    // Если достигнуто конечное состояние - добавляем токен к текущей выходной строке
                    if (FSM.State == States.Final)
                    {
                        if (FSM.CurrentToken != null)
                        {
                            tokens += FSM.CurrentToken;
                            result.Add(FSM.CurrentToken);
                        }
                        FSM.State = States.Start;
                    }

                    // Проверяем нужно ли откатиться на один символ назад
                    if (FSM.StepBack)
                    {
                        i--;
                        FSM.StepBack = false;
                    }
                }

                // Если текущая строка токенов не пуста, добавляем её к результату
                if (tokens.Length != 0) Output.Add(tokens);
            }

            // Проверяем наличие незакрытого блочного комментария
            if (FSM.State == States.BlockComm || FSM.State == States.BlockCommEnd)
            {
                string errMsg = "Ошибка в строке " + lineNumber + ": незакрытый блочный комментарий";
                Console.WriteLine(errMsg);
                File.WriteAllText(Directory.GetCurrentDirectory() + @"\" + testFile + ".lex.out.txt", errMsg);
                //Console.ReadKey();
            }
            else
            {
                //File.WriteAllLines(Directory.GetCurrentDirectory() + @"\" + testFile + ".lex.out.txt", Output);
                //File.WriteAllText(Directory.GetCurrentDirectory() + @"\" + testFile + ".lex.out.tables.txt", FSM.PrintTables());
                Console.WriteLine("Лексический анализ успешно завершен.");
                //Console.ReadKey();
            }
            return result.ToArray();
        }
    }
}
