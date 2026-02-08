using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using XOMI.InfoHolder;
using XOMI.Parse;
using XOMI.Static;
using XOMI.TimedAction;
using XOMI.UDP;
using XOMI.UI;
using XOMI.Unstore.Xbox;

namespace XOMI.Unstore
{
    public class ProgramV1_TextVersion
    {
        //➤ ☗ | ↓ ← → ↑ _ ‾ ∨ ∧ ¬ ⊗ ≡ ≤ ≥ ⌃ ⌄ ⊓⇅ ⊔⇵ ⊏ ⊐ ↱↳ ∑ -no unity ⤒ ⤓ ⌈ ⌊ 🀲 🀸 ⌛ ⏰ ▸ ▹ 🐁 🖱 💾



        public static XBoxActionStack m_waitingActions = new XBoxActionStack();
        public static Queue<string> m_udpPackageReceived = new Queue<string>();
        public static UDPTextListenerThread m_udpThread = new UDPTextListenerThread();
        public static XboxSingleControllerExecuter m_xboxExecuter;
        public static void Run(string[] args)
        {

            // To Add later
            //↓ ← → ↑ as array click
            //jl↓ jl← jl→ jl↑ as array click
            //jr↓ jr← jr→ jr↑  jr↖ jr↗ jr↘ jr↙ as array click


            // ←↑→=↓↔=↕↖↗↘↙ 	◰◱◲◳


            for(int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-p" && i + 1 < args.Length)
                {
                    int.TryParse(args[i + 1], out StaticUserPreference.m_port);
                }
                if (args[i] == "-d")
                {
                    StaticUserPreference.m_wantDebugInfo = true;
                }
            }
            StaticVariable.m_debugUserMessage = StaticUserPreference.m_wantDebugInfo;

            ///////////SET THE APP
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;


            //SAY HELLO
            ConsoleUI.DisplayWelcomeMessage();
            LaunchTheUdpThreadListener();
            ConsoleUI.DisplayipAndPortTargets();

            try
            {
                m_xboxExecuter = new XboxSingleControllerExecuter();

            }
            catch (Exception e) {

                ConsoleUI.DrawLine();
                SC.WriteLine("Impossible to create the controller. Error happened:" + e.StackTrace);
                ConsoleUI.DrawLine();
                SC.WriteLine("Make sure you installed ViGEm.");
                SC.WriteLine("Contact me on GitHub or Discord for Help");
            }

            try
            {
              
                TextParseToActions parser = new TextParseToActions(m_waitingActions);
                List<TimedXBoxAction> actionInWaitingToBeExecuted = m_waitingActions.GetRefToActionStack();
                List<TimedXBoxAction> readyToBeExecutedAndRemoved = new List<TimedXBoxAction>();


                while (true)
                {
                    m_udpThread.UpdateTheAutodestructionOfThreadTimer();
                    if (m_udpPackageReceived.Count > 0)
                    {
                        string s = m_udpPackageReceived.Dequeue();
                        //if (StaticVariable.m_debugDevMessage)
                            SC.WriteLine("Received UDP Message:" + s);
                        parser.TryToAppendParseToWaitingActions(s);
                    }
                    CheckForActionToExecute(actionInWaitingToBeExecuted, readyToBeExecutedAndRemoved);

                    bool requestFlush = false;
                    for (int i = 0; i < readyToBeExecutedAndRemoved.Count; i++)
                    {
                      //  SC.WriteLine("E#" + readyToBeExecutedAndRemoved.GetType());
                        if (readyToBeExecutedAndRemoved[i] is TimedXBoxAction_ApplyChange)
                        {
                            TimedXBoxAction_ApplyChange toApply = (TimedXBoxAction_ApplyChange)readyToBeExecutedAndRemoved[i];
                            m_xboxExecuter.Execute(toApply);
                        }
                        if (readyToBeExecutedAndRemoved[i] is TimedXBoxAction_AxisChange)
                        {
                            TimedXBoxAction_AxisChange toApply = (TimedXBoxAction_AxisChange)readyToBeExecutedAndRemoved[i];
                            m_xboxExecuter.Execute(toApply);
                        }
                        if (readyToBeExecutedAndRemoved[i] is TimedXBoxAction_JoysticksChange)
                        {
                            TimedXBoxAction_JoysticksChange toApply = (TimedXBoxAction_JoysticksChange)readyToBeExecutedAndRemoved[i];
                            m_xboxExecuter.Execute(toApply);
                        }
                        if (readyToBeExecutedAndRemoved[i] is TimedXBoxAction_FlushAllCommands)
                        {
                            requestFlush = true;

                        }
                        if (readyToBeExecutedAndRemoved[i] is TimedXBoxAction_Disconnect)
                        {
                            TimedXBoxAction_Disconnect toApply = (TimedXBoxAction_Disconnect)readyToBeExecutedAndRemoved[i];
                            m_xboxExecuter.Execute(toApply);
                        }
                        if (readyToBeExecutedAndRemoved[i] is TimedXBoxAction_Connect)
                        {
                            TimedXBoxAction_Connect toApply = (TimedXBoxAction_Connect)readyToBeExecutedAndRemoved[i];
                            m_xboxExecuter.Execute(toApply);
                        }

                        if (readyToBeExecutedAndRemoved[i] is TimedXBoxAction_ReleaseAll)
                        {

                            TimedXBoxAction_ReleaseAll toApply = (TimedXBoxAction_ReleaseAll)readyToBeExecutedAndRemoved[i];
                            m_xboxExecuter.Execute(toApply);
                        }
                    }
                    if (requestFlush)
                    {
                        SC.WriteLine("Flush Requested, Deleted:" + actionInWaitingToBeExecuted.Count);
                        actionInWaitingToBeExecuted.Clear();
                        requestFlush = false;
                    }



                    Thread.Sleep(1);

                }
            }
            catch (Exception e)
            {
                SC.WriteLine("An exception happen:" + e.StackTrace);
                SC.WriteLine("Contact me if you need help: https://eloistree.page.link/discord");
                SC.WriteLine("Did you install ViGemBus?\n https://github.com/ViGEm/ViGEmBus/releases/tag/v1.21.442.0");
            }
        }

