using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Timers;
using XOMI.InfoHolder;
using XOMI.Int1899;
using XOMI.TimedAction;
using XOMI.UDP;
using XOMI.UI;
using XOMI.Unstore.Xbox;

namespace XOMI.Unstore
{
    public class ProgramV2_FourControllerIntegerVersion
    {
        public static XBoxActionStack m_waitingActions = new XBoxActionStack();
        public static UDPTextListenerThread m_udpThread = new UDPTextListenerThread();
        public static XboxSingleControllerExecuter m_xboxExecuter;


        public static List<int> m_banInput = new List<int>();



        //public static IndexIntegerDateQueue m_iidQueue = new IndexIntegerDateQueue();
        public static IntegerToActions [] m_integerToActions = new IntegerToActions[4];
        public static int m_controllerNumber = 4;



        public static void Run(string[] args, CancellationToken cancelToken)
        {

            for (int i = 0; i < args.Length; i++) { 
                if (args[i].Length>0 && args[i][0] == 'b' ){
                    if (int.TryParse( args[i].Replace("b", ""), out int integer)) 
                    {
                        m_banInput.Add(integer);
                        SC.WriteLine("Banned integer: " + integer);
                    }
                }
            }

            string relativeBanPath = "IntBan.txt";
            if (!File.Exists(relativeBanPath)) {
                File.WriteAllText(relativeBanPath,$"{EnumScratchToWarcraftGamepad.PressMenuLeft+0} {EnumScratchToWarcraftGamepad.PressMenuLeft + 1000} {EnumScratchToWarcraftGamepad.PressMenuLeft + 2000}");
            }

            string banIntegersText = File.ReadAllText(relativeBanPath);
            string [] banIntegersToken = banIntegersText.Split(new char[] { ' ', '\n' });
            foreach (string integer in banIntegersToken) {
                if (int.TryParse(integer.Trim(), out int i))
                    m_banInput.Add(i);
            }

            // Remove the Menu Left as it allows to leave path of exil and most game.
            //m_banInput.AddRange(new int[] { 1309 });


            Queue<byte[]> queueBytes = new Queue<byte[]>();
            Queue<string> queueText = new Queue<string>();


            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            //SAY HELLO
            ConsoleUI.DisplayWelcomeMessage();
            UDPIIDListenerThread udpIid = new UDPIIDListenerThread();
            udpIid.Launch(ref queueBytes,3615);
            // I removed the text version for the moment while I am coding the integer version.
            // Text is greate, but I am going in a more row version with an integer index and the logic outside of it.
            // Using text was good to push atomic command, but it is the reason of the D in IID (having a precised NTP action
            // Code to add later.
            // If you add text interpretor in this code you have to do it in all the remote control code.
            // When the interpretor is on the client side, you just need some interpretor on creat.io pip, Nuget for all the RC possible...
            //UDPTextListenerThread udpText = new UDPTextListenerThread();

            //udpText.Launch(ref queueText,3614);
            ConsoleUI.DisplayipAndPortTargets();



            SC.WriteLine("Port Integer: " + 3615);
            SC.WriteLine("Index Mapping: https://github.com/EloiStree/2024_08_29_ScratchToWarcraft.git");

            SC.WriteLine("Did you install ViGemBus?\n https://github.com/ViGEm/ViGEmBus/releases/tag/v1.21.442.0");


            m_integerToActions = new IntegerToActions[4];
            if (m_controllerNumber > 4)
                m_controllerNumber = 4;
            for (int i = 0; i < m_controllerNumber; i++){ 
                m_integerToActions[i]= new IntegerToActions(i + 1, new XboxSingleControllerExecuter());
                Thread.Sleep(100);
            }


            MillisecondsDelayer delayer = new MillisecondsDelayer();

            delayer.m_onExecuteDelayedIndexIntegerAction = ProcessIndexIntegerReceviedWithoutDate;
            delayer.m_onExecuteDelayedIntegerAction = ProcessIndexIntegerReceviedWithoutDate;



            //bool useAllFeatureTest = false;
            //if (useAllFeatureTest) {
            //    foreach (var action in m_integerToActions[0].m_actions)
            //    {
            //        SC.WriteLine("Test:" + action.m_name);
            //        Thread.Sleep(2000);
            //        for (int i = 0; i < 4; i++)
            //        {
            //            m_integerToActions[i].FetchAndApply(action.m_pressInteger);
            //            Thread.Sleep(500);
            //        }
            //        Thread.Sleep(2000);
            //        for (int i = 0; i < 4; i++)
            //        {
            //            m_integerToActions[i].FetchAndApply(action.m_releaseInteger);
            //            Thread.Sleep(500);

            //        }

            //        Thread.Sleep(1000);
            //        for (int i = 0; i < 4; i++)
            //        {
            //            m_integerToActions[i].m_executer.Execute(new TimedXBoxAction_ReleaseAll(DateTime.Now));

            //        }

            //    }
            //}


            foreach (int b in banIntegersText)
            {
                SC.WriteLine("Ban Input:"+b);
            }

            SC.WriteLine("Ready to loop for udp action.");
            while (true)
            {
               // udpText.UpdateTheAutodestructionOfThreadTimer();
                udpIid.UpdateTheAutodestructionOfThreadTimer();
                while (queueBytes.Count > 0) { 
                
                    byte[] bytes = queueBytes.Dequeue();
                    SC.WriteLine($"{bytes.Length}: {bytes}");
                    if (bytes.Length == 4)
                    {
                        int integer = BitConverter.ToInt32(bytes, 0);
                        ProcessIndexIntegerReceviedWithoutDate(integer);
                    }
                    else if (bytes.Length == 8) { 
                    
                        int index = BitConverter.ToInt32(bytes, 0);
                        int value = BitConverter.ToInt32(bytes, 4);
                        ProcessIndexIntegerReceviedWithoutDate(index, value);
                    }
                    else if (bytes.Length == 16)
                    {

                        int index = BitConverter.ToInt32(bytes, 0);
                        int value = BitConverter.ToInt32(bytes, 4);


                        // NEED TO BE REFACTORED BECAUSE WE NEED NTP DATA UTC TO WORKS.
                        // LEARNED FROM UNTIY NOT APPLY HERE YET.
                        ulong time = BitConverter.ToUInt64(bytes, 8);
                        if (time < 1000 * 3600 * 25)
                        {
                            delayer.AppendDelayedAction(index, value, time);
                        }
                        else { 

                            ProcessIndexIntegerReceviedWithoutDate(index, value);
                        }
                    }
                    else if (bytes.Length == 12)
                    {
                        int value = BitConverter.ToInt32(bytes, 0);
                        // NEED TO BE REFACTORED BECAUSE WE NEED NTP DATA UTC TO WORKS.
                        // LEARNED FROM UNTIY NOT APPLY HERE YET.
                        ulong time = BitConverter.ToUInt64(bytes, 4);
                        if (time < 1000 * 3600 * 25)
                        {
                            delayer.AppendDelayedAction( value, time);
                        }
                        else
                        {
                        ProcessIndexIntegerReceviedWithoutDate(value);

                        }
                    }
                    else if (bytes.Length> 16  ) {

                        // Cut the block in to piece
                        for (int i = 0; i < bytes.Length; i+=16) { 
                            int index = BitConverter.ToInt32(bytes, i);
                            int value = BitConverter.ToInt32(bytes, i+4);
                            ulong time = BitConverter.ToUInt64(bytes, i+8);
                            SC.WriteLine($"Debug Received Udp Block: index {index} value {value} time {time}");
                            if (time < 1000*3600*25) { 
                                DateTime date = DateTime.UtcNow.AddMilliseconds(time);
                                delayer.AppendDelayedAction( index, value, time);
                            }
                            else
                            {
                                // NEED TO BE REFACTORED BECAUSE WE NEED NTP DATA UTC TO WORKS.
                                // LEARNED FROM UNTIY NOT APPLY HERE YET.
                                ProcessIndexIntegerReceviedWithoutDate(index, value);
                            }
                        }
                    }
                }

                delayer.CheckForActionToExecute();
                Thread.Sleep(1);
            }

        }


