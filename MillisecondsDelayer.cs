using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMI
{
    public class MillisecondsDelayer
    {

        public class DelayedIntegerAction
        {
            public int m_actionIntegerToExecute;
            public ulong m_whenToExecute;
        }
        public class DelayedIndexIntegerAction
            {
            public int m_actionIntegerToExecute;
            public int m_indexToExecuteWith;
            public ulong m_whenToExecute;
        }

        public List<DelayedIndexIntegerAction> m_listOfActionIndexInteger = new List<DelayedIndexIntegerAction>();
        public List<DelayedIntegerAction> m_listOfActionInteger = new List<DelayedIntegerAction>();

        public MillisecondsDelayer()
        {}

        public ulong  GetDateNowInMilliseconds()
        {
            return (ulong) DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            
        }
        public void AppendDelayedAction(int integerToExecute, ulong delayInMilliseconds)
        {
            ulong currentTime = GetDateNowInMilliseconds();
            ulong whenToExecute = currentTime + delayInMilliseconds;
            m_listOfActionInteger.Add(new DelayedIntegerAction() { m_actionIntegerToExecute = integerToExecute, m_whenToExecute = whenToExecute });
        }

        public void AppendDelayedAction(int indexToExecuteWith, int integerToExecute, ulong delayInMilliseconds)
        {
            ulong currentTime = GetDateNowInMilliseconds();
            ulong whenToExecute = currentTime + delayInMilliseconds;
            m_listOfActionIndexInteger.Add(new DelayedIndexIntegerAction() { m_indexToExecuteWith = indexToExecuteWith, m_actionIntegerToExecute = integerToExecute, m_whenToExecute = whenToExecute });

        }
        public Action<int, int> m_onExecuteDelayedIndexIntegerAction;
        public Action<int> m_onExecuteDelayedIntegerAction;

        public void CheckForActionToExecute()
        {

            ulong currentTime = GetDateNowInMilliseconds();
           
            for (int i = m_listOfActionInteger.Count - 1; i >= 0; i--)
            {
                if (currentTime >= m_listOfActionInteger[i].m_whenToExecute)
                {
                    int actionIntegerToExecute = m_listOfActionInteger[i].m_actionIntegerToExecute;
                    m_listOfActionInteger.RemoveAt(i);
                    m_onExecuteDelayedIntegerAction?.Invoke( actionIntegerToExecute);
                }
            }
            for (int i = m_listOfActionIndexInteger.Count - 1; i >= 0; i--)
            {
                if (currentTime >= m_listOfActionIndexInteger[i].m_whenToExecute)
                {
                    int actionIntegerToExecute = m_listOfActionIndexInteger[i].m_actionIntegerToExecute;
                    int indexToExecuteWith = m_listOfActionIndexInteger[i].m_indexToExecuteWith;
                    m_listOfActionIndexInteger.RemoveAt(i);
                    m_onExecuteDelayedIndexIntegerAction?.Invoke(indexToExecuteWith, actionIntegerToExecute);
                }

            }

        }
    }
}
