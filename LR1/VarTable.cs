namespace LR1
{
    /// <summary>
    /// Переменная таблица
    /// </summary>
    public class VarTable : BaseTable<string, Attribs>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public VarTable() : base() { }

        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="Id">Добавляемый элемент</param>
        /// <returns>True если операция успешна, иначе False</returns>
        public bool Add(string Id)
        {
            if (!table.ContainsKey(Id))
            {
                table.Add(Id, new Attribs());
                indexer.Add(Id);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Get-функция для атрибутов идентификатора
        /// </summary>
        /// <param name="Id">Имя идентификатора</param>
        /// <param name="Attributes">Возвращаемая структура атрибутов</param>
        /// <returns>True если операция успешна, иначе False</returns>
        public bool GetAttribs(string Id, out Attribs Attributes)
        {
            if (table.ContainsKey(Id))
            {
                Attributes = table[Id];
                return true;
            }
            else
            {
                Attributes = new Attribs();
                return false;
            }
        }

        /// <summary>
        /// Set-функция для атрибутов идентификатора
        /// </summary>
        /// <param name="Id">Имя идентификатора</param>
        /// <param name="Attributes">Cтруктура атрибутов</param>
        /// <returns>True если операция успешна, иначе False</returns>
        public bool SetAttribs(string Id, Attribs Attributes)
        {
            if (table.ContainsKey(Id))
            {
                table[Id] = Attributes;
                return true;
            }
            else
                return false;
        }
    }
}