        public static void ProcessIndexIntegerReceviedWithoutDate(int index, int value)
        {
            if (value < 1000000)
            {
                
                ProcessValueAsScratchToWarcraftButton(index, value);

            }
            else if (value > 1700000000)
            {
                // That a XOMI 1899999999 action or 1700000000
                CheckForGamepadAction(index, value);

            }
            else {

                Int1899Parser.GetValue999999(value, out int value999999);
                Int1899Parser.GetPlayerId(value, out byte playerId);
                Int1899Parser.GetTag99(value, out byte tag99);
                if (tag99 == 19)
                {

                    ProcessValueAsScratchToWarcraftButton(playerId, value999999);
                }
                else {


                    CheckForGamepadAction( index, value);
                }

            }

          

        }

        private static void CheckForGamepadAction(int index, int value)
        {
            CheckForGamepadAction(m_integerToActions, index, value, DateTime.UtcNow);
        }

        private static void ProcessValueAsScratchToWarcraftButton(int playerId, int value999999)
        {
            if (IsBanGamepadButton(value999999)) { 
                SC.WriteLine($"Ban Gamepad Button: {value999999} for player {playerId}");
                return;
            }
            int indexGamepad = playerId;
            if (indexGamepad > 4)
                indexGamepad = 0;

            for (int selectGamepad = 0; selectGamepad < 4; selectGamepad++) {
                int gamepadNumber = selectGamepad + 1;
                IntegerToActions focusGamepad = m_integerToActions[selectGamepad];
                if (indexGamepad == 0 || indexGamepad == gamepadNumber) {

                    focusGamepad.FetchAndApply(value999999);
                }
            }

        }


