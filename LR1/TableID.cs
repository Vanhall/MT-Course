namespace LR1
{
    public abstract class TableID
    {
        static int GlobalID = 0;
        public int ID { get; }

        public TableID()
        {
            ID = GlobalID++;
        }
    }
}
