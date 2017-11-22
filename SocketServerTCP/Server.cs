using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace RoverGUI.SocketServerTCP
{
    class Server
    {
        NetworkStream steam;
        Boolean status = false;
        Boolean connect;

        public void startServer()
        {         
            Thread tcpServerRunThread = new Thread(new ThreadStart(TCPServerRun));
            tcpServerRunThread.Start();
        }

        private void TCPServerRun()
        {
           TcpListener tcpListener = new TcpListener(IPAddress.Any, 23456);      
            if (connect)
           {              
               tcpListener.Start();
           }
            else
            {
                tcpListener.Stop();
            }
            while (connect)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                Thread tcpHandlerThread = new Thread(new ParameterizedThreadStart(tcpHandler));
                tcpHandlerThread.Start(client);           
            }
        }

        private void tcpHandler(object client)
        {
            TcpClient mClient = (TcpClient)client;
            steam = mClient.GetStream();
            if (status == false)
            {
                clientConnected();
            }
            // steam.Close();
            // mClient.Close();
        }

        private void clientConnected()
        {
            byte[] receive = new byte[1024];
            try
            {
               steam.Read(receive, 0, receive.Length);              
            }
            catch (IOException ex)
            {}
            catch (Exception ex)
            {}
            String validate = Encoding.ASCII.GetString(receive);
            if (validate.Equals(""))
                status = false;
            else
                status = true;
        }

        public Boolean checkConnection()
        {
            return status;
        }

        public Boolean sendAction(String input)
        {
            byte[] sendMsg = Encoding.ASCII.GetBytes(input);
            try
            {
                steam.Write(sendMsg, 0, sendMsg.Length);
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Action faild");
                return false;
            }
            catch(ObjectDisposedException e){
               MessageBox.Show("" + e);
               return false;
            };
            return true;
        }

        public void runServer()
        {
            this.connect = true;
        }

        public void closeServer()
        {
            this.connect = false;
            TCPServerRun();
        }
    }
}
