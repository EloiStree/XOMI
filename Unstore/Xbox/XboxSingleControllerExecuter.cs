using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using XOMI.Static;
using XOMI.TimedAction;

namespace XOMI.Unstore.Xbox
{
    public class XboxSingleControllerExecuter
    {
        ViGEmClient client;
        IXbox360Controller controller;

        public XboxSingleControllerExecuter()
        {
            client = new ViGEmClient();
            controller = client.CreateXbox360Controller();
            controller.Connect();
        }

        public void Execute(TimedXBoxAction toDo)
        {
            if ( toDo is TimedXBoxAction_ApplyChange)
            {
                Execute((TimedXBoxAction_ApplyChange)toDo);
            }
            else if (toDo is TimedXBoxAction_AxisChange)
            {
                Execute((TimedXBoxAction_AxisChange)toDo);
            }
            else if (toDo is TimedXBoxAction_JoysticksChange)
            {
                Execute((TimedXBoxAction_JoysticksChange)toDo);
            }
            else if (toDo is TimedXBoxAction_DoubleJoysticksChange)
            {
                Execute((TimedXBoxAction_DoubleJoysticksChange)toDo);
            }
            else if (toDo is TimedXBoxAction_Disconnect)
            {
                Execute((TimedXBoxAction_Disconnect)toDo);
            }
            else if (toDo is TimedXBoxAction_Connect)
            {
                Execute((TimedXBoxAction_Connect)toDo);
            }
            else if (toDo is TimedXBoxAction_Exit)
            {
                Execute((TimedXBoxAction_Exit)toDo);
            }
            else if (toDo is TimedXBoxAction_Restart)
            {
                Execute((TimedXBoxAction_Restart)toDo);
            }
            else if (toDo is TimedXBoxAction_ReleaseAll)
            {
                Execute((TimedXBoxAction_ReleaseAll)toDo);
            }
            else if (toDo is TimedXBoxAction_ReleaseAllButMenu)
            {
                Execute((TimedXBoxAction_ReleaseAllButMenu)toDo);
            }
        }

        public void Execute(TimedXBoxAction_DoubleJoysticksChange toDo)
        {

            if (StaticVariable.m_debugDevMessage)
                SC.WriteLine(string.Format("Gamepad {0}| {1} {2} {3} {4}",
                    toDo.GetWhenToExecute().ToString("yyyy-dd-HH-mm-ss-fff"),
                    toDo.GetJoystickLeftX(),
                    toDo.GetJoystickLeftY(),
                    toDo.GetJoystickRightX(),
                    toDo.GetJoystickRightY()
                    ));
            
                    controller.SetAxisValue(Xbox360Axis.LeftThumbX, (short)(toDo.GetJoystickLeftX() * short.MaxValue));
                    controller.SetAxisValue(Xbox360Axis.LeftThumbY, (short)(toDo.GetJoystickLeftY() * short.MaxValue));
                    controller.SetAxisValue(Xbox360Axis.RightThumbX, (short)(toDo.GetJoystickRightX() * short.MaxValue));
                    controller.SetAxisValue(Xbox360Axis.RightThumbY, (short)(toDo.GetJoystickRightY() * short.MaxValue));
        }

