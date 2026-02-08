using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace XOMI.UDP
{
    public class UDPIIDListenerThread
    {
        public int m_portId = 2505;
        public ThreadPriority m_threadPriority;

        public Queue<byte[]> m_receivedMessages = new Queue<byte[]>();
        public byte[] m_lastReceived;
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

        public void Launch(ref Queue<byte[]> messageQueue, int port)
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

            SC.WriteLine("Start Thread UDP Int");

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

                    byte[] receiveBytes = m_listener.Receive(ref m_ipEndPoint).ToArray();
                    SC.WriteLine($"Debug Received Udp: {receiveBytes.Length}|{receiveBytes}");
                    // Must be 16 ,12,8,4
                    if (receiveBytes.Length == 16
                        || receiveBytes.Length == 12
                        || receiveBytes.Length == 8
                        || receiveBytes.Length == 4
                        ) {
                            m_receivedMessages.Enqueue(receiveBytes);
                        } 
                }
                catch (Exception e)
                {
                    SC.WriteLine(e.ToString());
                    m_wantThreadAlive = false;
                }
            }
            SC.WriteLine("End Thread UDP Int");
        }
    }

}