        private static void CheckForActionToExecute(List<TimedXBoxAction> actionInWaitingToBeExecuted, List<TimedXBoxAction> readyToBeExecutedAndRemoved)
        {
            readyToBeExecutedAndRemoved.Clear();
            DateTime now = DateTime.Now;
            for (int i = 0; i < actionInWaitingToBeExecuted.Count; i++)
            {
                if (actionInWaitingToBeExecuted[i].GetWhenToExecute() <= now)
                {
                    readyToBeExecutedAndRemoved.Add(actionInWaitingToBeExecuted[i]);
                }
            }
            for (int i = 0; i < readyToBeExecutedAndRemoved.Count; i++)
            {
                actionInWaitingToBeExecuted.Remove(readyToBeExecutedAndRemoved[i]);

            }
        }

        private static void LaunchTheUdpThreadListener()
        {
            int i = 0;
            int port = StaticUserPreference.m_port;
            bool succeedToCreatePort=false;
            while (!succeedToCreatePort && i<20) {

                SC.WriteLine("Attempt udp connection to " + (port+i));
                if (IsPortOpen(port + i))
                {
                    Thread.Sleep(10);
                    m_udpThread.Launch(ref m_udpPackageReceived, port + i);
                    succeedToCreatePort = true;
                }
                else { 
                    i++;
                }
            }
            StaticUserPreference.m_port = port + i;
        }

        static bool IsPortOpen(int port)
        {
            try
            {
                using (var client = new UdpClient(port))
                {


                    client.Close();
                    client.Dispose();
                    return true;
                }

            }
            catch
            {
                return false;
            }
        }

        private static void  Multi(ref StringBuilder sb,  string text, int count)
        {
            for (int i = 0; i < count; i++)
            {
                sb.Append(text);
            }
        }
    }
}
