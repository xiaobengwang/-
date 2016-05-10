using System;
using System.Collections.Generic;

namespace Calculate
{
    class Calculator
    {
        static Dictionary<char, int> priorities = null;       //这是干嘛的 ？ Allen:这是操作符优先级字典，key是操作符，value是优先级，值越大，优先级越高
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

        static bool IsOperator(char op)    //如果op中是操作符，返回操作符索引大于等于0？Allen:这是检查一下当前字符是不是操作符，是就返回true，否则返回false
        {
            return operators.IndexOf(op) >= 0;
        }

        static bool IsLeftAssoc(char op)   //这是干嘛的 Allen:是否左结合表达式，也就是
        {
            return op == '+' || op == '-' || op == '*' || op == '/';
        }

        static Queue<object> PreOrderToPostOrder(string expression) //这是设置什么的？ Allen:这就是中序表达式转
        {
            var result = new Queue<object>();
            var operatorStack = new Stack<char>();
            operatorStack.Push('#');      //怎么少了这个输入老是显示表达式错误 Allen:程序写死了用一个特殊符号作为表达式起始位置，后面的判断会用到
            char top, cur, tempChar;     //定义栈顶元素，读取元素？，tempchar是临时存放吗？
            string tempNum;
            if (expression[0] == '-') expression = '0' + expression;  //如果符号优先级为0并且为-，字符串前面加0 Allen:No no no,这是处理第一个操作数是负数的情况，在前面加一个0这个表达式就从-x变成0-x，就不要特殊处理了

            for (int i = 0, j; i < expression.Length;)
            {
                cur = expression[i++];    //将第i个元素依次赋值给cur Allen:i会自增的，不然会死循环

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

        static double Calucate(string expression)//进行报错部分 Allen:这里不是报错，是程序的核心逻辑，也就是计算表达式的部分
        {
            try
            {
                var rpn = PreOrderToPostOrder(expression);//这些都捕捉的什么信息，没看懂 Allen:调用中序表达式转后序表达式逻辑，返回后序表达式，队列里的每个元素都是一个操作数或者操作符
                var operandStack = new Stack<double>();
                double left, right;
                object cur;
                while (rpn.Count > 0)
                {
                    cur = rpn.Dequeue();
                    if (cur is char) //Allen:字符类型的是操作符就计算，操作数之前已经压栈了
                    {
                        right = operandStack.Pop(); //Allen:弹出栈顶的两个元素分别作为左值和右值，这种方式不能处理多参数的函数，需要你思考下怎么解决
                        left = operandStack.Pop();
                        operandStack.Push(Compute(left, right, (char)cur));//Allen:执行当前操作数，计算结果再作为操作数压栈
                    }
                    else
                    {
                        operandStack.Push(double.Parse(cur.ToString())); //Allen ：不是字符类型的就做操作数，转成数值型压栈
                    }
                }
                return operandStack.Pop(); //Allen：执行成功的话，堆栈应该只有一个元素，就是最终的结果，出栈作为整个表达式的值
            }
            //catch      //异常时弹出错误 Allen:这种写法非常不好，异常的详细信息都丢了，应该写成下面这样，这样不能明确错误的原因，不和题意，需要考虑怎么在解析表达式的时候能提示出正确的错误类型
            catch (Exception ex)
            {
                throw new Exception("表达式格式不正确！", ex);//可以到innerException中看细节
            }
        }

        static void Main(string[] args)
        {
            string expression;
            Console.Write("输入表达式（支持+-*/），输入exit退出: ");   //一开始控制台显示的文字
            while ((expression = Console.ReadLine()) != "exit")    //将输入的字串符赋值给上面的表达式并且不退出？ Allen:是的，每行写一个表达式，回车然后计算，只有输入exit回车才算结束
            {
                try
                {
                    Console.WriteLine(expression + " = " + Calucate(expression));//控制台显示表达式加=加计算结果
                }
                catch (Exception ex)    //这是弹出什么 Allen:没有弹出窗口，只是把异常信息显示到控制台上，这里会俘获所有的异常，也就是说，程序内部抛出的错误都会拦截下来，让程序不结束。关于异常的详细信息，还是需要先读下C# 入门经典里的相关章节
                {
                    Console.WriteLine(ex.Message);
                }
                Console.Write("输入表达式（支持+-*/），输入exit退出: ");
            }
        }
    }
}