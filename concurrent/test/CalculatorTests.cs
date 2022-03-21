using Microsoft.VisualStudio.TestTools.UnitTesting;
using lib;
using System;

namespace test
{
    [TestClass]
    public class CalculatorTests
    {
        [TestMethod]
        public void Calculator1000Test()
        {
            var c = new Calculator();
            var rng = new Random();
            for(int i = 0; i < 1000; i++)
            {
                double a = rng.NextDouble();
                double b = rng.NextDouble();
                Assert.AreEqual(c.add(a, b), a + b);
                Assert.AreEqual(c.sub(a,b), a - b);
                Assert.AreEqual(c.mul(a,b), a * b);
                Assert.AreEqual(c.div(a,b), a / b);
            }
        }
    }
}