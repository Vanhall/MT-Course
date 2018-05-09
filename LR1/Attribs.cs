namespace LR1
{
    /// <summary>
    /// Структура атрибутов
    /// </summary>
    public struct Attribs
    {
        /// <summary>
        /// Инициализирован ли идентификатор?
        /// </summary>
        public bool HasType;

        /// <summary>
        /// Тип идентификатора
        /// </summary>
        public bool HasValue;
        
        /// <summary>
        /// Переопределение стандартного метода ToString
        /// </summary>
        /// <returns>Строковое представление объекта</returns>
        public override string ToString()
        {
            return "Type defined = " + HasType + "; Initialized = " + HasValue;
        }
    }
}