        public void Execute(TimedXBoxAction_ApplyChange toDo)
        {
            if (StaticVariable.m_debugDevMessage)
                SC.WriteLine(string.Format("{0}| {1}{3} {2}", toDo.GetWhenToExecute().ToString("yyyy-dd-HH-mm-ss-fff"),
                    toDo.GetPressionType(), toDo.GetInputType()
                , toDo.GetPressionType() == PressType.Press ? "↓" : "↑"));
            bool pression = toDo.GetPressionType() == PressType.Press;

            switch (toDo.GetInputType())
            {
                case XBoxInputType.ArrowLeft:
                    controller.SetButtonState(Xbox360Button.Left, pression);
                    break;
                case XBoxInputType.ArrowRight:
                    controller.SetButtonState(Xbox360Button.Right, pression);
                    break;
                case XBoxInputType.ArrowDown:
                    controller.SetButtonState(Xbox360Button.Down, pression);
                    break;
                case XBoxInputType.ArrowUp:
                    controller.SetButtonState(Xbox360Button.Up, pression);
                    break;


                case XBoxInputType.JoystickLeft_Left:
                    controller.SetAxisValue(Xbox360Axis.LeftThumbX, -32768);
                    break;
                case XBoxInputType.JoystickLeft_Right:
                    controller.SetAxisValue(Xbox360Axis.LeftThumbX, 32767);
                    break;
                case XBoxInputType.JoystickLeft_Down:
                    controller.SetAxisValue(Xbox360Axis.LeftThumbY, -32768);
                    break;
                case XBoxInputType.JoystickLeft_Up:
                    controller.SetAxisValue(Xbox360Axis.LeftThumbY, 32767);
                    break;

                case XBoxInputType.JoystickRight_Left:
                    controller.SetAxisValue(Xbox360Axis.RightThumbX, -32768);
                    break;
                case XBoxInputType.JoystickRight_Right:
                    controller.SetAxisValue(Xbox360Axis.RightThumbX, 32767);
                    break;
                case XBoxInputType.JoystickRight_Down:
                    controller.SetAxisValue(Xbox360Axis.RightThumbY, -32768);
                    break;
                case XBoxInputType.JoystickRight_Up:
                    controller.SetAxisValue(Xbox360Axis.RightThumbY, 32767);
                    break;


                case XBoxInputType.ButtonUp:
                    controller.SetButtonState(Xbox360Button.Y, pression);
                    break;
                case XBoxInputType.ButtonDown:
                    controller.SetButtonState(Xbox360Button.A, pression);
                    break;
                case XBoxInputType.ButtonRight:
                    controller.SetButtonState(Xbox360Button.B, pression);
                    break;
                case XBoxInputType.ButtonLeft:
                    controller.SetButtonState(Xbox360Button.X, pression);
                    break;
                case XBoxInputType.SideButtonLeft:
                    controller.SetButtonState(Xbox360Button.LeftShoulder, pression);
                    break;
                case XBoxInputType.SideButtonRight:
                    controller.SetButtonState(Xbox360Button.RightShoulder, pression);
                    break;
                case XBoxInputType.TriggerLeft:
                    controller.SetSliderValue(Xbox360Slider.LeftTrigger, pression ? (byte)255 : (byte)0);
                    break;
                case XBoxInputType.TriggerRight:
                    controller.SetSliderValue(Xbox360Slider.RightTrigger, pression ? (byte)255 : (byte)0);
                    break;
                case XBoxInputType.MenuLeft:
                    controller.SetButtonState(Xbox360Button.Back, pression);
                    break;
                case XBoxInputType.MenuRight:
                    controller.SetButtonState(Xbox360Button.Start, pression);
                    break;
                case XBoxInputType.XboxButton:
                    //Check if it is correct.
                    controller.SetButtonState(Xbox360Button.Guide, pression);
                    break;
                case XBoxInputType.JoystickLeftButton:
                    controller.SetButtonState(Xbox360Button.LeftThumb, pression);
                    break;
                case XBoxInputType.JoystickRightButton:
                    controller.SetButtonState(Xbox360Button.RightThumb, pression);
                    break;
                default:
                    break;
            }
        }

