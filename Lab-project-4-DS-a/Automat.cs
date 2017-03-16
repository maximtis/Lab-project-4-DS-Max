using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_project_4_DS_a
{
    enum Param { shift=1, hold=0 };
    delegate bool Function();
    delegate bool Computing();
    class Automat
    {
        Function[,] FunctionTable;
        Function Rule;
        public Stack<string> StackWorker;       //Распознаватель
        public List<string> ChainletWorker;     //Рабочая входная цепочка
        public Stack<string> StackComputor;       //Рабочий стек дял расчетов
        List<string> Alphabet;                  //Список возможных елементов
        List<string> StateList;                 //Список состояний
        public int CurrentIndex;                       // Текущая позиция в цепочке
        int StackCellIndex;                     // Индексатор значений полей в стеке
        string LastOperator;
        bool shiftFlag;


        public Automat()
        {
        #region Initializing
            shiftFlag = true;
            LastOperator = string.Empty;
            StackCellIndex = 0;
            CurrentIndex = 0;
            Alphabet = new List<string>
            {
                "+","*","C","~"
            };
            StateList = new List<string>
            {
                "<S>","<E>","~","{слож}","{умнож}","{ответ}"
            };
            StackComputor=new Stack<string>();
            StackWorker = new Stack<string>();
            StackWorker.Push("<S>");
            ChainletWorker = new List<string>();
            FunctionTable= new Function[6,4]
            {
                {ChangeRule1,  ChangeRule1,  ChangeRule1,  Error},
                {ChangeRule2,  ChangeRule3,  ChangeRule4,  Error},
                {  Error,      Error,        Error,      Confirm},
                {ChangeRule4,ChangeRule4,ChangeRule4,ChangeRule4},
                {ChangeRule4,ChangeRule4,ChangeRule4,ChangeRule4},
                {ChangeRule4,ChangeRule4,ChangeRule4,ChangeRule4}
            };
        #endregion
        }
        public bool RunStep()
        {
            if (StackWorker.Count == 0) return false;
            double CheckIndex;
            if(double.TryParse(ChainletWorker[CurrentIndex],out CheckIndex))
                Rule = FunctionTable[StateList.IndexOf(StackWorker.Peek()), Alphabet.IndexOf("C")]; 
            else
                Rule = FunctionTable[StateList.IndexOf(StackWorker.Peek()), Alphabet.IndexOf(ChainletWorker[CurrentIndex])];
            return Rule();
        }
        public bool Calculate(string Choose)
        {
            int Operand1 = 0;
            int Operand2 = 0;
            if (shiftFlag)
                    switch (Choose)
                    {
                        case "{слож}":
                            Operand1=Convert.ToInt32(StackComputor.Pop());
                            Operand2=Convert.ToInt32(StackComputor.Pop());
                            StackComputor.Pop();
                            StackComputor.Push((Operand1 + Operand2).ToString());
                            break;
                        case "{умнож}":
                            Operand1=Convert.ToInt32(StackComputor.Pop());
                            Operand2=Convert.ToInt32(StackComputor.Pop());
                            StackComputor.Pop();
                            StackComputor.Push((Operand1 * Operand2).ToString());
                            break;
                        default:
                            break;
                    }
                shiftFlag = false;
            return true;
        }
        public bool ChangeRule1()
        {
            //------- Stack ---------//
            Pop();
            StackWorker.Push(NewFieldWithIndex());
            StackWorker.Push("{ответ}");
            StackWorker.Push(NewFieldWithIndex());
            StackWorker.Push("<E>");
            // <-Hold-> Chainlet
            //-----------------------//
            return true;
        }
        public bool ChangeRule2()
        {
            //------- Stack ---------//
            Pop();
            StackWorker.Push(NewFieldWithIndex());
            StackWorker.Push(NewFieldWithIndex());
            StackWorker.Push("{слож}");
            LastOperator = "{слож}";
            StackWorker.Push(NewFieldWithIndex());
            StackWorker.Push("<E>");
            StackWorker.Push(NewFieldWithIndex());
            StackWorker.Push("<E>");
            StackComputor.Push(ChainletWorker[CurrentIndex]);
            shiftFlag=Shift();
            // <-Shift-> Chainlet
            //-----------------------//
            return true;
        }
        public bool ChangeRule3()
        {
            //------- Stack ---------//
            Pop();
            StackWorker.Push(NewFieldWithIndex());
            StackWorker.Push(NewFieldWithIndex());
            StackWorker.Push("{умнож}");
            LastOperator = "{умнож}";
            StackWorker.Push(NewFieldWithIndex());
            StackWorker.Push("<E>");
            StackWorker.Push(NewFieldWithIndex());
            StackWorker.Push("<E>");
            StackComputor.Push(ChainletWorker[CurrentIndex]);
            shiftFlag=Shift();
            // <-Shift-> Chainlet
            //-----------------------//
            return true;
        }
        public bool ChangeRule4()
        {
            if(StackWorker.Peek()=="{слож}"||(StackWorker.Peek()=="{умнож}"))
            {
                switch(StackWorker.Peek())
                {
                    case "{слож}":
                        Calculate(StackWorker.Peek());
                        break;
                    case "{умнож}":
                        Calculate(StackWorker.Peek());
                        break;
                }
                Pop();
                Pop();
                Pop();
                Pop();
            }
            else
            {
                StackComputor.Push(ChainletWorker[CurrentIndex]);
                Pop();
                Pop();
                shiftFlag = Shift();
            }
            return true;
        }
        public bool Shift()
        {

            CurrentIndex++;
            return true;
        }
        public bool Pop()
        {
            return StackWorker.Pop() != string.Empty;
        }
        public string GetOper()
        {
            string value = StackComputor.Pop();
            StackComputor.Pop();
            return value;
        }
        public bool Error()
        {
            throw new Exception();
        }
        public void Reset()
        {
            CurrentIndex = 0;
        }

        public string NewFieldWithIndex()
        {
            string NewField = "[" + StackCellIndex + "]";
            StackCellIndex++;
            return NewField;
        }
        public bool Confirm()
        {
            return true;
        }
        public bool IsOperator()
        {
            foreach (var x in Alphabet)
                if (x == StackComputor.Peek())
                    return true;
            return false;
        }
        public bool IsDigit()
        {
            int checkValue;
            return int.TryParse(ChainletWorker[CurrentIndex], out checkValue);
        }
    }
}
