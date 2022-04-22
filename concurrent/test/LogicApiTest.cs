using Microsoft.VisualStudio.TestTools.UnitTesting;
using logic;
using logic.Event;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics; // Process

namespace test
{
    [TestClass]
    public class LogicApiTest
    {
        private TextWriterTraceListener myListener;
        private int threadWaitTime = 100;
        public LogicApiTest()
        {
            // file located in concurrent\test\bin\Debug\net6.0\ApiTest.log
            myListener = new TextWriterTraceListener("ApiTest.log", "myListener");
            myListener.WriteLine("================ Beginning of test class at: " + DateTime.Now.ToString("MM-dd hh:mm:ss"));
            myListener.Flush();
            LogicApi.Implementation.PosUpdated += new MarbleEventHandler(PrintMarblesPos);
        }

        ~LogicApiTest()
        {
            myListener.Flush();
        }

        [TestMethod]
        public void ApiListenerTest()
        {
            myListener.WriteLine("ApiListenerTest ------------------------");
            LogicApi.Implementation.CreateUnmovingMarble();
            myListener.WriteLine("created one marble.");
            
            myListener.WriteLine("added handler to event.");

            myListener.WriteLine("starting to sleep on main thread.");
            Thread.Sleep(threadWaitTime);

            myListener.WriteLine("deleted one marble.");
            LogicApi.Implementation.DeleteMarble(0);
            myListener.WriteLine("end ApiListenerTest ------------------------");
            myListener.Flush();
        }

        [TestMethod]
        public void CreateDeleteTest()
        {
            myListener.WriteLine("CreateDeleteTest ------------------------");
            myListener.Flush();
            int a;

            for (int i = 0; i < 3; i++)
            {
                a = LogicApi.Implementation.MarbleCount;
                Assert.AreEqual(i, a);
                LogicApi.Implementation.CreateUnmovingMarble();
                Thread.Sleep(threadWaitTime);
            }

            for (int i = 0; i < 3; i++)
                LogicApi.Implementation.DeleteMarble(0);
            myListener.WriteLine("deleted three marbles.");
            myListener.WriteLine("end CreateDeleteTest ------------------------");
            myListener.Flush();
        }

        [TestMethod]
        public void CreateMovingTest()
        {
            LogicApi.Implementation.CreateMovingMarble();
            LogicApi.Implementation.Start();
            Thread.Sleep(threadWaitTime);
        }

        

        public void PrintMarblesPos(object source, MarbleArgs args)
        {
            myListener.WriteLine("PosUpdated:");
            foreach(MarbleInfo arg in args.InfoArr)
            {
                string s = "x: " + arg.X;
                s += ", y: " + arg.Y;
                s += ", idx: " + arg.Index;
                myListener.WriteLine(s);
            }
            myListener.Flush();
        }
    }
}