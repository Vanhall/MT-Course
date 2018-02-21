using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR1
{
    public class VarTable : BaseTable<string, Attribs>
    {
        public VarTable() : base() { }

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
