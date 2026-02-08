using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;
using XOMI.InfoHolder;
using XOMI.ParseItemClass;
using XOMI.TimedAction;
using XOMI.Unstore.Core;

namespace XOMI.Parse
{
    internal class TextParseToActions
    {
        private XBoxActionStack m_waitingActions;
        public int m_millisecondsBetweenPress = 150;


 
 
 
 
 
 
 
 
        public static GroupOfAlias<XBoxJoystickInputType> m_aliasJoystick = new GroupOfAlias<XBoxJoystickInputType>(
            new EnumAlias<XBoxJoystickInputType>(XBoxJoystickInputType.JoystickLeft, "jl"),
            new EnumAlias<XBoxJoystickInputType>(XBoxJoystickInputType.JoystickRight, "jr"),
            new EnumAlias<XBoxJoystickInputType>(XBoxJoystickInputType.JoystickLeft, "left"),
            new EnumAlias<XBoxJoystickInputType>(XBoxJoystickInputType.JoystickRight, "right")
            );
        public static GroupOfAlias<XBoxAxisInputType> m_aliasAxis = new GroupOfAlias<XBoxAxisInputType>(
            new EnumAlias<XBoxAxisInputType>(XBoxAxisInputType.TriggerRight, "tr", "rt"),
            new EnumAlias<XBoxAxisInputType>(XBoxAxisInputType.TriggerLeft, "tl", "lt"),
            new EnumAlias<XBoxAxisInputType>(XBoxAxisInputType.JoystickLeft_Down2Up, "jlv", "lv"),
            new EnumAlias<XBoxAxisInputType>(XBoxAxisInputType.JoystickLeft_Left2Right, "jlh", "lh"),
            new EnumAlias<XBoxAxisInputType>(XBoxAxisInputType.JoystickRight_Down2Up, "jrv", "rv"),
            new EnumAlias<XBoxAxisInputType>(XBoxAxisInputType.JoystickRight_Left2Right, "jrh", "rh")
            );

        public static GroupOfAlias<XBoxInputType> m_boolAlias = new GroupOfAlias<XBoxInputType>(
          new EnumAlias<XBoxInputType>(XBoxInputType.TriggerLeft, "tl", "lt", "l2", "TriggerLeft"),
          new EnumAlias<XBoxInputType>(XBoxInputType.TriggerRight, "tr", "rt", "r2", "TriggerRight"),
          new EnumAlias<XBoxInputType>(XBoxInputType.SideButtonLeft, "sbl", "l1", "lb", "SideButtonLeft"),
          new EnumAlias<XBoxInputType>(XBoxInputType.SideButtonRight, "sbr", "r1", "rb", "SideButtonRight"),
          new EnumAlias<XBoxInputType>(XBoxInputType.ArrowLeft, "al", "ArrowLeft"),
          new EnumAlias<XBoxInputType>(XBoxInputType.ArrowRight, "ar", "ArrowRight"),
          new EnumAlias<XBoxInputType>(XBoxInputType.ArrowDown, "ad", "ArrowDown", "ab"),
          new EnumAlias<XBoxInputType>(XBoxInputType.ArrowUp, "au", "ArrowUp", "at"),
          new EnumAlias<XBoxInputType>(XBoxInputType.ButtonDown, "a", "ba", "bd", "paddown", "pd", "buttondown"),
          new EnumAlias<XBoxInputType>(XBoxInputType.ButtonRight, "b", "bb", "br", "padright", "pr", "ButtonRight"),
          new EnumAlias<XBoxInputType>(XBoxInputType.ButtonLeft, "x", "bx", "bl", "padleft", "pl", "ButtonLeft"),
          new EnumAlias<XBoxInputType>(XBoxInputType.ButtonUp, "y", "by", "bu", "padup", "pu", "ButtonUp"),
          new EnumAlias<XBoxInputType>(XBoxInputType.MenuLeft, "m", "ml", "menu", "b", "back", "MenuLeft"),
          new EnumAlias<XBoxInputType>(XBoxInputType.MenuRight, "s", "mr", "start", "buttoMenuRightndown"),
          new EnumAlias<XBoxInputType>(XBoxInputType.JoystickLeftButton, "jl", "jlb", "JoystickLeftButton"),
          new EnumAlias<XBoxInputType>(XBoxInputType.JoystickRightButton, "jr", "jrb", "JoystickRightButton"),
               new EnumAlias<XBoxInputType>(XBoxInputType.JoystickLeft_Right, "jlr", "jle", "JoystickLeftRight"),
               new EnumAlias<XBoxInputType>(XBoxInputType.JoystickLeft_Up, "jlu", "jln", "JoystickLeftUp"),
               new EnumAlias<XBoxInputType>(XBoxInputType.JoystickLeft_Down, "jld", "jls", "JoystickLeftDown"),
               new EnumAlias<XBoxInputType>(XBoxInputType.JoystickLeft_Left, "jll", "jlw", "JoystickLeftLeft"),
               new EnumAlias<XBoxInputType>(XBoxInputType.JoystickRight_Right, "jrr", "jre", "JoystickRightRight"),
               new EnumAlias<XBoxInputType>(XBoxInputType.JoystickRight_Up, "jru", "jrn", "JoystickRightUp"),
               new EnumAlias<XBoxInputType>(XBoxInputType.JoystickRight_Down, "jrd", "jrs", "JoystickRightDown"),
               new EnumAlias<XBoxInputType>(XBoxInputType.JoystickRight_Left, "jrl", "jrw", "JoystickRightLeft"),
               new EnumAlias<XBoxInputType>(XBoxInputType.XboxButton, "mc", "guide", "xbox", "XboxMenu"),
               new EnumAlias<XBoxInputType>(XBoxInputType.JoystickLeft_Right, "jlr", "jle", "JoystickLeftRight")
  );

