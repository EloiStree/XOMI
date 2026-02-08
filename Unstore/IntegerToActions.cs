using System;
using System.Collections.Generic;
using XOMI;
using XOMI.TimedAction;
using XOMI.Unstore.Xbox;

public class IntegerToActions {
    public int m_integerIndex= 0;
    public bool m_useDebugConsole = true;
    public void FetchAndApply(int value) { 
    
       
        for (int i = 0;i < m_actions.Count ;i++) {
            if (m_actions[i].IsPressing(value)) { 
                m_actions[i].Press();
                if(m_useDebugConsole)
                    SC.WriteLine($"Pressed ({value}):{m_actions[i].GetDescription()}");
                return;
            }
            if (m_actions[i].IsReleasing(value)) { 
                m_actions[i].Release();
                if (m_useDebugConsole)
                    SC.WriteLine($"Release ({value}):{m_actions[i].GetDescription()}");
                return;
            }
        }


        

        if (m_useDebugConsole)
            SC.WriteLine($"No action for ({value})");
    }

    public class PressReleaseIntegerAction {
        public string m_name;
        public int m_pressInteger;
        public int m_releaseInteger;
        public Action m_pressAction;
        public Action m_releaseAction;
        public PressReleaseIntegerAction(string name, int press, int release, Action pressAction, Action releaseAction) {
            m_name = name;
            m_pressInteger = press;
            m_releaseInteger = release;
            m_pressAction = pressAction;
            m_releaseAction = releaseAction;
        }

        public bool IsPressing(int value) {
            return m_pressInteger ==value;
        }
        public bool IsReleasing(int value)
        {
            return m_releaseInteger == value;
        }

        public void Press() {
            if(m_pressAction!=null)
                m_pressAction();
        }
        public void Release()
        {
            if (m_releaseAction != null)
                m_releaseAction();
        }   
        public string GetDescription() {
            return m_name;
        }
    }

    public List<PressReleaseIntegerAction> m_actions = new List<PressReleaseIntegerAction>();


    public void Add(string name, int press, Action pressAction, Action releaseAction)
    {
        m_actions.Add(new PressReleaseIntegerAction(name, press, press + 1000, pressAction, releaseAction));
    }
    public void Add(string name, int press, int release, Action pressAction, Action releaseAction)
    {
        m_actions.Add(new PressReleaseIntegerAction(name, press, release, pressAction, releaseAction));
    }
    public int m_index = 0;
    public XboxSingleControllerExecuter m_executer = null;

    public DateTime Now() { return DateTime.Now; }

    public float RandomFloat11() { return (float)(new Random().NextDouble() * 2 - 1); }

   

    public IntegerToActions(int index, XboxSingleControllerExecuter executor)
    {
        m_index = index;
        m_executer = executor;




        Add("Random input for all gamepads, no menu", 1399, 2399
            , () => {


                m_executer.Randomize_AllButMenu();
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_ReleaseAll(Now()));
            }
        );
        Add("Enable hardware joystick ON/OFF", 1390, 2390
            , () => { }
            , () => { }
        );

