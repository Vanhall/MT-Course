using System;

namespace LR1
{
    class Program
    {
        static void Main(string[] args)
        {
            /*                 Тестовая программа                 */

            // Постоянные таблицы ---------------------------------------------
            ConstTable KeyWords = new ConstTable(@"Tables/KeyWords.txt");   // Создание из файла
            ConstTable Separators = new ConstTable(new string[] { " ", "{", "}", ";", "(", ")" });  // Создание из массива
            ConstTable Operators = new ConstTable(@"Tables/Operators.txt");
            Console.WriteLine("### Key Words Table ###");   // Распечатка таблиц
            Console.WriteLine(KeyWords);                    //
            Console.WriteLine("\n### Separators Table ###");//
            Console.WriteLine(Separators);                  //
            Console.WriteLine("\n### Operators Table ###"); //
            Console.WriteLine(Operators);                   //

            // Методы
            Console.WriteLine(KeyWords.Contains("main") + "| expected: True");
            Console.WriteLine(KeyWords.Contains("for") + "| expected: False");
            Console.WriteLine(KeyWords.Find("while") + "| expected: 2");
            Console.WriteLine(KeyWords.GetToken(2) + "| expected: (0,2)");
            Console.WriteLine(KeyWords.GetToken("int") + "| expected: (0,1)");
            Console.WriteLine();

            // Переменные таблицы ---------------------------------------------
            Console.WriteLine("\n### Identifiers Table ###");
            VarTable Identifiers = new VarTable();                              // Создание пустой

            // Методы
            Console.WriteLine(Identifiers.Add("a") + "| expected: True");       // Добавим "а"
            Console.WriteLine(Identifiers.Add("a") + "| expected: False");      // Добавим "а" (уже есть) 
            Console.WriteLine(Identifiers.Add("b") + "| expected: True");       // Добавим "b"
            Console.WriteLine(Identifiers.Find("b") + "| expected: 1");         // Поиск (значение) 
            Console.WriteLine(Identifiers.Find(1) + "| expected: \"b\"");       // Поиск (индекс)
            Console.WriteLine(Identifiers.Contains("b") + "| expected: True");  // Есть ли в таблице?
            Console.WriteLine(Identifiers.Contains("c") + "| expected: False"); //

            // Атрибуты
            Attribs A;
            Identifiers.GetAttribs("a", out A); // Получаем
            Console.WriteLine(A);

            A.Init = true; A.IdType = 1; A.Val = -100500;
            Identifiers.SetAttribs("a", A);     // Задаем
            Identifiers.GetAttribs("a", out A);
            Console.WriteLine(A);
            
            Console.WriteLine(Identifiers);     // Распечатка таблицы

            Console.ReadKey();
        }
    }
}