        public TextParseToActions(XBoxActionStack m_waitingActions)
        {
            this.m_waitingActions = m_waitingActions;
        }

        public void TryToAppendParseToWaitingActions(string receivedMessage)
        {//↑↓↕


            receivedMessage = SecureTextWithOnlyUnderscoreForAndSpaceAround(receivedMessage);

            RawMessageToParse message = new RawMessageToParse(receivedMessage);

            message.GetTimeOfDayToExecute(out bool hasTimeInMessage, out DateTime whenToExecute);
            message.GetMessageAsString(out string messageToParse);


            List<ParseItemAsString> parseItemsAsString = new List<ParseItemAsString>();

            string[] spaceTokens = messageToParse.Split(" ");
            for (int i = 0; i < spaceTokens.Length; i++)
            {
                string t = spaceTokens[i].Trim();
                if (t.Length > 0)
                {
                    parseItemsAsString.Add(new ParseItemAsString(t));
                }
            }

            List<ParseItem> parseItems = new List<ParseItem>();




            for (int i = 0; i < parseItemsAsString.Count; i++)
            {
                string m = parseItemsAsString[i].Message();
                string mcase = m.Trim().ToLower();

                if (mcase.IndexOf ("exit")==0)
                {
                    parseItems.Add(new ParseItem_ExitApp());
                    continue;
                }
                if (mcase.IndexOf("restart")== 0)
                {
                    parseItems.Add(new ParseItem_Restart());
                    continue;
                }
                if (mcase == "flush")
                {
                    parseItems.Add(new ParseItem_FlushAllCommands());
                    continue;
                }
                if (mcase == "release")
                {
                    parseItems.Add(new ParseItem_ReleaseAll());
                    continue;
                }
                if (mcase == "stop")
                {
                    parseItems.Add(new ParseItem_FlushAllCommands());
                    parseItems.Add(new ParseItem_ReleaseAll());
                    continue;
                }
                if (mcase == "plug")
                {
                    parseItems.Add(new ParseItem_Disconnect());
                    parseItems.Add(new ParseItem_Connect());
                    continue;
                }
                if (mcase == "unplug")
                {
                    parseItems.Add(new ParseItem_Disconnect()); 
                    continue;
                }
                if (mcase == "jlzero" || mcase == "jlz")
                {
                    parseItems.Add(new ParseItem_JoystickPercent(XBoxJoystickInputType.JoystickLeft, 0, 0)); 
                    continue;
                }
                if (mcase == "jrzero" || mcase == "jrz")
                {
                    parseItems.Add(new ParseItem_JoystickPercent(XBoxJoystickInputType.JoystickRight, 0, 0));
                    continue;
                }

                if (mcase == "jlpn") { parseItems.Add(new ParseItem_JoystickPercent(XBoxJoystickInputType.JoystickLeft, 0, 1)); continue; }
                if (mcase == "jlpne") { parseItems.Add(new ParseItem_JoystickPercent(XBoxJoystickInputType.JoystickLeft, 1, 1)); continue; }
                if (mcase == "jlpe") { parseItems.Add(new ParseItem_JoystickPercent(XBoxJoystickInputType.JoystickLeft, 1, 0)); continue; }
                if (mcase == "jlpse") { parseItems.Add(new ParseItem_JoystickPercent(XBoxJoystickInputType.JoystickLeft, 1, -1)); continue; }
                if (mcase == "jlps") { parseItems.Add(new ParseItem_JoystickPercent(XBoxJoystickInputType.JoystickLeft, 0, -1)); continue; }
                if (mcase == "jlpso") { parseItems.Add(new ParseItem_JoystickPercent(XBoxJoystickInputType.JoystickLeft, -1, -1)); continue; }
                if (mcase == "jlpo") { parseItems.Add(new ParseItem_JoystickPercent(XBoxJoystickInputType.JoystickLeft, -1, 0)); continue; }
                if (mcase == "jlpno") { parseItems.Add(new ParseItem_JoystickPercent(XBoxJoystickInputType.JoystickLeft, -1, 1)); continue; }
                if (mcase == "jrpn") { parseItems.Add(new ParseItem_JoystickPercent(XBoxJoystickInputType.JoystickRight, 0, 1)); continue; }
                if (mcase == "jrpne") { parseItems.Add(new ParseItem_JoystickPercent(XBoxJoystickInputType.JoystickRight, 1, 1)); continue; }
                if (mcase == "jrpe") { parseItems.Add(new ParseItem_JoystickPercent(XBoxJoystickInputType.JoystickRight, 1, 0)); continue; }
                if (mcase == "jrpse") { parseItems.Add(new ParseItem_JoystickPercent(XBoxJoystickInputType.JoystickRight, 1, -1)); continue; }
                if (mcase == "jrps") { parseItems.Add(new ParseItem_JoystickPercent(XBoxJoystickInputType.JoystickRight, 0, -1)); continue; }
                if (mcase == "jrpso") { parseItems.Add(new ParseItem_JoystickPercent(XBoxJoystickInputType.JoystickRight, -1, -1)); continue; }
                if (mcase == "jrpo") { parseItems.Add(new ParseItem_JoystickPercent(XBoxJoystickInputType.JoystickRight, -1, 0)); continue; }
                if (mcase == "jrpno") { parseItems.Add(new ParseItem_JoystickPercent(XBoxJoystickInputType.JoystickRight, -1, 1)); continue; }
                if (mcase == "jrrandom")
                {
                    parseItems.Add(new ParseItem_JoystickPercent(XBoxJoystickInputType.JoystickRight,
                    GetRandomLess1to1(), GetRandomLess1to1())); continue;
                }

                if (mcase == "jlrandom")
                {
                    parseItems.Add(new ParseItem_JoystickPercent(XBoxJoystickInputType.JoystickLeft,
                    GetRandomLess1to1(), GetRandomLess1to1())); continue;
                }
                if (mcase == "jrrandnorm")
                {
                    GetRandomNormalized(out float x, out float y);
                    parseItems.Add(new ParseItem_JoystickPercent(XBoxJoystickInputType.JoystickRight, x, y));
                    continue;
                }

                if (mcase == "jlrandnorm")
                {
                    GetRandomNormalized(out float x, out float y);
                    parseItems.Add(new ParseItem_JoystickPercent(XBoxJoystickInputType.JoystickLeft, x, y));
                    continue;
                }



                if (m.Contains('%'))
                {
                    
                    string[] tokens = m.ToLower().Trim().Split('%');
                    if (tokens.Length > 1)
                    {
                        if (tokens.Length == 2)
                        {
                            //jlh%0.1    //jlv%0.1      //jrh%0.1     //jrv%0.1
                            if (float.TryParse(tokens[1], out float percentValue))
                            {
                                ParseToXboxAxis(tokens[0], out bool converted, out XBoxAxisInputType axis);
                                if (converted)
                                {
                                    parseItems.Add(new ParseItem_OneAxisPercent(axis, percentValue));
                                    continue;
                                }
                            }
                        }
                        if (tokens.Length == 3)
                        {
                            //jr%0.1%0.2         //jl%0.1%0.2
                            if (float.TryParse(tokens[1], out float percentValueX) && float.TryParse(tokens[2], out float percentValueY))
                            {
                                ParseToXboxAxis(tokens[0], out bool converted, out XBoxJoystickInputType axis);
                                if (converted)
                                {
                                    parseItems.Add(new ParseItem_JoystickPercent(axis, percentValueX, percentValueY));
                                    continue;
                                }
                            }
                        }
                    }



                }

                if (int.TryParse(parseItemsAsString[i].GetText(), out int msValue1))
                {
                    parseItems.Add(new ParseItem_DelayNextItems(msValue1));
                    continue;
                }
                if (
                    parseItemsAsString[i].EndWithChar('>'))
                {
                    string mInteger = parseItemsAsString[i].GetText().Replace(">", "");
                    if (int.TryParse(mInteger, out int ms))
                    {
                        parseItems.Add(new ParseItem_DelayNextItems(ms));
                        continue;
                    }

                }

                //If Pression type only
                m = ConvertPressReleaseToUnderScores(m);

                SC.WriteLine("m " + m);
                string aliasName = GetFrontOfSpliter(m);
                GetTimeAfterSplitter(parseItemsAsString[i], out bool hasSpliter, out bool hasValideTime, out int timeInMilliseconds);

                m_boolAlias.Get(aliasName, out bool found, out XBoxInputType inputType);
                SC.WriteLine("tdd |" + aliasName + "|" + string.Join(" # ", hasSpliter, hasValideTime, timeInMilliseconds, inputType));

                if (hasSpliter && m.IndexOf('_')>-1 )
                {
                    if (found && inputType != XBoxInputType.Undefined)
                    {
                        parseItems.Add(new ParseItem_PressInput(PressType.Press, inputType));
                        if (hasValideTime)
                        {

                            parseItems.Add(new ParseItem_DelayNextItems(timeInMilliseconds));
                            parseItems.Add(new ParseItem_PressInput(PressType.Release, inputType));
                        }
                        continue;
                    }

                }
                else if (hasSpliter && m.IndexOf('-') > -1)
                {
                    if (found && inputType != XBoxInputType.Undefined)
                    {
                        parseItems.Add(new ParseItem_PressInput(PressType.Release, inputType));
                        if (hasValideTime)
                        {

                            parseItems.Add(new ParseItem_DelayNextItems(timeInMilliseconds));
                            parseItems.Add(new ParseItem_PressInput(PressType.Press, inputType));
                        }
                        continue;
                    }

                }
                else if (hasSpliter && m.IndexOf('=') > -1)
                {

                    if (found && inputType != XBoxInputType.Undefined)
                    {
                        parseItems.Add(new ParseItem_PressInput(PressType.Press, inputType));
                        if (hasValideTime)
                            parseItems.Add(new ParseItem_DelayNextItems(timeInMilliseconds));
                        else
                            parseItems.Add(new ParseItem_DelayNextItems(m_millisecondsBetweenPress));
                        parseItems.Add(new ParseItem_PressInput(PressType.Release, inputType));
                        SC.WriteLine("t " + aliasName + " " + inputType);
                        continue;
                    }

                }
                else if(!hasSpliter)
                {
                    SC.WriteLine("i " + aliasName + " " + inputType);
                    if (found && inputType != XBoxInputType.Undefined)
                    {
                        parseItems.Add(new ParseItem_PressInput(PressType.Press, inputType));
                        parseItems.Add(new ParseItem_DelayNextItems(m_millisecondsBetweenPress));
                        parseItems.Add(new ParseItem_PressInput(PressType.Release, inputType));
                        continue;
                    }
                }
            }

            List<TimedXBoxAction> actions = m_waitingActions.GetRefToActionStack();
            DateTime timeCount = whenToExecute;
            for (int i = 0; i < parseItems.Count; i++)
            {
                if (parseItems[i] is ParseItem_DelayNextItems)
                {
                    ParseItem_DelayNextItems item = (ParseItem_DelayNextItems)parseItems[i];
                    timeCount = timeCount.AddMilliseconds(item.GetValueInMilliseconds());
                }
                if (parseItems[i] is ParseItem_PressInput)
                {
                    ParseItem_PressInput item = (ParseItem_PressInput)parseItems[i];
                    // SC.WriteLine(string.Format("{0}| {1}-{2}", timeCount.ToString("yyyy-dd-HH-mm-ss-fff"), item.press, item.inputType));
                    actions.Add(new TimedXBoxAction_ApplyChange(timeCount, item.press, item.inputType));
                }
                if (parseItems[i] is ParseItem_OneAxisPercent)
                {
                    ParseItem_OneAxisPercent item = (ParseItem_OneAxisPercent)parseItems[i];
                    actions.Add(new TimedXBoxAction_AxisChange(timeCount, item.m_inputType, item.m_pressionPercent));
                }
                if (parseItems[i] is ParseItem_JoystickPercent)
                {
                    ParseItem_JoystickPercent item = (ParseItem_JoystickPercent)parseItems[i];
                    actions.Add(new TimedXBoxAction_JoysticksChange(timeCount, item.m_inputType, item.m_percentHorizontal, item.m_percentVertical));
                }
                if (parseItems[i] is ParseItem_FlushAllCommands)
                {
                    actions.Add(new TimedXBoxAction_FlushAllCommands(timeCount));
                }
                if (parseItems[i] is ParseItem_ReleaseAll)
                {
                    actions.Add(new TimedXBoxAction_ReleaseAll(timeCount));
                }
                if (parseItems[i] is ParseItem_ExitApp)
                {
                    actions.Add(new TimedXBoxAction_Exit(timeCount));
                }
                if (parseItems[i] is ParseItem_Restart)
                {
                    actions.Add(new TimedXBoxAction_Restart(timeCount));
                }

            }

        }

