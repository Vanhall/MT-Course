using System;
using System.Text;
using LR1;

namespace LR2
{
    /// <summary>
    /// Конечный автомат для лексического анализа.
    /// </summary>
    public class FiniteStateMachine
    {
        /// <summary>
        /// Именованые константы состояний автомата.
        /// </summary>
        public enum States
        {
            Start,
            Const,
            Id,
            Op1,
            Op2,
            Slash,
            LineComm,
            BlockComm,
            BlockCommEnd,
            Final
        }

        /// <summary>
        /// Текущее состояние автомата.
        /// </summary>
        public States State;

        ConstTable KeyWords;
        ConstTable Delimiters;
        ConstTable Operations;
        VarTable Constants;
        VarTable Identifiers;
        ConstTable Letters;
        ConstTable Digits;

        /// <summary>
        /// Токен распознанной лексемы. В случае если лексема - пробел
        /// или табуляция, присваивается null.
        /// </summary>
        public Token CurrentToken;

        /// <summary>
        /// Это свойство показывает, нужно ли откатиться на один символ назад
        /// (чтобы не пропустить разделители).
        /// </summary>
        public bool StepBack;

        /// <summary>
        /// Буфер символов распознаваемой лексемы.
        /// </summary>
        string Buffer;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public FiniteStateMachine()
        {
            State = States.Start;
            KeyWords = new ConstTable(@"Tables/KeyWords.txt");
            Delimiters = new ConstTable(@"Tables/Delimiters.txt");
            Operations = new ConstTable(@"Tables/Operations.txt");
            Constants = new VarTable();
            Identifiers = new VarTable();
            Letters = new ConstTable(@"Tables/Letters.txt");
            Digits = new ConstTable(@"Tables/Digits.txt");

            Buffer = "";
            StepBack = false;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public FiniteStateMachine(
            ConstTable KeyWords,
            ConstTable Delimiters,
            ConstTable Operations,
            VarTable Constants,
            VarTable Identifiers
            )
        {
            State = States.Start;
            this.KeyWords = KeyWords;
            this.Delimiters = Delimiters;
            this.Operations = Operations;
            this.Constants = Constants;
            this.Identifiers = Identifiers;
            Letters = new ConstTable(@"Tables/Letters.txt");
            Digits = new ConstTable(@"Tables/Digits.txt");

            Buffer = "";
            StepBack = false;
        }

        /// <summary>
        /// Обработка очередного символа.
        /// </summary>
        /// <param name="C">Обрабатываемый символ.</param>
        public void ProcessSymbol(char C)
        {
            string CStr = Char.ToLower(C).ToString();
            switch (State)
            {
                case States.Start:
                    {
                        Buffer = "" + C;
                        if (Digits.Contains(CStr)) State = States.Const;
                        else if (Letters.Contains(CStr)) State = States.Id;
                        else if (Operations.Contains(CStr)) State = States.Op1;
                        else if (C == '/') State = States.Slash;
                        else if (Delimiters.Contains(CStr))
                        {
                            State = States.Final;
                            CurrentToken = GenerateToken();
                        }
                        else if (Char.IsWhiteSpace(C))
                        {
                            State = States.Final;
                            CurrentToken = null;
                        }
                        else throw new Exception("Недопустимый символ");
                    }
                    break;
                case States.Const:
                    {
                        if (Digits.Contains(CStr)) Buffer += C;
                        else if (Delimiters.Contains(CStr) || Char.IsWhiteSpace(C) || Operations.Contains(CStr))
                        {
                            CurrentToken = GenerateToken();
                            StepBack = true;
                            State = States.Final;
                        }
                        else throw new Exception("Недопустимый символ в константе");
                    }
                    break;
                case States.Id:
                    {
                        if (Letters.Contains(CStr) || Digits.Contains(CStr)) Buffer += C;
                        else if (Delimiters.Contains(CStr) || Char.IsWhiteSpace(C) || Operations.Contains(CStr))
                        {
                            CurrentToken = GenerateToken();
                            StepBack = true;
                            State = States.Final;
                        }
                        else throw new Exception("Недопустимый символ в идентификаторе");
                    }
                    break;
                case States.Op1:
                    {
                        if (C == '=')
                        {
                            Buffer += C;
                            State = States.Op2;
                        }
                        else if (Delimiters.Contains(CStr) || Char.IsWhiteSpace(C) || Letters.Contains(CStr) || Digits.Contains(CStr))
                        {
                            CurrentToken = GenerateToken();
                            StepBack = true;
                            State = States.Final;
                        }
                        else throw new Exception("Недопустимый символ после знака операции");
                    }
                    break;
                case States.Op2:
                    {
                        if (Delimiters.Contains(CStr) || Char.IsWhiteSpace(C) || Letters.Contains(CStr) || Digits.Contains(CStr))
                        {
                            CurrentToken = GenerateToken();
                            StepBack = true;
                            State = States.Final;
                        }
                        else throw new Exception("Недопустимый символ после знака операции");
                    }
                    break;
                case States.Slash:
                    {
                        if (C == '/') State = States.LineComm;
                        else if (C == '*') State = States.BlockComm;
                        else throw new Exception("Ошибка в комментарии");
                    }
                    break;
                case States.BlockComm:
                    {
                        if (C == '*') State = States.BlockCommEnd;
                    }
                    break;
                case States.BlockCommEnd:
                    {
                        if (C == '/') State = States.Final;
                        else State = States.BlockComm;
                    }
                    break;
            }
        }

        /// <summary>
        /// Генерация токена распознанной лексемы.
        /// </summary>
        /// <returns>Токен распознанной лексемы.</returns>
        Token GenerateToken()
        {
            switch (State)
            {
                case States.Const:
                    {
                        Constants.Add(Buffer);
                        return Constants.GetToken(Buffer);
                    }
                case States.Id:
                    {
                        if (KeyWords.Contains(Buffer)) return KeyWords.GetToken(Buffer);
                        else
                        {
                            Identifiers.Add(Buffer);
                            return Identifiers.GetToken(Buffer);
                        }
                    }
                case States.Op1:
                case States.Op2:
                    {
                        return Operations.GetToken(Buffer);
                    }
                case States.Final:
                    {
                        return Delimiters.GetToken(Buffer);
                    }
                default: return null;
            }
        }

        /// <summary>
        /// Вывод таблиц языка.
        /// </summary>
        /// <returns>Таблицы языка.</returns>
        public string PrintTables()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("ID {0} | KeyWords\n", KeyWords.ID);
            sb.Append(KeyWords + "\n");
            sb.AppendFormat("ID {0} | Delimiters\n", Delimiters.ID);
            sb.Append(Delimiters + "\n");
            sb.AppendFormat("ID {0} | Operations\n", Operations.ID);
            sb.Append(Operations + "\n");
            sb.AppendFormat("ID {0} | Constants\n", Constants.ID);
            sb.Append(Constants + "\n");
            sb.AppendFormat("ID {0} | Identifiers\n", Identifiers.ID);
            sb.Append(Identifiers + "\n");
            return sb.ToString();
        }
    }
}
