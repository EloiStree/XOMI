using System;
using System.Collections.Generic;

namespace XOMI.Unstore
{
    public class IndexIntegerDateQueue { 
    
        public List<IndexIntegerDate> m_queue = new List<IndexIntegerDate>();
        public struct IndexIntegerDate { 
            public int m_index;
            public int m_integer;
            public DateTime m_date;
            public IndexIntegerDate(int index, int integer, DateTime date) {
                m_index = index;
                m_integer = integer;
                m_date = date;
            }
        }

        public void Enqueue(int index, int value, DateTime time) { 
        
            m_queue.Add(new IndexIntegerDate(index, value, time));
        }

        public void DequeueActionReady() {

            // Remote from top of the stack to start if date less that now
            for(int i= m_queue.Count-1; i>=0; i--) {
                if (m_queue[i].m_date <= DateTime.Now) {
                    
                    PushToAction( m_queue[i]);
                    m_queue.RemoveAt(i);
                }
            }
        }

        private void PushToAction( IndexIntegerDate indexIntegerDate)
        {
            SC.WriteLine("PushToAction:" + indexIntegerDate.m_index + " " + indexIntegerDate.m_integer);
        }
    }
}
