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
    public class ApiTest
    {
        private TextWriterTraceListener myListener;
        private int threadWaitTime = 400;
        public ApiTest()
        {
            // file located in concurrent\test\bin\Debug\net6.0\ApiTest.log
            myListener = new TextWriterTraceListener("ApiTest.log", "myListener");
            myListener.WriteLine("================ Beginning of test class at: " + DateTime.Now.ToString("MM-dd hh:mm:ss"));
            myListener.Flush();
            Api.Instance.onPosUpdated += new MarbleEventHandler(PrintMarblesPos);
        }

        ~ApiTest()
        {
            myListener.Flush();
        }

        [TestMethod]
        public void ApiListenerTest()
        {
            myListener.WriteLine("ApiListenerTest ------------------------");
            Api.Instance.CreateUnmovingMarble();
            myListener.WriteLine("created one marble.");
            
            myListener.WriteLine("added handler to event.");

            myListener.WriteLine("starting to sleep on main thread.");
            Thread.Sleep(threadWaitTime);

            myListener.WriteLine("deleted one marble.");
            Api.Instance.DeleteMarble(0);
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
                a = Api.Instance.MarbleCount;
                Assert.AreEqual(i, a);
                Api.Instance.CreateUnmovingMarble();
                Thread.Sleep(threadWaitTime);
            }

            for (int i = 0; i < 3; i++)
                Api.Instance.DeleteMarble(0);
            myListener.WriteLine("deleted three marbles.");
            myListener.WriteLine("end CreateDeleteTest ------------------------");
            myListener.Flush();
        }

        [TestMethod]
        public void CreateMovingTest()
        {
            Api.Instance.CreateMovingMarble();
            Api.Instance.Start();
            Thread.Sleep(threadWaitTime);
        }

        

        public void PrintMarblesPos(object source, MarbleArgs args)
        {
            myListener.WriteLine("onPosUpdated:");
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