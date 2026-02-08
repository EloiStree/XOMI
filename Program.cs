using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using System.Threading;
using XOMI.Unstore;

namespace XOMI
{

    public class Program
    {
        private static Thread workerThread;
        private static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        static void Main(string[] args)
        {
            MainThreadConsole.AddToDisplayQueue("Hello World :) !");
            workerThread = new Thread(() =>
            {
                ProgramV2_FourControllerIntegerVersion.Run(args, cancellationTokenSource.Token);
            });
            workerThread.Start();

            MainThreadConsole.Run();

            cancellationTokenSource.Cancel();
            workerThread.Join();
        }
    }
}
