using System;
using System.Collections.Generic;
using System.Text;

namespace Calculate
{
    class Calculator
    {
        static Dictionary<char, int> priorities = null;              
        const string operators = "+-*/!";

        static Calculator()
        {
            priorities = new Dictionary<char, int>();
          
            priorities.Add('+', 0);
            priorities.Add('-', 0);
            priorities.Add('*', 1);
            priorities.Add('/', 1);
            priorities.Add('!', 2);  
                 
        }

        static double Compute(double leftNum, double rightNum, char op)
        {
            switch (op)
            {
                case '+': return leftNum + rightNum;
                case '-': return leftNum - rightNum;
                case '*': return leftNum * rightNum;
                case '/': return leftNum / rightNum;                      
               default: return 0;
            }
        }
        static double Computer(double rightNum, char op)  //
        {
            switch (op)
            {
                case '!': return -rightNum;
                default: return 0;
            }                                          //
        }  
        static bool IsOperator(char op)                
        {
            return operators.IndexOf(op) >= 0;
        }

        static bool IsLeftAssoc(char op)
        {
            return op == '+' || op == '-' || op == '*' || op == '/';
        }
        static bool IsRightAssoc(char op)              //
        {
            return op == '!';
        }                                             //
       

        static Queue<object> PreOrderToPostOrder(string expression)
        {
            System.Text.StringBuilder carray = new System.Text.StringBuilder(expression);
            
            var result = new Queue<object>();
            var operatorStack = new Stack<char>();
            operatorStack.Push('#');
            char top, cur, tempChar;
            string tempNum;

            for (int i = 0, j; i < carray.Length;i++ )
            if (carray[i] == '-' && (i == 0 || carray[i - 1] == '('))

                carray[i] = '!';//这边预定义把-换为！号，可是没效果
           
            for (int i = 0,j; i < carray.Length; )
                {

                 
                    cur = carray[i++];
                    top = operatorStack.Peek();

                    if (cur == '(')
                    {
                        operatorStack.Push(cur);
                    }
                   
                    else
                    {
                        if (IsOperator(cur))
                        {
                            while (IsOperator(top) && ((IsLeftAssoc(cur) && priorities[cur] <= priorities[top])) || (IsRightAssoc(cur) && priorities[cur] < priorities[top]))
                            {
                                result.Enqueue(operatorStack.Pop());
                                top = operatorStack.Peek();
                            }
                            operatorStack.Push(cur);
                        }
                        else if (cur == ')')
                        {
                            while (operatorStack.Count > 0 && (tempChar = operatorStack.Pop()) != '(')
                            {
                                result.Enqueue(tempChar);
                            }
                        }
                        else
                        {
                            tempNum = "" + cur;
                            j = i;
                            while (j < carray.Length && (carray[j] == '.' || (carray[j] >= '0' && carray[j] <= '9')))
                            {
                                tempNum += carray[j++];
                            }
                            i = j;
                            result.Enqueue(tempNum);
                        }
                    }
                }
            
            while (operatorStack.Count > 0)
            {
                cur = operatorStack.Pop();
                if (cur == '#') continue;
                if (operatorStack.Count > 0)
                {
                    top = operatorStack.Peek();
                }

                result.Enqueue(cur);
            }

            return result;
        }

        static double Calucate(string expression)
        {
            try
            {
                var rpn = PreOrderToPostOrder(expression);
                var operandStack = new Stack<double>();
                double left, right;
                object cur;
                while (rpn.Count > 0)
                {
                    cur = rpn.Dequeue();
                    if (cur is char)
                    {
                        if (IsRightAssoc(Convert.ToChar(cur)))
                        {
                            right = operandStack.Pop();
                            operandStack.Push(Computer(right, (char)cur));
                        }

                        else
                        {
                            right = operandStack.Pop();
                            left = operandStack.Pop();
                            operandStack.Push(Compute(left, right, (char)cur));
                        }
                    }
                    else
                    {
                        operandStack.Push(double.Parse(cur.ToString()));
                    }
                }
                return operandStack.Pop();
            }
            catch
            {
                throw new Exception("表达式格式不正确！");
            }
        }

        static void Main(string[] args)
        {
            string expression;
            Console.Write("输入表达式（支持+-*/），输入exit退出: ");
            while ((expression = Console.ReadLine()) != "exit")
            {
                try
                {
                    Console.WriteLine(expression + " = " + Calucate(expression));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.Write("输入表达式（支持+-*/），输入exit退出: ");
            }
        }
    }
}