using System.IO;

namespace LR1
{
    public class ConstTable : BaseTable<string, int>
    {
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