        public float GetRandomLess1to1()
        {
            Random random = new Random();
            return (float)(random.NextDouble() * 2 - 1);
        }
        public void GetRandomNormalized(out float x, out float y)
        {
            Vector2 v2 = new Vector2(GetRandomLess1to1(), GetRandomLess1to1());
            v2 = Vector2.Normalize(v2);
            x = v2.X;
            y = v2.Y;
        }

        private void ParseToXboxAxis(string givenAlias, out bool converted, out XBoxJoystickInputType axis)
        {
            m_aliasJoystick.Get(givenAlias, out converted, out axis);
        }

        private void ParseToXboxAxis(string givenAlias, out bool converted, out XBoxAxisInputType axis)
        {
            m_aliasAxis.Get(givenAlias, out converted, out axis);
        }

        private static string SecureTextWithOnlyUnderscoreForAndSpaceAround(string receivedMessage)
        {
            receivedMessage = receivedMessage.Replace("(", " ( ");
            receivedMessage = receivedMessage.Replace(")", " ) ");
            while (receivedMessage.IndexOf("  ") > -1)
            {
                receivedMessage = receivedMessage.Replace("  ", " ");
            }
            receivedMessage = receivedMessage.Trim();
            return receivedMessage;
        }

        private static string ConvertPressReleaseToUnderScores(string receivedMessage)
        {
            receivedMessage = receivedMessage.Replace("-", "↑");
            receivedMessage = receivedMessage.Replace("'", "↑");
            receivedMessage = receivedMessage.Replace("_", "↓");
            receivedMessage = receivedMessage.Replace(",", "↓");
            receivedMessage = receivedMessage.Replace(".", "↓");
            receivedMessage = receivedMessage.Replace(":", "↕");
            receivedMessage = receivedMessage.Replace(";", "↕");
            receivedMessage = receivedMessage.Replace("↑", "-");
            receivedMessage = receivedMessage.Replace("↓", "_");
            receivedMessage = receivedMessage.Replace("↕", "=");
            receivedMessage = receivedMessage.Trim();
            return receivedMessage;
        }

        char[] m_pressiontypeCharSplitter = new char[] { '-', '_', '=', };
        private string GetFrontOfSpliter(string m)
        {
            string[] tokens = m.Split(m_pressiontypeCharSplitter);
            if (tokens.Length <= 0) return m;
            else return tokens[0];
        }

        private void GetTimeAfterSplitter(ParseItemAsString parseItemAsString, out bool hasSpliter, out bool hasValideTime, out int timeInMilliseconds)
        {
            parseItemAsString.GetText(out string text);
            string[] tokens = text.Split(m_pressiontypeCharSplitter);
            if (tokens.Length <= 0)
            {
                hasSpliter = false; hasValideTime = false;
                timeInMilliseconds = 0;
            }
            else
            {
                hasSpliter = true;
                hasValideTime = int.TryParse(tokens[tokens.Length - 1].Trim().ToLower(), out timeInMilliseconds);
            }
        }
    }
}