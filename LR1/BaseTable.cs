using System.Collections.Generic;
using System.Text;

namespace LR1
{
    public abstract class BaseTable<T, U> : TableID
    {
        protected List<T> indexer;
        protected Dictionary<T, U> table;

        public BaseTable() : base()
        {
            indexer = new List<T>();
            table = new Dictionary<T, U>();
        }

        public Token GetToken(int Index)
        {
            if (Index >= 0 && Index < indexer.Count) return new Token(ID, Index);
            else return null;
        }

        public Token GetToken(T Lexem)
        {
            if (table.ContainsKey(Lexem))
                return new Token(ID, indexer.IndexOf(Lexem));
            else return null;
        }
        
        public bool Contains(T Lexem) => table.ContainsKey(Lexem);
        public bool Contains(int Index) => (Index >= 0 && Index < indexer.Count) ? true : false;

        public T Find(int Index)
        {
            if (Index >= 0 && Index < indexer.Count) return indexer[Index];
            else return default(T);
        }

        public int Find(T Lexem) => indexer.IndexOf(Lexem);
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("index\t| lexem\t| token\n--------+-------+--------\n");
            for (int i = 0; i < indexer.Count; i++)
                sb.AppendFormat("{0}\t| {1}\t| {2}\n", i, indexer[i], GetToken(i));
            return sb.ToString();
        }
    }
}