        public static void ProcessIndexIntegerReceviedWithoutDate(int value)
        {
            ProcessIndexIntegerReceviedWithoutDate(0, value);

        }


        public static void CheckForGamepadAction(IntegerToActions[] integerToActions, int index, int value, DateTime utcNow)
        {
            int absolutValue = value < 0 ? -value : value;
            if (value >= 1700000000)
            {

                int tag = value / 100000000;
                if (tag == 18)
                {


                    int integer = value % 100000000;
                    int value99000000LX = integer / 1000000 % 100;
                    int value00990000LY = integer / 10000 % 100;
                    int value00009900RX = integer / 100 % 100;
                    int value00000099RY = integer % 100;
                    Convert01To99ToPercent(value99000000LX, out float percent99000000LX);
                    Convert01To99ToPercent(value00990000LY, out float percent00990000LY);
                    Convert01To99ToPercent(value00009900RX, out float percent00009900RX);
                    Convert01To99ToPercent(value00000099RY, out float percent00000099RY);

                TimedXBoxAction_DoubleJoysticksChange doubleJoystick =
                                        new TimedXBoxAction_DoubleJoysticksChange(utcNow,
                                        percent99000000LX,
                                        percent00990000LY,
                                        percent00009900RX,
                                        percent00000099RY);
                    int indexGamepad = index;
                    if (indexGamepad > 4)
                        indexGamepad = 0;
                    for (int selectGamepad = 0; selectGamepad < 4; selectGamepad++)
                    {
                        int gamepadNumber = selectGamepad + 1;
                        if (indexGamepad == 0 || indexGamepad == gamepadNumber)
                        {
                            IntegerToActions focusGamepad = m_integerToActions[selectGamepad];
                            integerToActions[selectGamepad].m_executer.Execute(doubleJoystick);
                            // Print a debug of the doubleJoystick

                            //SC.WriteLine($"Debug Double Joystick: {gamepadNumber} {percent99000000LX} {percent00990000LY} {percent00009900RX} {percent00000099RY}");


                        }
                    }
                }



            }
       
            else if (absolutValue >= 1000000)

            { 
                List<TimedXBoxAction> actions = new List<TimedXBoxAction>();
                
                Int1899Parser.GetValue999999(value, out int value999999);
                Int1899Parser.GetPlayerId(value, out byte playerId);
                Int1899Parser.GetTag99(value, out byte tag);

                if(playerId>4)
                {
                    playerId = 0;
                }

                bool signed = value < 0;
                if (signed) {
                    value999999 = -value999999;
                }

                if (tag == 19) {
                    // Scratch to warcraft commande

                    ProcessValueAsScratchToWarcraftButton(playerId, value999999);
                    SC.WriteLine($"Player {playerId} Scratch to Warcraft Command: {value999999}");
                }
                else if (tag == 20)
                {
                    // Is  axis  9 9 9 9  trigger 9 9 

                    byte triggerRight = (byte)(value999999 % 10);
                    byte triggerLeft = (byte)((value999999 / 10) % 10);
                    byte rightAxisY = (byte)((value999999 / 100) % 10);
                    byte rightAxisX = (byte)((value999999 / 1000) % 10);
                    byte leftAxisY = (byte)((value999999 / 10000) % 10);
                    byte leftAxisX = (byte)((value999999 / 100000) % 10);

                    SC.WriteLine($"Player {playerId} Trigger:  {triggerLeft} {triggerRight} {leftAxisX} {leftAxisY} Axis 9 9 9 9: {rightAxisX} {rightAxisY}");

                    Pourcent01From9(triggerLeft, out float percentTriggerLeft);
                    Pourcent01From9(triggerRight, out float percentTriggerRight);
                    Pourcent11From9(leftAxisX, out float percentLeftAxisX);
                    Pourcent11From9(leftAxisY, out float percentLeftAxisY);
                    Pourcent11From9(rightAxisX, out float percentRightAxisX);
                    Pourcent11From9(rightAxisY, out float percentRightAxisY);

                    DateTime dateNow = DateTime.UtcNow;
                    TimedXBoxAction_AxisChange triggerAction =
                        new TimedXBoxAction_AxisChange(dateNow,
                        XBoxAxisInputType.TriggerLeft, percentTriggerLeft);
                    TimedXBoxAction_AxisChange triggerActionRight =
                        new TimedXBoxAction_AxisChange(dateNow,
                        XBoxAxisInputType.TriggerRight, percentTriggerRight);
                    TimedXBoxAction_AxisChange leftAxisXAction =
                        new TimedXBoxAction_AxisChange(dateNow,
                        XBoxAxisInputType.JoystickLeft_Left2Right, percentLeftAxisX);
                    TimedXBoxAction_AxisChange leftAxisYAction =
                        new TimedXBoxAction_AxisChange(dateNow,
                        XBoxAxisInputType.JoystickLeft_Down2Up, percentLeftAxisY);
                    TimedXBoxAction_AxisChange rightAxisXAction =
                        new TimedXBoxAction_AxisChange(dateNow,
                        XBoxAxisInputType.JoystickRight_Left2Right, percentRightAxisX);
                    TimedXBoxAction_AxisChange rightAxisYAction =        
                        new TimedXBoxAction_AxisChange(dateNow,
                        XBoxAxisInputType.JoystickRight_Down2Up, percentRightAxisY);


                    actions.Add(triggerAction);
                    actions.Add(triggerActionRight);
                    actions.Add(leftAxisXAction);
                    actions.Add(leftAxisYAction);
                    actions.Add(rightAxisXAction);
                    actions.Add(rightAxisYAction);


                }
                else if (tag == 21)
                {
                    //left xy
                    PourcentDouble999(value999999, out float percentLeftX, out float percentLeftY);
                    actions.Add(new TimedXBoxAction_JoysticksChange(utcNow, XBoxJoystickInputType.JoystickLeft, percentLeftX, percentLeftY));
                }
                else if (tag == 22) {

                    //right xy

                    PourcentDouble999(value999999, out float percentRightX, out float percenRightY);
                    actions.Add(new TimedXBoxAction_JoysticksChange(utcNow, XBoxJoystickInputType.JoystickRight, percentRightX, percenRightY));

                }
                else if (tag == 23)
                {
                    //left x
                    float percentLeftX = 0.0f;
                    Pourcent11From999999(value999999, out percentLeftX);
                    SC.WriteLine($"Player {playerId} Joystick Left X: {percentLeftX}");
                    actions.Add(new TimedXBoxAction_AxisChange(utcNow, XBoxAxisInputType.JoystickLeft_Left2Right, percentLeftX));
                }
                else if (tag == 24)
                {
                    //right y
                    float percentLeftY = 0.0f;
                    Pourcent11From999999(value999999, out percentLeftY);
                    SC.WriteLine($"Player {playerId} Joystick Left Y: {percentLeftY}");
                    actions.Add(new TimedXBoxAction_AxisChange(utcNow, XBoxAxisInputType.JoystickLeft_Down2Up, percentLeftY));

                }
                else if (tag == 25)
                {
                    //right x
                    float percentRightX = 0.0f;
                    Pourcent11From999999(value999999, out percentRightX);
                    actions.Add(new TimedXBoxAction_AxisChange(utcNow, XBoxAxisInputType.JoystickRight_Left2Right, percentRightX));
                }
                else if (tag == 26)
                {

                    //right y
                    float percentRightY = 0.0f;
                    Pourcent11From999999(value999999, out percentRightY);
                    actions.Add(new TimedXBoxAction_AxisChange(utcNow, XBoxAxisInputType.JoystickRight_Down2Up, percentRightY));
                }
                if (tag == 27)
                {
                    //trigger y
                    float percentTriggerLeft = 0.0f;
                    Pourcent01From999999(value999999, out percentTriggerLeft);
                    actions.Add(new TimedXBoxAction_AxisChange(utcNow, XBoxAxisInputType. TriggerLeft, percentTriggerLeft));

                }
                else if (tag == 28)
                {
                    //trigger x
                    float percentTriggerRight = 0.0f;
                    Pourcent01From999999(value999999, out percentTriggerRight);
                    actions.Add(new TimedXBoxAction_AxisChange(utcNow, XBoxAxisInputType.TriggerRight, percentTriggerRight));
                }



                for (int xbox = 0; xbox < 4; xbox++) {
                    if (xbox >= integerToActions.Length)
                    {
                        continue;
                    }

                    if (integerToActions[xbox] != null)
                    {
                        if (playerId == 0 || playerId == xbox + 1)
                        {
                            foreach (TimedXBoxAction action in actions)
                            {
                                integerToActions[xbox].m_executer.Execute(action);
                            }
                        }
                    }
                }





            }
        
        }

