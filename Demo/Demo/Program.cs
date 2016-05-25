using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Calculate
{

    class Calculator
    {
        static Dictionary<char, int> priorities = null;
        const string operators = "+-*/!`@^%$";


        static Calculator()
        {
            priorities = new Dictionary<char, int>();
            priorities.Add('#', -2);
            priorities.Add('(', -1);
            priorities.Add(')', -1);
            priorities.Add(',', -1);
            priorities.Add('+', 0);
            priorities.Add('-', 0);
            priorities.Add('*', 1);
            priorities.Add('/', 1);
            priorities.Add('%', 1);
            priorities.Add('!', 2);
            priorities.Add('^', 3);
            priorities.Add('`', 4);
            priorities.Add('@', 4);
            priorities.Add('$', 4);

        }

        static double Compute(double leftNum, double rightNum, char op)
        {
            switch (op)
            {
                case '+': return leftNum + rightNum;
                case '-': return leftNum - rightNum;
                case '*': return leftNum * rightNum;
                case '/': return leftNum / rightNum;

                case '@': return leftNum < rightNum ? leftNum : rightNum;
                case '^': return Math.Pow(leftNum, rightNum);
                case '%': return leftNum % rightNum;
                default: return 0;
            }
        }

        public static double Max(char[] charArray)
        {
           
            double maxVal = charArray[0];

            for (int i = 1; i < charArray.Length; i++)
            {
                if (charArray[i] > maxVal)
                {
                    maxVal = charArray[i];
                }
               
            }
           double str = maxVal - 48;
            return str;
        }

        static double Computer(double rightNum, char op)
        {
            switch (op)
            {
                case '!': return -rightNum;
                case '$': return Math.Sin(rightNum * Math.PI / 180);
                default: return 0;
            }
        }

        static bool IsOperator(char op)
        {
            return operators.IndexOf(op) >= 0;

        }

        static bool IsLeftAssoc(char op)
        {
            return op == '+' || op == '-' || op == '*' || op == '/' || op == '@' || op == '%' || op == '^';
        }
        static bool IsRightAssoc(char op)
        {
            return op == '!' || op == '$';
        }
        static bool IsFunc(char op)
        {
            return op == '`';
        }

        static bool IsMid(char op)
        {
            return op == ',';
        }


        static Queue<object> PreOrderToPostOrder(string expression)
        {
            System.Text.StringBuilder carray = new System.Text.StringBuilder(expression);

            var result = new Queue<object>();
            var operatorStack = new Stack<char>();
            operatorStack.Push('#');
            char top, cur, tempChar;
            string tempNum;

            for (int i = 0; i < carray.Length - 1; i++)
            {

                if (carray[i] == '-' && (i == 0 || carray[i - 1] == '('))

                    carray[i] = '!';
            }
            for (int i = 0; i < carray.Length - 1; i++)
            {
                if (carray[i] == 'm' && carray[i + 1] == 'a' && carray[i + 2] == 'x')
                {
                    carray[i] = '`';

                    carray.Remove(i + 1, i + 2);
                }
                if (carray[i] == 'm' && carray[i + 1] == 'i' && carray[i + 2] == 'n')
                {
                    carray[i] = '@';

                    carray.Remove(i + 1, i + 2);
                }
                if (carray[i] == 's' && carray[i + 1] == 'i' && carray[i + 2] == 'n')
                {
                    carray[i] = '$';

                    carray.Remove(i + 1, i + 2);
                }
            }

            for (int i = 0, j; i < carray.Length; )
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
                        while (IsOperator(top) && ((IsLeftAssoc(cur) && priorities[cur] <= priorities[top])) || (IsRightAssoc(cur) && priorities[cur] < priorities[top]) || IsFunc(top))
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
                            if (operatorStack.Count == 1)
                            {
                                Console.Write("括号不匹配");
                            }

                        }
                        if (top == '`')
                        {
                            result.Enqueue(operatorStack.Pop());
                            top = operatorStack.Peek();
                        }


                    }
                    else if (cur == ',')
                    {
                        while (operatorStack.Count > 0 && (tempChar = operatorStack.Pop()) != '(')
                        {
                            result.Enqueue(tempChar);
                        }
                        result.Enqueue(cur);
                        operatorStack.Push('(');
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

                if (cur == '(')
                {
                    Console.Write("括号不匹配");
                }
                else if (cur == '#') continue;


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
                string str = Convert.ToString(' ');
                char[] Str = new Char[100];

                List<char> StrList = new List<char>();

                while (rpn.Count > 0)
                {

                    cur = rpn.Dequeue();
                    if (cur is char && !IsMid(Convert.ToChar(cur)))
                    {
                        if (IsRightAssoc(Convert.ToChar(cur)))
                        {
                            right = operandStack.Pop();
                            operandStack.Push(Computer(right, (char)cur));
                        }


                        else if (IsLeftAssoc(Convert.ToChar(cur)))
                        {
                            if (operandStack.Count == 1)
                            {
                                Console.Write("符号不匹配");
                            }
                            right = operandStack.Pop();
                            left = operandStack.Pop();
                            operandStack.Push(Compute(left, right, (char)cur));

                        }
                        else if (IsFunc(Convert.ToChar(cur)))
                        {
                            if (operandStack.Count > 0)
                            {

                                str += Convert.ToString(operandStack.Pop());
                                for (int i = str.Length - 1, j = 0; i >= 0; i--)
                                {
                                    Str[j++] = str[i];
                                }
                            }
                            operandStack.Push(Max(Str));
                        }

                    }
                    else if (IsMid(Convert.ToChar(cur)))
                    {
                        str += Convert.ToString(operandStack.Pop());

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