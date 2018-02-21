using System.IO;

namespace LR1
{
    /// <summary>
    /// Постоянная таблица
    /// </summary>
    public class ConstTable : BaseTable<string, int>
    {
        /// <summary>
        /// Конструктор (массив)
        /// </summary>
        /// <param name="Lexems">массив лексем</param>
        public ConstTable(string[] Lexems) : base()
        {
            int i = 0;
            foreach (string lexeme in Lexems)
            {
                if (!table.ContainsKey(lexeme))
                {
                    table.Add(lexeme, i++);
                    indexer.Add(lexeme);
                }
            }
        }

        /// <summary>
        /// Конструктор (файл)
        /// </summary>
        /// <param name="FilePath">Путь к файлу</param>
        public ConstTable(string FilePath) : base()
        {
            int i = 0;
            string[] lexems = File.ReadAllLines(FilePath);
            foreach (string lexeme in lexems)
            {
                if (!table.ContainsKey(lexeme))
                {
                    table.Add(lexeme, i++);
                    indexer.Add(lexeme);
                }
            }
        }
    }
}
