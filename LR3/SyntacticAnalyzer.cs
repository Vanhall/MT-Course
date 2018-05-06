using System.Collections.Generic;
using LR1;
using System;

namespace LR3
{
    public class SyntacticAnalyzer
    {
        struct StateDescriptor
        {
            public HashSet<Token> Terminals;
            public int Jump;
            public bool Accept, Stack, Ret, Error;

            public StateDescriptor(Token[] Terminals, int Jump, bool Accept, bool Stack, bool Ret, bool Error)
            {
                this.Terminals = new HashSet<Token>(Terminals);
                this.Jump = Jump;
                this.Accept = Accept;
                this.Stack = Stack;
                this.Ret = Ret;
                this.Error = Error;
            }

            public StateDescriptor(Token Terminal, int Jump, bool Accept, bool Stack, bool Ret, bool Error)
            {
                Terminals = new HashSet<Token> { Terminal };
                this.Jump = Jump;
                this.Accept = Accept;
                this.Stack = Stack;
                this.Ret = Ret;
                this.Error = Error;
            }
        }

        ConstTable KeyWords;
        ConstTable Delimiters;
        ConstTable Operations;
        VarTable Constants;
        VarTable Identifiers;

        StateDescriptor[] Table;

        public SyntacticAnalyzer(
            ConstTable KeyWords,
            ConstTable Delimiters,
            ConstTable Operations,
            VarTable Constants,
            VarTable Identifiers
            )
        {
            this.KeyWords = KeyWords;
            this.Delimiters = Delimiters;
            this.Operations = Operations;
            this.Constants = Constants;
            this.Identifiers = Identifiers;

            var BODYTerminals = new List<Token> { KeyWords["int"], KeyWords["while"], Delimiters["}"] };
            BODYTerminals.AddRange(Identifiers.ToArray());

            var D1Terminals = new Token[] { Operations["="], Delimiters[";"] };

            var EXPRTerminals = new List<Token> { Delimiters[";"] };
            EXPRTerminals.AddRange(Constants.ToArray());
            EXPRTerminals.AddRange(Identifiers.ToArray());

            var ICTerminals = new List<Token>(Constants.ToArray());
            ICTerminals.AddRange(Identifiers.ToArray());

            var W_BODYTerminals = new List<Token>(Identifiers.ToArray());
            W_BODYTerminals.Add(Delimiters["{"]);

            var COMPTerminals = new Token[] { Operations["=="], Operations["<="], Operations[">="], Operations["<"], Operations[">"] };
            var XTerminals = new Token[] { Operations["+"], Operations["-"], Delimiters[";"] };
            var OPTerminals = new Token[] { Operations["+"], Operations["-"] };

            Table = new StateDescriptor[]
            {
                new StateDescriptor(KeyWords["void"], 1, false, false, false, true),          //0 PROG
                new StateDescriptor(KeyWords["void"], 2, true, false, false, true),           //1
                new StateDescriptor(KeyWords["main"], 3, true, false, false, true),           //2
                new StateDescriptor(Delimiters["("], 4, true, false, false, true),            //3
                new StateDescriptor(Delimiters[")"], 5, true, false, false, true),            //4
                new StateDescriptor(Delimiters["{"], 6, true, false, false, true),            //5
                new StateDescriptor(BODYTerminals.ToArray(), 8, false, true, false, true),    //6 BODY
                new StateDescriptor(Delimiters["}"], -1, true, false, false, true),           //7
                new StateDescriptor(KeyWords["int"], 12, false, false, true, false),          //8 BODY1
                new StateDescriptor(KeyWords["while"], 23, false, false, false, false),       //9 BODY2
                new StateDescriptor(Identifiers.ToArray(), 46, false, false, false, false),   //10 BODY3
                new StateDescriptor(Delimiters["}"], 75, false, false, false, true),          //11 BODY4
                new StateDescriptor(KeyWords["int"], 13, true, false, false, true),           //12
                new StateDescriptor(Identifiers.ToArray(), 15, false, true, false, true),     //13 DECL
                new StateDescriptor(BODYTerminals.ToArray(), 8, false, false, false, true),   //14 BODY
                new StateDescriptor(Identifiers.ToArray(), 16, true, false, false, true),     //15
                new StateDescriptor(D1Terminals, 18, false, true, false, true),               //16 D1
                new StateDescriptor(Delimiters[";"], -1, true, false, true, true),            //17
                new StateDescriptor(Operations["="], 20, false, false, false, false),         //18 D11
                new StateDescriptor(Delimiters[";"], 22, false, false, false, true),          //19 D12
                new StateDescriptor(Operations["="], 21, true, false, false, true),           //20
                new StateDescriptor(EXPRTerminals.ToArray(), 60, false, false, false, true),  //21 EXPR
                new StateDescriptor(Delimiters[";"], -1, false, false, true, true),           //22 EPS
                new StateDescriptor(KeyWords["while"], 24, true, false, false, true),         //23
                new StateDescriptor(Delimiters["("], 25, true, false, false, true),           //24
                new StateDescriptor(ICTerminals.ToArray(), 29, false, true, false, true),     //25 L_EXPR
                new StateDescriptor(Delimiters[")"], 27, true, false, false, true),           //26
                new StateDescriptor(W_BODYTerminals.ToArray(), 51, false, true, false, true), //27 W_BODY
                new StateDescriptor(BODYTerminals.ToArray(), 8, false, false, false, true),   //28 BODY
                new StateDescriptor(ICTerminals.ToArray(), 32, false, true, false, true),     //29 LOP
                new StateDescriptor(COMPTerminals, 36, false, true, false, true),             //30 COMP
                new StateDescriptor(ICTerminals.ToArray(), 32, false, false, false, true),    //31 LOP
                new StateDescriptor(Identifiers.ToArray(), 34, false, false, false, false),   //32 LOP1
                new StateDescriptor(Constants.ToArray(), 35, false, false, false, true),      //33 LOP1
                new StateDescriptor(Identifiers.ToArray(), -1, true, false, true, true),      //34
                new StateDescriptor(Constants.ToArray(), -1, true, false, true, true),        //35
                new StateDescriptor(Operations["=="], 41, false, false, false, false),        //36 COMP1
                new StateDescriptor(Operations["<="], 42, false, false, false, false),        //37 COMP2
                new StateDescriptor(Operations[">="], 43, false, false, false, false),        //38 COMP3
                new StateDescriptor(Operations["<"], 44, false, false, false, false),         //39 COMP4
                new StateDescriptor(Operations[">"], 45, false, false, false, true),          //40 COMP5
                new StateDescriptor(Operations["=="], -1, true, false, true, true),           //41
                new StateDescriptor(Operations["<="], -1, true, false, true, true),           //42
                new StateDescriptor(Operations[">="], -1, true, false, true, true),           //43
                new StateDescriptor(Operations["<"], -1, true, false, true, true),            //44
                new StateDescriptor(Operations[">"], -1, true, false, true, true),            //45
                new StateDescriptor(Identifiers.ToArray(), 47, true, false, false, true),     //46
                new StateDescriptor(Operations["="], 48, true, false, false, true),           //47
                new StateDescriptor(EXPRTerminals.ToArray(), 60, false, true, false, true),   //48 EXPR
                new StateDescriptor(Delimiters[";"], 50, true, false, false, true),           //49
                new StateDescriptor(BODYTerminals.ToArray(), 8, false, false, false, true),   //50 BODY
                new StateDescriptor(Delimiters["{"], 53, false, false, false, false),         //51 W_BODY1
                new StateDescriptor(Identifiers.ToArray(), 56, false, false, false, true),    //52 W_BODY1
                new StateDescriptor(Delimiters["{"], 54, true, false, false, true),           //53
                new StateDescriptor(BODYTerminals.ToArray(), 8, false, true, false, true),    //54 BODY
                new StateDescriptor(Delimiters["}"], 54, true, false, true, true),            //55
                new StateDescriptor(Identifiers.ToArray(), 57, true, false, false, true),     //56
                new StateDescriptor(Operations["="], 58, true, false, false, true),           //57
                new StateDescriptor(EXPRTerminals.ToArray(), 60, false, true, false, true),   //58 EXPR
                new StateDescriptor(Delimiters[";"], -1, true, false, true, true),            //59
                new StateDescriptor(Constants.ToArray(), 62, false, false, false, false),     //60 EXPR1
                new StateDescriptor(Identifiers.ToArray(), 64, false, false, false, true),    //61 EXPR2
                new StateDescriptor(Constants.ToArray(), 63, true, false, false, true),       //62
                new StateDescriptor(XTerminals, 66, false, false, false, true),               //63 X
                new StateDescriptor(Identifiers.ToArray(), 65, true, false, false, true),     //64
                new StateDescriptor(XTerminals, 66, false, false, false, true),               //65 X
                new StateDescriptor(OPTerminals, 68, false, false, false, false),             //66 X1
                new StateDescriptor(Delimiters[";"], 70, false, false, false, true),          //67 X2
                new StateDescriptor(OPTerminals, 71, false, true, false, true),               //68 OP
                new StateDescriptor(EXPRTerminals.ToArray(), 60, false, false, false, true),  //69 EXPR
                new StateDescriptor(Delimiters[";"], -1, false, false, true, true),           //70 EPS
                new StateDescriptor(Operations["+"], 73, false, false, false, false),         //71 OP1
                new StateDescriptor(Operations["-"], 73, false, false, false, true),          //72 OP2
                new StateDescriptor(Operations["+"], -1, true, false, true, true),            //73
                new StateDescriptor(Operations["-"], -1, true, false, true, true),            //74
                new StateDescriptor(Delimiters["}"], -1, false, false, true, true),           //75 EPS
            };
        }

        public Token[] AnalyzeTokens(Token[] Tokens)
        {
            var result = new List<Token>();

            var Token = ((IEnumerable<Token>)Tokens).GetEnumerator();
            int CurrentState = 0;
            bool hasNext = Token.MoveNext();
            var StateStack = new Stack<int>();

            while (hasNext)
            {
                if (Table[CurrentState].Terminals.Contains(Token.Current))
                {
                    if (Table[CurrentState].Accept) hasNext = Token.MoveNext();
                    if (Table[CurrentState].Stack) StateStack.Push(CurrentState + 1);
                    if (Table[CurrentState].Jump >= 0)
                    {
                        CurrentState = Table[CurrentState].Jump;
                        continue;
                    }
                    if (Table[CurrentState].Ret) CurrentState = StateStack.Pop();
                }
                else
                {
                    if (Table[CurrentState].Error) ReportError();
                    else CurrentState++;
                }
            }

            return result.ToArray();
        }

        void ReportError()
        {
            throw new NotImplementedException();
        }
    }
}