        public static void PourcentDouble999(int value999999, out float percentLeftX, out float percentLeftY)
        {
            int partLeft = value999999 / 1000;
            partLeft = partLeft % 1000;
            int partRight = value999999 % 1000;


            if(partLeft == 0 )
            {
                percentLeftX = 0.0f;
            }
            else
            {
                percentLeftX = (((partLeft-1) / 998f) - 0.5f) * 2f;
            }

            if (partRight == 0)
            {
                percentLeftY = 0.0f;
            }
            else
            {
                percentLeftY = (((partRight - 1) / 998f) - 0.5f) * 2f;
            }
        }

        private static void Pourcent01From999999(int value, out float percent01)
        {
            if (value == 0)
                percent01 = 0;
            else
                percent01 = Math.Clamp(value / 999998f, 0f, 1f);
        }

        private static void Pourcent11From999999(int value, out float percent11)
        {
            if (value == 0)
                percent11 = 0;
            else 
                percent11 = Math.Clamp((value / 999998f - 0.5f) * 2f, -1f, 1f);
        }

        private static void Pourcent11From9(byte leftAxisX, out float percent1To1)
        {
            if (leftAxisX == 0)
            {
                percent1To1 = 0.0f;
            }
            else
            {
                percent1To1 = ((leftAxisX - 1f) / 8f - 0.5f) * 2f;
            }
        }

        private static void Pourcent01From9(byte triggerLeft, out float percent0To1)
        {
            
            if (triggerLeft == 0)
            {
                percent0To1 = 0.0f;
            }
            else
            {
                percent0To1 = ((triggerLeft - 1f) / 8f );
            }
        }

        private static void Convert01To99ToPercent(int intValue99, out float percent)
        {
            if (intValue99 == 0)
            {
                percent = 0.0f;
            }
            else {
                percent = ((intValue99 - 1) / 98f-0.5f)*2f;
            }
        }

        public static bool FlushTimedActionIf1256(int integer, ref Queue<byte[]> queue)
        {
            if (integer == 1256 || integer == 2256) { 
                queue.Clear();
                return true;
            }
            return false;
        }

        public static bool IsNotBanGamepadButton(int integer)
        {
            return true;
            // DISABLE THE TIME OF A BUG CORRECTION
            // return !m_banInput.Contains(integer);
        }
        public static bool IsBanGamepadButton(int integer)
        {
            return false;
            // DISABLE THE TIME OF A BUG CORRECTION
            // return m_banInput.Contains(integer);
        }

    }
}
