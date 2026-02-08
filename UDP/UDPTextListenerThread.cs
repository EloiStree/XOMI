using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace XOMI.UDP
{

    public class UDPTextListenerThread
    {
        public enum TypeEncoding { UTF8, Unicode }
        public TypeEncoding m_encodingType;
        public int m_portId = 2504;
        public float m_timeBetweenUnityCheck = 0.05f;
        public ThreadPriority m_threadPriority;

        public Queue<string> m_receivedMessages = new Queue<string>();
        public string m_lastReceived;
        private bool m_wantThreadAlive = true;
        private Thread m_threadListener = null;
        public UdpClient m_listener;
        public IPEndPoint m_ipEndPoint;
        public bool m_hasBeenKilled;

        public DateTime m_isStillUsed;
        public float m_timeMaxBetweenPingToStayAlive = 10;

        public void UpdateTheAutodestructionOfThreadTimer()
        {
            m_isStillUsed = DateTime.Now;

        }

        public bool ShouldStayAlive()
        {
            return (DateTime.Now - m_isStillUsed).Seconds < m_timeMaxBetweenPingToStayAlive;
        }

        public void Launch(ref Queue<string> messageQueue, int port)
        {
            UpdateTheAutodestructionOfThreadTimer();
            m_receivedMessages = messageQueue;
            m_portId = port;
            m_wantThreadAlive = true;
            if (m_threadListener == null)
            {
                m_threadListener = new Thread(ChechUdpClientMessageInComing);
                m_threadListener.Priority = m_threadPriority;
                m_threadListener.Start();
            }
        }


        private void Kill()
        {
            if (m_listener != null)
                m_listener.Close();
            m_wantThreadAlive = false;
            m_hasBeenKilled = true;
        }


        private void ChechUdpClientMessageInComing()
        {

            if (m_listener == null)
            {
                m_listener = new UdpClient(m_portId);
                m_ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
            }

            while (m_wantThreadAlive)
            {
                if (!ShouldStayAlive())
                    Kill();
                try
                {

                    byte[] receiveBytes = m_listener.Receive(ref m_ipEndPoint);

                    string returnData = m_encodingType == TypeEncoding.UTF8 ? Encoding.UTF8.GetString(receiveBytes) : Encoding.Unicode.GetString(receiveBytes);
                    m_receivedMessages.Enqueue(returnData);
                }
                catch (Exception e)
                {
                    SC.WriteLine(e.ToString());
                    m_wantThreadAlive = false;
                }
            }
        }
    }

}