        public void Execute(TimedXBoxAction_AxisChange toDo)
        {
            if (StaticVariable.m_debugDevMessage)
                SC.WriteLine(string.Format("GG {0}| {1} {2} ",
                    toDo.GetWhenToExecute().ToString("yyyy-dd-HH-mm-ss-fff"),
                    toDo.GetInputType(),
                    toDo.GetPercentToApply()));


            switch (toDo.GetInputType())
            {
                case XBoxAxisInputType.TriggerLeft:
                    controller.SetSliderValue(Xbox360Slider.LeftTrigger, (byte)(toDo.GetPercentToApply() * byte.MaxValue));
                    break;
                case XBoxAxisInputType.TriggerRight:
                    controller.SetSliderValue(Xbox360Slider.RightTrigger, (byte)(toDo.GetPercentToApply() * byte.MaxValue));
                    break;

                case XBoxAxisInputType.JoystickLeft_Left2Right:
                    controller.SetAxisValue(Xbox360Axis.LeftThumbX, (short)(toDo.GetPercentToApply() * short.MaxValue));
                    break;
                case XBoxAxisInputType.JoystickLeft_Down2Up:
                    controller.SetAxisValue(Xbox360Axis.LeftThumbY, (short)(toDo.GetPercentToApply() * short.MaxValue));
                    break;
                case XBoxAxisInputType.JoystickRight_Left2Right:
                    controller.SetAxisValue(Xbox360Axis.RightThumbX, (short)(toDo.GetPercentToApply() * short.MaxValue));
                    break;
                case XBoxAxisInputType.JoystickRight_Down2Up:
                    controller.SetAxisValue(Xbox360Axis.RightThumbY, (short)(toDo.GetPercentToApply() * short.MaxValue));
                    break;


                default:
                    break;
            }


        }
        public void Execute(TimedXBoxAction_JoysticksChange toDo)
        {
            if (StaticVariable.m_debugDevMessage)
                SC.WriteLine(string.Format("GG {0}| {1} {2} {3} ",
                    toDo.GetWhenToExecute().ToString("yyyy-dd-HH-mm-ss-fff"),
                    toDo.GetInputType(),
                    toDo.GetPercentHorizontalLeftRightToApply(),
                    toDo.GetPercentVerticalBotTopToApply()
                    ));


            switch (toDo.GetInputType())
            {
                case XBoxJoystickInputType.JoystickLeft:
                    controller.SetAxisValue(Xbox360Axis.LeftThumbX, (short)(toDo.GetPercentHorizontalLeftRightToApply() * short.MaxValue));
                    controller.SetAxisValue(Xbox360Axis.LeftThumbY, (short)(toDo.GetPercentVerticalBotTopToApply() * short.MaxValue));
                    break;
                case XBoxJoystickInputType.JoystickRight:
                    controller.SetAxisValue(Xbox360Axis.RightThumbX, (short)(toDo.GetPercentHorizontalLeftRightToApply() * short.MaxValue));
                    controller.SetAxisValue(Xbox360Axis.RightThumbY, (short)(toDo.GetPercentVerticalBotTopToApply() * short.MaxValue));
                    break;

                default:
                    break;
            }
        }
        public void Execute(TimedXBoxAction_Disconnect toDo)
        {
            controller.Disconnect();
        }
        public void Execute(TimedXBoxAction_Connect toDo)
        {
            controller.Connect();
        }
        public void Execute(TimedXBoxAction_Exit toDo)
        {
            Environment.Exit(0);


        }
        public void Execute(TimedXBoxAction_Restart toDo)
        {
            //{ //VA
            //var fileName = Assembly.GetExecutingAssembly().Location;
            //System.Diagnostics.Process.Start(fileName);
            //Environment.Exit(0);
            //}

            { //VB
            System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.FriendlyName);
            Environment.Exit(0);
            }

        }

        
        public void Execute(TimedXBoxAction_ReleaseAll toDo)
        {
            controller.SetAxisValue(Xbox360Axis.LeftThumbX, 0);
            controller.SetAxisValue(Xbox360Axis.LeftThumbY, 0);
            controller.SetAxisValue(Xbox360Axis.RightThumbX, 0);
            controller.SetAxisValue(Xbox360Axis.RightThumbY, 0);
            controller.SetSliderValue(Xbox360Slider.LeftTrigger, 0);
            controller.SetSliderValue(Xbox360Slider.RightTrigger, 0);
            controller.SetButtonState(Xbox360Button.A, false);
            controller.SetButtonState(Xbox360Button.B, false);
            controller.SetButtonState(Xbox360Button.X, false);
            controller.SetButtonState(Xbox360Button.Y, false);
            controller.SetButtonState(Xbox360Button.Up, false);
            controller.SetButtonState(Xbox360Button.Down, false);
            controller.SetButtonState(Xbox360Button.Left, false);
            controller.SetButtonState(Xbox360Button.Right, false);
            controller.SetButtonState(Xbox360Button.RightThumb, false);
            controller.SetButtonState(Xbox360Button.LeftThumb, false);
            controller.SetButtonState(Xbox360Button.LeftShoulder, false);
            controller.SetButtonState(Xbox360Button.RightShoulder, false);
            controller.SetButtonState(Xbox360Button.Start, false);
            controller.SetButtonState(Xbox360Button.Guide, false);
            controller.SetButtonState(Xbox360Button.Back, false);

            if (StaticUserPreference.m_wantDebugInfo)
                SC.WriteLine("Released All");
        }

