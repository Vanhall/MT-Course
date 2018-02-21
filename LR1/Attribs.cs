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
        public bool Init;

        /// <summary>
        /// Тип идентификатора
        /// </summary>
        public int IDType;

        /// <summary>
        /// Значение идентификатора
        /// </summary>
        public int Val;

        /// <summary>
        /// Переопределение стандартного метода ToString
        /// </summary>
        /// <returns>Строковое представление объекта</returns>
        public override string ToString()
        {
            return "Initialized = " + Init + "; Type = " + IDType + "; Value = " + Val;
        }
    }
}
