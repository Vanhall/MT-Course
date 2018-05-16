using System;

namespace LR1
{
    /// <summary>
    /// Класс, реализующий токен
    /// </summary>
    public class Token : IEquatable<Token>
    {
        /// <summary>
        /// Идентификатор таблицы
        /// </summary>
        public int TableID { get; set; }

        /// <summary>
        /// Идекс в таблице
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="TableID">Идентификатор таблицы</param>
        /// <param name="Index">Идекс в таблице</param>
        public Token(int TableID, int Index)
        {
            this.TableID = TableID;
            this.Index = Index;
        }

        /// <summary>
        /// Переопределение стандартного метода ToString
        /// </summary>
        /// <returns>Строковое представление объекта</returns>
        public override string ToString()
        {
            return "(" + TableID + "," + Index + ")";
        }
        
        public bool Equals(Token other)
        {
            return (TableID == other.TableID && Index == other.Index);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Token;
            if (other == null) return false;

            return Equals(other);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
