namespace LR1
{
    /// <summary>
    /// Глобальный идентификатор таблиц
    /// </summary>
    public abstract class TableID
    {
        static int GlobalID = 0;

        /// <summary>
        /// Идентификатор таблицы
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public TableID()
        {
            ID = GlobalID++;
        }
    }
}
