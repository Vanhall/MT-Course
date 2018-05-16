using System.Collections.Generic;
using System.Text;
using LR1;

namespace LR4
{
    public class CodeBuilder
    {
        ConstTable KeyWords;
        ConstTable Operations;
        VarTable Constants;
        VarTable Identifiers;
        ConstTable Goto;
        VarTable Labels;
        StringBuilder output;

        public CodeBuilder(
            ConstTable KeyWords,
            ConstTable Operations,
            VarTable Constants,
            VarTable Identifiers,
            ConstTable Goto,
            VarTable Labels
            )
        {
            this.KeyWords = KeyWords;
            this.Operations = Operations;
            this.Constants = Constants;
            this.Identifiers = Identifiers;
            this.Goto = Goto;
            this.Labels = Labels;

            output = new StringBuilder();
        }

        public void BuildCode(List<Token[]> Tokens)
        {
            // блок данных
            output.AppendLine(".386");
            output.AppendLine(".MODEL FLAT, C");
            output.AppendLine();
            output.AppendLine(".DATA");
            for (int i = 0; i < Identifiers.Count(); i++) 
                output.AppendLine(Identifiers.Find(i) + " DD ?");
            output.AppendLine();

            // код программы
            output.AppendLine(".CODE");
            output.AppendLine("MAIN PROC");
            output.AppendLine();


            var OpStack = new Stack<Token>();
            string CompOp = "";

            foreach (Token[] Line in Tokens)
            {
                if (Line[0].TableID == Labels.ID)
                {
                    if (Line.Length == 1)
                    {
                        output.AppendLine(Labels.Find(Line[0].Index) + ":");
                        output.AppendLine("PUSH EAX");
                        output.AppendLine("PUSH EBX");
                    }
                    else if (Line[1].Equals(Goto["GTF"]))
                    {
                        output.AppendLine("POP EBX");
                        output.AppendLine("POP EAX");
                        output.AppendLine(CompOp + Labels.Find(Line[0].Index));
                        output.AppendLine();
                    }
                    else
                    {
                        output.AppendLine("JMP " + Labels.Find(Line[0].Index));
                        output.AppendLine(Labels.Find(Line[2].Index) + ":");
                        output.AppendLine();
                    }
                }
                else
                {
                    bool newExpr = true;
                    foreach (Token T in Line)
                    {
                        if (T.TableID == Constants.ID || T.TableID == Identifiers.ID) OpStack.Push(T);
                        else if (newExpr)
                        {
                            CompOp = ResolveExpr(OpStack.Pop(), T, OpStack.Pop());
                            newExpr = false;
                        }
                        else CompOp = ResolveExpr(T, OpStack.Pop());
                    }
                    output.AppendLine();
                }
            }
            
            output.AppendLine("MAIN ENDP");
            output.AppendLine("END MAIN");
        }

        string ResolveExpr(Token LOP, Token Op, Token ROP)
        {
            string lop, rop;
            if (LOP.TableID == Constants.ID) lop = Constants.Find(LOP.Index);
            else lop = Identifiers.Find(LOP.Index);
            if (ROP.TableID == Constants.ID) rop = Constants.Find(ROP.Index);
            else rop = Identifiers.Find(ROP.Index);
            
            if (Op.Equals(Operations["+"]))
            {
                output.AppendLine("MOV EAX, " + lop);
                output.AppendLine("MOV EBX, " + rop);
                output.AppendLine("ADD EAX, EBX");
                return "";
            }

            if (Op.Equals(Operations["-"]))
            {
                output.AppendLine("MOV EAX, " + lop);
                output.AppendLine("MOV EBX, " + rop);
                output.AppendLine("SUB EAX, EBX");
                return "";
            }

            if (Op.Equals(Operations["="]))
            {
                output.AppendLine("MOV EBX, " + lop);
                output.AppendLine("MOV " + rop + ", EBX");
                return "";
            }

            if (Op.Equals(Operations["<"]))
            {
                output.AppendLine("MOV EAX, " + rop);
                output.AppendLine("MOV EBX, " + lop);
                output.AppendLine("CMP EAX, EBX");
                return "JGE ";
            }

            if (Op.Equals(Operations[">"]))
            {
                output.AppendLine("MOV EAX, " + rop);
                output.AppendLine("MOV EBX, " + lop);
                output.AppendLine("CMP EAX, EBX");
                return "JLE ";
            }

            if (Op.Equals(Operations["<="]))
            {
                output.AppendLine("MOV EAX, " + rop);
                output.AppendLine("MOV EBX, " + lop);
                output.AppendLine("CMP EAX, EBX");
                return "JG ";
            }

            if (Op.Equals(Operations[">="]))
            {
                output.AppendLine("MOV EAX, " + rop);
                output.AppendLine("MOV EBX, " + lop);
                output.AppendLine("CMP EAX, EBX");
                return "JL ";
            }

            return "";
        }


        string ResolveExpr(Token Op, Token ROP)
        {
            string rop;
            if (ROP.TableID == Constants.ID) rop = Constants.Find(ROP.Index);
            else rop = Identifiers.Find(ROP.Index);

            if (Op.Equals(Operations["+"]))
            {
                output.AppendLine("MOV EBX, " + rop);
                output.AppendLine("ADD EAX, EBX");
                return "";
            }

            if (Op.Equals(Operations["-"]))
            {
                output.AppendLine("MOV EBX, " + rop);
                output.AppendLine("SUB EAX, EBX");
                return "";
            }

            if (Op.Equals(Operations["="]))
            {
                output.AppendLine("MOV " + rop + ", EAX");
                return "";
            }
            
            return "";
        }

        public override string ToString()
        {
            return output.ToString();
        }
    }
}
