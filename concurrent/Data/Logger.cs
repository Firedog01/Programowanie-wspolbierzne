using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace Data
{
    internal class Logger
    {
        private static List<Marble> marbles;
        private Stopwatch watch = new Stopwatch();
        public Logger(List<Marble> m) {
            marbles = m;
            Thread t = new Thread(() =>
            {
                watch.Start();
                while (true)
                {
                    if (watch.ElapsedMilliseconds >= 5)
                    {
                        watch.Restart();
                        using (StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + "\\log.txt", true))
                        {
                            string stamp = ($"Log started {DateTime.Now:o}");
                            foreach (Marble marble in marbles)
                            {
                                writer.WriteLine(stamp + JsonSerializer.Serialize(marble));
                            }
                        }
                    }
                }
            })
            {
                IsBackground = true
            };
            t.Start();
        }

        public void stop() {
            watch.Reset();
            watch.Stop();
        }
    }
}
