using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XOMI
{

    public class SC
        {
        public static void WriteLine(string text)
        {
            MainThreadConsole.AddToDisplayQueue(text);
        }
    }
    public class MainThreadConsole
    {
        public static Queue<string> m_displayQueue = new Queue<string>();

        public static void AddToDisplayQueue(string text)
        {
            m_displayQueue.Enqueue(text);
        }


        public static void Run()
        {
            while (true)
            {
                if (m_displayQueue.Count > 0)
                {
                    string text = m_displayQueue.Dequeue();
                    Console.WriteLine(text);
                }
                else
                {
                    // Sleep for a short time to prevent busy waiting
                    Thread.Sleep(10);
                }
            }
        }
    }
}
