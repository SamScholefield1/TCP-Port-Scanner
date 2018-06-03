using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Port_Scanner
{
    public class TCPScanner
    {
        private ManualResetEvent _doneEvent;
        private int _startPort;
        private int _endPort;
        private int _port;
        private string _IP;

        public int STARTPORT { get { return _startPort; } }
        public int ENDPORT { get { return _endPort; } }
        public string portStatus { get { if(scanPort(_startPort)) { return "open"; } else { return "closed"; } } }

        public TCPScanner(int startPort, int endPort, string IP, ManualResetEvent doneEvent)
         {
             _startPort = startPort;
             _endPort = endPort;
             _IP = IP;
             _doneEvent = doneEvent;
         }
       
         public void ThreadPoolCallback(Object threadContext)
         {
             int threadIndex = (int)threadContext;
             Console.WriteLine("thread {0} started...", threadIndex);
             bool portResult = scanPort(_startPort);
             Console.WriteLine("thread {0} result calculated...", threadIndex);
             _doneEvent.Set();
         }

         public bool scanPort(int startPort)
         {
             TcpClient tcp = new TcpClient();
             bool connect;

             try
             {
                 tcp.Connect(_IP, startPort);
                 connect = true;
             }
             catch (SocketException)
             {
                 connect = false;
             }
             finally
             {
                 tcp.Close();
             }
             return connect;
         } 
    }
}
