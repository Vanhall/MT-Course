using System.Collections.Generic;
using System.Text;

namespace LR1
{
    /// <summary>
    /// Базовый шаблон для таблиц
    /// </summary>
    /// <typeparam name="T">Ключ таблицы</typeparam>
    /// <typeparam name="U">Значение таблицы</typeparam>
    public abstract class BaseTable<T, U> : TableID
    {
        /// <summary>
        /// "Индексатор" таблицы. Для легкого доступа к эл-там по индексу
        /// </summary>
        protected List<T> indexer;

        /// <summary>
        /// Таблица
        /// </summary>
        protected Dictionary<T, U> table;

        /// <summary>
        /// Конструктор
        /// </summary>
        public BaseTable() : base()
        {
            indexer = new List<T>();
            table = new Dictionary<T, U>();
        }

        /// <summary>
        /// Количество элементов в таблице
        /// </summary>
        /// <returns>Количество элементов в таблице</returns>
        public int Count() => indexer.Count;

        /// <summary>
        /// Get-функция токена (по индексу)
        /// </summary>
        /// <param name="Index">Индекс искомого токена</param>
        /// <returns>Токен или null если указаный индекс не существует в таблице</returns>
        public Token GetToken(int Index)
        {
            if (Index >= 0 && Index < indexer.Count) return new Token(ID, Index);
            else return null;
        }

        /// <summary>
        /// Get-функция токена (по имени)
        /// </summary>
        /// <param name="Lexem">Лексема, для которой нужно создать токен</param>
        /// <returns>Токен или null если указаная лексема не существует в таблице</returns>
        public Token GetToken(T Lexem)
        {
            if (table.ContainsKey(Lexem))
                return new Token(ID, indexer.IndexOf(Lexem));
            else return null;
        }

        public Token this[int Index]
        {
            get
            {
                if (Index >= 0 && Index < indexer.Count) return new Token(ID, Index);
                else return null;
            }
        }

        public Token this[T Lexem]
        {
            get
            {
                if (table.ContainsKey(Lexem))
                    return new Token(ID, indexer.IndexOf(Lexem));
                else return null;
            }
        }

        /// <summary>
        /// Содержит ли таблица лексему
        /// </summary>
        /// <param name="Lexem">Искомая лексема</param>
        /// <returns>True если лексема есть в таблице, иначе False</returns>
        public bool Contains(T Lexem) => table.ContainsKey(Lexem);

        /// <summary>
        /// Содержит ли таблица индекс
        /// </summary>
        /// <param name="Index">Искомый индекс</param>
        /// <returns>True если индекс есть в таблице, иначе False</returns>
        public bool Contains(int Index) => (Index >= 0 && Index < indexer.Count);

        /// <summary>
        /// Поиск элемента по индексу
        /// </summary>
        /// <param name="Index">Индекс искомого элемента</param>
        /// <returns>Элемент, если он есть, иначе значение по умолчанию (null)</returns>
        public T Find(int Index)
        {
            if (Index >= 0 && Index < indexer.Count) return indexer[Index];
            else return default(T);
        }

        /// <summary>
        /// Поиск индекса по элементу
        /// </summary>
        /// <param name="Lexem">Лексема, для которой ищется индекс</param>
        /// <returns>Индекс, если лексема есть в таблице, иначе -1</returns>
        public int Find(T Lexem) => indexer.IndexOf(Lexem);

        /// <summary>
        /// Переопределение стандартного метода ToString
        /// </summary>
        /// <returns>Строковое представление объекта</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("index\t| lexem\t| token\n--------+-------+--------\n");
            for (int i = 0; i < indexer.Count; i++)
                sb.AppendFormat("{0}\t| {1}\t| {2}\n", i, indexer[i], GetToken(i));
            return sb.ToString();
        }
        
        public Token[] ToArray()
        {
            var Tokens = new List<Token>();
            for (int i = 0; i < indexer.Count; i++) Tokens.Add(this[i]);
            return Tokens.ToArray();
        }
    }
}