        public void Execute(TimedXBoxAction_ReleaseAllButMenu action) {
            controller.SetAxisValue(Xbox360Axis.LeftThumbX, 0);
            controller.SetAxisValue(Xbox360Axis.LeftThumbY, 0);
            controller.SetAxisValue(Xbox360Axis.RightThumbX, 0);
            controller.SetAxisValue(Xbox360Axis.RightThumbY, 0);
            controller.SetSliderValue(Xbox360Slider.LeftTrigger, 0);
            controller.SetSliderValue(Xbox360Slider.RightTrigger, 0);
            controller.SetButtonState(Xbox360Button.A, false);
            controller.SetButtonState(Xbox360Button.B, false);
            controller.SetButtonState(Xbox360Button.X, false);
            controller.SetButtonState(Xbox360Button.Y, false);
            controller.SetButtonState(Xbox360Button.Up, false);
            controller.SetButtonState(Xbox360Button.Down, false);
            controller.SetButtonState(Xbox360Button.Left, false);
            controller.SetButtonState(Xbox360Button.Right, false);
            controller.SetButtonState(Xbox360Button.RightThumb, false);
            controller.SetButtonState(Xbox360Button.LeftThumb, false);
            controller.SetButtonState(Xbox360Button.LeftShoulder, false);
            controller.SetButtonState(Xbox360Button.RightShoulder, false);

            if (StaticUserPreference.m_wantDebugInfo)
                SC.WriteLine("Released All");

        }

        public void Randomize_AllButMenu()
        {
            controller.SetAxisValue(Xbox360Axis.LeftThumbX, (short)(new Random().Next(-32768, 32767)));
            controller.SetAxisValue(Xbox360Axis.LeftThumbY, (short)(new Random().Next(-32768, 32767)));
            controller.SetAxisValue(Xbox360Axis.RightThumbX, (short)(new Random().Next(-32768, 32767)));
            controller.SetAxisValue(Xbox360Axis.RightThumbY, (short)(new Random().Next(-32768, 32767)));
            controller.SetSliderValue(Xbox360Slider.LeftTrigger, (byte)(new Random().Next(0, 255)));
            controller.SetSliderValue(Xbox360Slider.RightTrigger, (byte)(new Random().Next(0, 255)));
            controller.SetButtonState(Xbox360Button.A, new Random().Next(0, 2) == 1);
            controller.SetButtonState(Xbox360Button.B, new Random().Next(0, 2) == 1);
            controller.SetButtonState(Xbox360Button.X, new Random().Next(0, 2) == 1);
            controller.SetButtonState(Xbox360Button.Y, new Random().Next(0, 2) == 1);
            controller.SetButtonState(Xbox360Button.Up, new Random().Next(0, 2) == 1);
            controller.SetButtonState(Xbox360Button.Down, new Random().Next(0, 2) == 1);
            controller.SetButtonState(Xbox360Button.Left, new Random().Next(0, 2) == 1);
            controller.SetButtonState(Xbox360Button.Right, new Random().Next(0, 2) == 1);
            controller.SetButtonState(Xbox360Button.RightThumb, new Random().Next(0, 2) == 1);
            controller.SetButtonState(Xbox360Button.LeftThumb, new Random().Next(0, 2) == 1);
            controller.SetButtonState(Xbox360Button.LeftShoulder, new Random().Next(0, 2) == 1);
            controller.SetButtonState(Xbox360Button.RightShoulder, new Random().Next(0, 2) == 1);
            controller.SetButtonState(Xbox360Button.Start, new Random().Next(0, 2) == 1);

        }

        
    }
}
