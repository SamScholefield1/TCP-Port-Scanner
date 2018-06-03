using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Port_Scanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void buttonGo_Click(object sender, RoutedEventArgs e)
        {
            lbOpenPorts.Items.Clear();

            int startPort = Convert.ToInt32(txtStartPort.Text);
            int endPort = Convert.ToInt32(txtEndPort.Text);
            int portRange = endPort - startPort + 1;
            string IP = txtIP.Text;
            ManualResetEvent[] doneEvents = new ManualResetEvent[portRange];
            TCPScanner[] portResultArray = new TCPScanner[portRange];

            for (int i = 0; i < portRange; i++)
            {
                doneEvents[i] = new ManualResetEvent(false);
                TCPScanner f = new TCPScanner(startPort, endPort, IP, doneEvents[i]);
                portResultArray[i] = f;
                ThreadPool.SetMinThreads(200, 200);
                ThreadPool.QueueUserWorkItem(f.ThreadPoolCallback, i);
                startPort++;
            }

            for (int i = 0; i < portRange; i++)
            {
                TCPScanner f = portResultArray[i];
                lbOpenPorts.Items.Add("Port: " + f.STARTPORT + " is " + f.portStatus);
            }
        }
    }
}
