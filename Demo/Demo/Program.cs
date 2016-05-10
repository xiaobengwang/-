using System;
using System.Collections.Generic;
using System.Text;

namespace Calculate
{
    class Calculator
    {
        static Dictionary<char, int> priorities = null;       //这是干嘛的 ？          
        const string operators = "+-*/";                      //声明加减乘除4个字符串，并且不可改变

        static Calculator()           //定义符号的优先级
        {
            priorities = new Dictionary<char, int>();

            priorities.Add('+', 0);
            priorities.Add('-', 0);
            priorities.Add('*', 1);
            priorities.Add('/', 1);

        }

        static double Compute(double leftNum, double rightNum, char op) //遇到符号左右两个数的计算方式
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

        static bool IsOperator(char op)    //如果op中是操作符，返回操作符索引大于等于0？
        {
            return operators.IndexOf(op) >= 0;
        }

        static bool IsLeftAssoc(char op)   //这是干嘛的
        {
            return op == '+' || op == '-' || op == '*' || op == '/';
        }

        static Queue<object> PreOrderToPostOrder(string expression) //这是设置什么的？
        {
            var result = new Queue<object>();   
            var operatorStack = new Stack<char>();
            operatorStack.Push('#');      //怎么少了这个输入老是显示表达式错误 
            char top, cur, tempChar;     //定义栈顶元素，读取元素？，tempchar是临时存放吗？
            string tempNum;     
            if (expression[0] == '-') expression = '0' + expression;  //如果符号优先级为0并且为-，字符串前面加0

            for (int i = 0, j; i < expression.Length; )    
            {
                cur = expression[i++];    //将第i个元素依次赋值给cur

                top = operatorStack.Peek();  //

                if (cur == '(')  //如果为（，则入栈
                {
                    operatorStack.Push(cur);  
                }
                else
                {
                    if (IsOperator(cur))   //这下面什么意思
                    {
                        while (IsOperator(top) && ((IsLeftAssoc(cur) && priorities[cur] <= priorities[top])) || (!IsLeftAssoc(cur) && priorities[cur] < priorities[top]))
                        {
                            result.Enqueue(operatorStack.Pop());
                            top = operatorStack.Peek();
                        }
                        operatorStack.Push(cur);
                    }
                    else if (cur == ')')  //如果为），当运算符数量大于0并且运算符前面不为左括号？
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
                        while (j < expression.Length && (expression[j] == '.' || (expression[j] >= '0' && expression[j] <= '9')))
                        {
                            tempNum += expression[j++];
                        }
                        i = j;
                        result.Enqueue(tempNum);
                    }
                }
            }
            while (operatorStack.Count > 0)//这边是干嘛的？
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

        static double Calucate(string expression)//进行报错部分
        {
            try
            {
                var rpn = PreOrderToPostOrder(expression);//这些都捕捉的什么信息，没看懂
                var operandStack = new Stack<double>();
                double left, right;
                object cur;
                while (rpn.Count > 0)
                {
                    cur = rpn.Dequeue();
                    if (cur is char)
                    {
                        right = operandStack.Pop();
                        left = operandStack.Pop();
                        operandStack.Push(Compute(left, right, (char)cur));
                    }
                    else
                    {
                        operandStack.Push(double.Parse(cur.ToString()));
                    }
                }
                return operandStack.Pop();
            }
            catch      //异常时弹出错误
            {
                throw new Exception("表达式格式不正确！");
            }
        }

        static void Main(string[] args)   
        {
            string expression;
            Console.Write("输入表达式（支持+-*/），输入exit退出: ");   //一开始控制台显示的文字
            while ((expression = Console.ReadLine()) != "exit")    //将输入的字串符赋值给上面的表达式并且不退出？
            {
                try
                {
                    Console.WriteLine(expression + " = " + Calucate(expression));//控制台显示表达式加=加计算结果
                }
                catch (Exception ex)    //这是弹出什么
                {
                    Console.WriteLine(ex.Message);
                }
                Console.Write("输入表达式（支持+-*/），输入exit退出: ");
            }
        }
    }
}