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
    }
}
