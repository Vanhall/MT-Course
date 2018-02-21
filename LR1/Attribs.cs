namespace LR1
{
    public struct Attribs
    {
        public bool Init;
        public int IdType;
        public int Val;

        public override string ToString()
        {
            return "Initialized = " + Init + "; Type = " + IdType + "; Value = " + Val;
        }
    }
}
