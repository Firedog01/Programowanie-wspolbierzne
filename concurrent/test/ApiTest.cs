using Microsoft.VisualStudio.TestTools.UnitTesting;
using logic;
using System;

namespace test
{
    [TestClass]
    public class ApiTest
    {
        [TestMethod]
        public void CreateUnmovingMarbleTest()
        {
            int a = Api.Instance.MarbleCount;
            Assert.AreEqual(0, a);
            Api.Instance.CreateUnmovingMarble();
            Api.Instance.CreateUnmovingMarble();
            a = Api.Instance.MarbleCount;
            Assert.AreEqual(2, a);
        }
    }
}