        Add("Release All Button", 1390, 2390,
            () =>
            {
                m_executer.Execute(new TimedXBoxAction_ReleaseAll(Now()));
            },
            () =>
            {
                m_executer.Execute(new TimedXBoxAction_ReleaseAll(Now()));
            }
         );
        Add("Release All Button But Menu", 1391, 2391,
            () =>
            {
                m_executer.Execute(new TimedXBoxAction_ReleaseAllButMenu(Now()));
            },
            () =>
            {
                m_executer.Execute(new TimedXBoxAction_ReleaseAllButMenu(Now()));
            }
         );
        Add("Press A button", 1300, 2300
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.ButtonDown)); }
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.ButtonDown)); }
        );
        Add("Press X button", 1301, 2301
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.ButtonLeft)); }
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.ButtonLeft)); }
        );
        Add("Press B button", 1302, 2302
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.ButtonRight)); }
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.ButtonRight)); }

        );
        Add("Press Y button", 1303, 2303
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.ButtonUp)); }
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.ButtonUp)); }

        );
        Add("Press left side button", 1304, 2304
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.SideButtonLeft)); }
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.SideButtonLeft)); }
);
        Add("Press right side button", 1305, 2305
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.SideButtonRight)); }
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.SideButtonRight)); }

        );
        Add("Press left stick", 1306, 2306
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.JoystickLeftButton)); }
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.JoystickLeftButton)); }

        );
        Add("Press right stick", 1307, 2307
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.JoystickRightButton)); }
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.JoystickRightButton)); }

        );
        Add("Press menu right", 1308, 2308
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType. MenuRight)); }
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType. MenuRight )); }

        );
        Add("Press menu left", 1309, 2309
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.MenuLeft)); }
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.MenuLeft)); }

        );
        Add("Release D-pad", 1310, 2310
            , () => { 
            
                m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.ArrowLeft));
                m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.ArrowRight));
                m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.ArrowDown));
                m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.ArrowUp));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.ArrowLeft));
                m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.ArrowRight));
                m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.ArrowDown));
                m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.ArrowUp));
            }
        );
        Add("Press arrow north", 1311, 2311
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.ArrowUp)); }
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.ArrowUp)); }

        );
        Add("Press arrow northeast", 1312, 2312
            , () => {
            
                m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.ArrowRight));
                m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.ArrowUp));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.ArrowRight));
                m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.ArrowUp));
            }
        );
        Add("Press arrow east", 1313, 2313
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.ArrowRight)); }
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.ArrowRight)); }

        );
        Add("Press arrow southeast", 1314, 2314
            , () => { 
            
                m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.ArrowRight));
                m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.ArrowDown));
            }
            , () => {

                m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.ArrowRight));
                m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.ArrowDown));
            }
        );
        Add("Press arrow south", 1315, 2315
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.ArrowDown)); }
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.ArrowDown)); }

        );
        Add("Press arrow southwest", 1316, 2316
            , () => {
                m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.ArrowLeft));
                    m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.ArrowDown));

            }
            , () => {

                m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.ArrowLeft));
                m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.ArrowDown));
            }
        );
        Add("Press arrow west", 1317, 2317
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.ArrowLeft)); }
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.ArrowLeft)); }

        );
        Add("Press arrow northwest", 1318, 2318
            , () => {
                m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.ArrowLeft));
                m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.ArrowUp));

            }
            , () => {

                m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.ArrowLeft));
                m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.ArrowUp));
            }
        );
        Add("Press Xbox home button", 1319, 2319
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Press, XOMI.XBoxInputType.XboxButton)); }
            , () => { m_executer.Execute(new TimedXBoxAction_ApplyChange(Now(), XOMI.PressType.Release, XOMI.XBoxInputType.XboxButton)); }
        );
        Add("Random axis", 1320, 2320
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickLeft, RandomFloat11(), RandomFloat11()));
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickRight, RandomFloat11(), RandomFloat11()));

            }
            , () => {

                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickLeft, 0,0));
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickRight, 0,0));
            }
        );
        Add("Start recording", 1321, 2321
            , () => {  }
            , () => {  }
        );
        Add("Set left stick to neutral(clockwise)  ", 1330, 2330

            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickLeft, 0, 0));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickLeft, 0, 0));
            }
        );
        Add("Move left stick up", 1331, 2331
            , () => { 
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickLeft,0, 1));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickLeft, 0, 0));
            }
        );
        Add("Move left stick up-right", 1332, 2332
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickLeft,1,1 ));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickLeft,0,0 ));
            }
        );
        Add("Move left stick right", 1333, 2333
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickLeft, 1, 0));

            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickLeft, 0, 0));
            }
        );
        Add("Move left stick down-right", 1334, 2334
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickLeft, 1, -1));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickLeft, 0, 0));
            }
        );
        Add("Move left stick down", 1335, 2335
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickLeft, 0, -1));

            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickLeft, 0,0));

            }
        );
        Add("Move left stick down-left", 1336, 2336
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickLeft, -1,-1));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickLeft, 0, 0));
            }
        );
        Add("Move left stick left", 1337, 2337
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickLeft, -1, 0));

            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickLeft, 0, 0));
            }
        );
        Add("Move left stick up-left", 1338, 2338
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickLeft, -1, 1));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickLeft, 0, 0));
            }
        );
        Add("Set right stick to neutral(clockwise)",1340, 2340
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickRight, 0, 0));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickRight, 0, 0));
            }
        );
        Add("Move right stick up", 1341, 2341
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickRight, 0, 1));

            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickRight, 0, 0));
            }
        );
        Add("Move right stick up-right", 1342, 2342
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickRight, 1, 1));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickRight, 0, 0));
            }
        );
        Add("Move right stick right", 1343, 2343
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickRight, 1, 0));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickRight, 0,0));
            }
        );
        Add("Move right stick down-right", 1344, 2344
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickRight, 1, -1));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickRight, 0, 0));
            }
        );
        Add("Move right stick down", 1345, 2345
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickRight, 0, -1));

            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickRight, 0, 0));

            }
        );
        Add("Move right stick down-left", 1346, 2346
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickRight, -1, -1));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickRight, 0, 0));
            }
        );
        Add("Move right stick left", 1347, 2347
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickRight, -1, 0));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickRight, 0, 0));
            }
        );
        Add("Move right stick up-left", 1348, 2348
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickRight, -1, 1));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_JoysticksChange(Now(), XOMI.XBoxJoystickInputType.JoystickRight, 0, 0));
            }
        );
        Add("Set left stick horizontal to 1.0", 1350, 2350
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Left2Right, 1));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Left2Right, 0));
            }
        );
        Add("Set left stick horizontal to -1.0", 1351, 2351
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Left2Right, -1));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Left2Right, 0));
            }
        );
        Add("Set left stick vertical to 1.0", 1352, 2352
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Down2Up, 1));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Down2Up, 0));
            }
        );
        Add("Set left stick vertical to -1.0", 1353, 2353
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Down2Up, -1));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Down2Up, 0));
            }
        );
        Add("Set right stick horizontal to 1.0", 1354, 2354
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Left2Right, 1));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Left2Right, 0));
            }
        );
        Add("Set right stick horizontal to -1.0", 1355, 2355
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Left2Right, -1));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Left2Right, 0));
            }
        );
        Add("Set right stick vertical to 1.0", 1356, 2356
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Down2Up, 1));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Down2Up, 0));
            }
        );
        Add("Set right stick vertical to -1.0", 1357, 2357
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Down2Up, -1));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Down2Up, 0));
            }
        );
        Add("Set left trigger to 100%", 1358, 2358
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.TriggerLeft, 1));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.TriggerLeft, 0));
            }
        );
        Add("Set right trigger to 100%", 1359, 2359
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.TriggerRight, 1));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.TriggerRight, 0));
            }
        );
        Add("Set left stick horizontal to 0.75", 1360, 2360
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Left2Right, 0.75f));
            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Left2Right, 0));
            }
        );
        Add("Set left stick horizontal to -0.75", 1361, 2361
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Left2Right, -0.75f));
            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Left2Right, 0));
            }
        );
        Add("Set left stick vertical to 0.75", 1362, 2362
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Down2Up, 0.75f));
            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Down2Up, 0));
            }
        );
        Add("Set left stick vertical to -0.75", 1363, 2363
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Down2Up, -0.75f));
            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Down2Up, 0));
            }
        );
        Add("Set right stick horizontal to 0.75", 1364, 2364
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Left2Right, 0.75f));
                
            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Left2Right, 0));
            }
        );
        Add("Set right stick horizontal to -0.75", 1365, 2365
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Left2Right, -0.75f));
                
            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Left2Right, 0));
            }
        );
        Add("Set right stick vertical to 0.75", 1366, 2366
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Down2Up, 0.75f));
                
            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Down2Up, 0));
            }
        );
        Add("Set right stick vertical to -0.75", 1367, 2367
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Down2Up, -0.75f));
                
            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Down2Up, 0));
            }
        );
        Add("Set left trigger to 75%", 1368, 2368
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.TriggerLeft, 0.75f));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.TriggerLeft, 0));
            }
        );
        Add("Set right trigger to 75%", 1369, 2369
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.TriggerRight, 0.75f));
                
            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.TriggerRight, 0));
            }
        );
        Add("Set left stick horizontal to 0.5", 1370, 2370
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Left2Right, 0.5f));
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Left2Right, 0));
                
            }
        );
        Add("Set left stick horizontal to -0.5", 1371, 2371
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Left2Right, -0.5f));
                
            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Left2Right, 0));
            }
        );
        Add("Set left stick vertical to 0.5", 1372, 2372
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Down2Up, 0.5f));
                
            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Down2Up, 0));
            }
        );
        Add("Set left stick vertical to -0.5", 1373, 2373
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Down2Up, -0.5f));
            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Down2Up, 0));
            }
        );
        Add("Set right stick horizontal to 0.5", 1374, 2374
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Left2Right, 0.5f));
            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Left2Right, 0));
            }
        );
        Add("Set right stick horizontal to -0.5", 1375, 2375
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Left2Right, -0.5f));
            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Left2Right, 0));
            }
        );
        Add("Set right stick vertical to 0.5", 1376, 2376
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Down2Up, 0.5f));
            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Down2Up, 0));
            }
        );
        Add("Set right stick vertical to -0.5", 1377, 2377
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Down2Up, -0.5f));
                
            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Down2Up, 0));
            }
        );
        Add("Set left trigger to 50%", 1378, 2378
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.TriggerLeft, 0.5f));
            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.TriggerLeft, 0));
            }
        );
        Add("Set right trigger to 50%", 1379, 2379
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.TriggerRight, 0.5f));
            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.TriggerRight, 0));
            }
        );
        Add("Set left stick horizontal to 0.25", 1380, 2380
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Left2Right, 0.25f));
            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Left2Right, 0));
            }
        );
        Add("Set left stick horizontal to -0.25", 1381, 2381
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Left2Right, -0.25f));
            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Left2Right, 0));
            }
        );
        Add("Set left stick vertical to 0.25", 1382, 2382
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Down2Up, 0.25f));
            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Down2Up, 0));
            }
        );
        Add("Set left stick vertical to -0.25", 1383, 2383
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Down2Up, -0.25f));
                
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickLeft_Down2Up, 0));
                
            }
        );
        Add("Set right stick horizontal to 0.25", 1384, 2384
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Left2Right, 0.25f));
                
            }
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Left2Right, 0));
            }
        );
        Add("Set right stick horizontal to -0.25", 1385, 2385
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Left2Right, -0.25f));
                
            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Left2Right, 0));
            }
        );
        Add("Set right stick vertical to 0.25", 1386, 2386
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Down2Up, 0.25f));
                
            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Down2Up, 0));
            }
        );
        Add("Set right stick vertical to -0.25", 1387, 2387
            , () => {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Down2Up, -0.25f));
                
            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.JoystickRight_Down2Up, 0));
            }
        );
        Add("Set left trigger to 25%", 1388, 2388
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.TriggerLeft, 0.25f));

            }
            , () => {
                
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.TriggerLeft, 0));
            }
        );
        Add("Set right trigger to 25%", 1389, 2389
            , () =>
            {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.TriggerRight, 0.25f));
                
            }
            , () =>
            {
                m_executer.Execute(new TimedXBoxAction_AxisChange(Now(), XOMI.XBoxAxisInputType.TriggerRight, 0));
                
            });
        }
    }
