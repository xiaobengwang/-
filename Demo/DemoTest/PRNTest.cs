using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace DemoTest
{
    /// <summary>
    /// 逆序表达式转换单元测试
    /// </summary>
    [TestClass]
    public class PrnTest
    {
        [TestMethod]
        public void Simple()
        {
            var result = Calculate.Calculator.PreOrderToPostOrder("5+((1+2)*4)-3");
            var prnExpress = new StringBuilder();
            foreach (var o in result)
            {
                prnExpress.Append(o);
            }
            Assert.AreEqual("512+4*+3-", prnExpress.ToString());
        }

        [TestMethod]
        public void ReflectionTest()
        {

            var className = "DemoTest.Math";//类全名

            var t = Type.GetType(className);//反射获取类型

            var obj = Activator.CreateInstance(t);//反射创建实例

            var m = t.GetMethod("Add"); //反射获取Add方法

            var returnValue = m.Invoke(obj, new object[] { 1, 2 });//反射执行方法

            Assert.AreEqual(3, returnValue);//比较结果是否符合预期，这个是单元测试的部分

        }
    }

    public class Math
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
    }

}
