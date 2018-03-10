using System;
using System.Collections.Generic;
using System.IO;

namespace LR2
{
    using States = FiniteStateMachine.States;
    class Program
    {
        static void Main(string[] args)
        {
            FiniteStateMachine FSM = new FiniteStateMachine();
            string testFile = "test2";                                          // Имя входного файла
            string[] source = File.ReadAllLines(@"Tests/" + testFile + ".cpp"); // Читаем исходник
            var output = new List<string>();                                    // Результат

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
                        File.WriteAllText(Directory.GetCurrentDirectory() + @"\" + testFile + ".out.txt", errMsg);
                        Console.ReadKey();
                        return;
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
                        if (FSM.CurrentToken != null) tokens += FSM.CurrentToken;
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
                if (tokens.Length != 0) output.Add(tokens);
            }

            // Проверяем наличие незакрытого блочного комментария
            if (FSM.State == States.BlockComm || FSM.State == States.BlockCommEnd)
            {
                string errMsg = "Ошибка в строке " + lineNumber + ": незакрытый блочный комментарий";
                Console.WriteLine(errMsg);
                File.WriteAllText(Directory.GetCurrentDirectory() + @"\" + testFile + ".out.txt", errMsg);
                Console.ReadKey();
            }
            else
            {
                File.WriteAllLines(Directory.GetCurrentDirectory() + @"\" + testFile + ".out.txt", output);
                File.WriteAllText(Directory.GetCurrentDirectory() + @"\" + testFile + ".out.tables.txt", FSM.PrintTables());
                Console.WriteLine("Лексический анализ успешно завершен.");
                Console.ReadKey();
            }
        }
    }
}
