namespace LR1
{
    public class Token
    {
        public int TableID { get; set; }
        public int Index { get; set; }
        
        public Token(int TableID, int Index)
        {
            this.TableID = TableID;
            this.Index = Index;
        }

        public override string ToString()
        {
            return "(" + TableID + "," + Index + ")";
        }
    }
}
