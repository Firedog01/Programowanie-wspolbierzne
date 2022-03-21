using Microsoft.VisualStudio.TestTools.UnitTesting;
using concurrentLib;

namespace concurrentTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            System.Console.WriteLine(Class1.works());
        }
    }
}