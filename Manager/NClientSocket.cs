using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Ndev.NNetSocket
{
    public class NClientSocket
    {
        //Port: 1,024 to 65,535
        #region EnumConnectionEvent
        public enum EConnectionEventClient
        {
            RECEIVEDATA,
            CLIENTCONNECTED,
            CLIENTDISCONNECTED
        }
        #endregion

        #region Constructor
        public NClientSocket()
        {

        }
        public NClientSocket(string ipaddress, int port)
        {
            if (string.IsNullOrEmpty(ipaddress))
            {
                m_errorMsg = "Cannot is empty IP address, let's enter the IP address exactly!";
                ClientErrorEventCallback?.Invoke(m_errorMsg);
                return;
            }
            this.IpAddress = ipaddress;
            this.Port = port;
        }
        #endregion

        #region Variable
        private string m_errorMsg = string.Empty;
        private string m_ReceiveString = string.Empty;
        private string IpAddress = string.Empty;
        private int Port = 0;
        private Socket m_clientSocket = null;
        #endregion

        #region Callback Variable
        IAsyncResult m_asyncResult;
        public AsyncCallback dlgCallback;
        #endregion

        #region Property
        public string ErrorMsg
        {
            get { return this.m_errorMsg; }
            set { this.m_errorMsg = value; }
        }
        public string ReceiveString
        {
            get { return this.m_ReceiveString; }
            set { this.m_ReceiveString = value; }
        }
        public Socket ClientSocket
        {
            get { return this.m_clientSocket; }
            set { this.m_clientSocket = value; }
        }
        public int ReceiveBufferSize
        {
            get { return this.m_clientSocket.ReceiveBufferSize; }
            set { this.m_clientSocket.ReceiveBufferSize = value; }
        }
        public bool IsConnected
        {
            get { return this.m_clientSocket.Connected; }
        }
        public bool IsBlocking
        {
            get { return (this.m_clientSocket.Blocking); }
        }
        public bool IsAvailable
        {
            get { return this.m_clientSocket.Available > 0; }
        }
        #endregion

        #region Methods
        private bool ConnectToServer(string ipaddress, int port)
        {
            bool result = false;
            try
            {
                this.IpAddress = ipaddress;
                this.Port = port;
                this.m_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ipaddress2 = IPAddress.Parse(this.IpAddress.Trim());
                IPEndPoint remoteEP = new IPEndPoint(ipaddress2, this.Port);
                this.m_clientSocket.Connect(remoteEP);
                result = true;
            }
            catch (Exception ex)
            {
                this.m_errorMsg = "Failed to occur when connecting, try again!\n" + ex.Message;
                result = false;
                ClientErrorEventCallback?.Invoke(m_errorMsg);
            }
            return result;
        }
        private void Disconnect(bool reuseSocket = true)
        {
            this.m_clientSocket.Disconnect(reuseSocket);
        }
        public bool Reconnect()
        {
            bool result;
            try
            {
                this.m_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ipaddress = IPAddress.Parse(this.IpAddress.Trim());
                IPEndPoint remoteEP = new IPEndPoint(ipaddress, this.Port);
                this.m_clientSocket.Connect(remoteEP);
                result = true;
            }
            catch (Exception ex)
            {
                this.m_errorMsg = "Cannot reuse socket\n" + ex.Message;
                result = false;
                ClientErrorEventCallback?.Invoke(m_errorMsg);
            }
            return result;
        }
        public void Dispose()
        {
            this.m_clientSocket.Close();
            this.m_clientSocket.Dispose();
        }
        public bool SendMsg(string Msg, string utf8 = null)
        {
            bool result;
            try
            {
                byte[] bytes;
                if (string.IsNullOrEmpty(utf8))
                    bytes = Encoding.ASCII.GetBytes(Msg);
                else
                    bytes = Encoding.UTF8.GetBytes(Msg);
                this.m_clientSocket.Send(bytes);
                result = true;
            }
            catch (Exception ex)
            {
                this.m_errorMsg = ex.Message;
                result = false;
            }
            return result;
        }
        public bool SendMsg(byte[] Msg)
        {
            bool result;
            try
            {
                bool connected = this.m_clientSocket.Connected;
                if (connected)
                {
                    this.m_clientSocket.Send(Msg);
                    result = true;
                }
                else
                {
                    result = false;
                }
                result = true;
            }
            catch (Exception ex)
            {
                this.m_errorMsg = ex.Message;
                this.ClientErrorEventCallback?.Invoke(m_errorMsg);
                result = false;
            }

            return result;
        }
        public bool RecieveMsg(out string Msg)
        {
            bool result;
            try
            {
                bool flag = !this.m_clientSocket.Connected;
                if (flag)
                {
                    this.m_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPAddress ipaddress = IPAddress.Parse(this.IpAddress.Trim());
                    IPEndPoint remoteEP = new IPEndPoint(ipaddress, this.Port);
                    this.m_clientSocket.Connect(remoteEP);
                }
                byte[] array = new byte[1048576];
                int num = this.m_clientSocket.Receive(array);
                bool flag2 = num > 0;
                if (flag2)
                {
                    Msg = Encoding.ASCII.GetString(array, 0, num);
                    result = true;
                }
                else
                {
                    Msg = "";
                    result = false;
                }
            }
            catch (Exception ex)
            {
                Msg = (this.m_errorMsg = ex.Message);
                result = false;
            }
            return result;
        }
        public bool RecieveMsg(out byte[] byteMsg)
        {
            bool result;
            try
            {
                byteMsg = new byte[1024];
                bool flag = !this.m_clientSocket.Connected;
                if (flag)
                {
                    byteMsg[0] = 0;
                    result = false;
                }
                else
                {
                    int num = this.m_clientSocket.Receive(byteMsg);
                    bool flag2 = num > 0;
                    if (flag2)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                this.m_errorMsg = ex.Message;
                byteMsg = new byte[2];
                result = false;
            }
            return result;
        }
        public bool RecieveMsg(out byte[] byteMsg, uint datalength)
        {
            bool result;
            try
            {
                byteMsg = new byte[(int)datalength];
                bool flag = !this.m_clientSocket.Connected;
                if (flag)
                {
                    byteMsg[0] = 0;
                    result = false;
                }
                else
                {
                    int num = this.m_clientSocket.Receive(byteMsg);
                    bool flag2 = num >= 0;
                    if (flag2)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                this.m_errorMsg = ex.Message;
                byteMsg = new byte[2];
                result = false;
            }
            return result;
        }
        public bool SetSendTimeOut(int millisecond)
        {
            bool result;
            try
            {
                this.m_clientSocket.SendTimeout = millisecond;
                result = true;
            }
            catch (Exception ex)
            {
                this.m_errorMsg = ex.Message;
                result = false;
            }
            return result;
        }
        public bool SetReceiveTimeOut(int millisecond)
        {
            bool result;
            try
            {
                this.m_clientSocket.ReceiveTimeout = millisecond;
                result = true;
            }
            catch (Exception ex)
            {
                this.m_errorMsg = ex.Message;
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Check IP valid. 
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public bool PingIP(string ip)
        {
            bool result;
            try
            {
                Ping pinger = new Ping();
                PingReply reply = pinger.Send(ip);
                if (reply.Status == IPStatus.Success)
                {
                    result = true;
                }
                else
                {
                    result = false;
                    m_errorMsg = "Cannot Ping to IP"; 
                    ClientErrorEventCallback?.Invoke(m_errorMsg);
                }
            }
            catch (Exception ex)
            {
                this.m_errorMsg = ex.Message;
                result = false;
                ClientErrorEventCallback?.Invoke(m_errorMsg);
            }
            return result;
        }
        /// <summary>
        /// Client connect to server and wait for data
        /// </summary>
        public void ClientConnect(string ipaddress, int port, bool dataReceiveIsUTF8 = true)
        {
            if (!PingIP(ipaddress))
                return;
            if (!ConnectToServer(ipaddress, port))
                return;
            WaitForData(dataReceiveIsUTF8);
            ConnectionEventCallback?.Invoke(EConnectionEventClient.CLIENTCONNECTED, true);
        }
        public void ClientConnect(bool dataReceiveIsUTF8 = true)
        {
            if (!PingIP(this.IpAddress))
                return;
            if (!ConnectToServer(this.IpAddress, this.Port))
                return;
            //SendMsg("Hi,Server!");
            WaitForData(dataReceiveIsUTF8);
            ConnectionEventCallback?.Invoke(EConnectionEventClient.CLIENTCONNECTED, true);
        }
        /// <summary>
        /// This function disconnect to server
        /// </summary>
        public void ClientDisconnect()
        {
            if (m_clientSocket != null)
            {
                try
                {
                    Disconnect();
                    ConnectionEventCallback?.Invoke(EConnectionEventClient.CLIENTDISCONNECTED, true);
                }
                catch (Exception ex)
                {
                    m_errorMsg = ex.Message;
                    ClientErrorEventCallback?.Invoke(m_errorMsg);
                }
            }
        }
        /// <summary>
        /// Function wait for data
        /// </summary>
        public void WaitForData(bool dataReceiveIsUTF8 = true)
        {
            try
            {
                if (dlgCallback == null)
                {
                    if (dataReceiveIsUTF8)
                        dlgCallback = new AsyncCallback(OnDataReceived);
                    else
                        dlgCallback = new AsyncCallback(OnDataReceived2);
                }
                CSocketPacket theSocketPkt = new CSocketPacket();
                theSocketPkt.thisSocket = m_clientSocket;
                m_asyncResult = m_clientSocket.BeginReceive(theSocketPkt.dataBuffer, 0, theSocketPkt.dataBuffer.Length, SocketFlags.None, dlgCallback, theSocketPkt);
            }
            catch (Exception ex)
            {
                m_errorMsg = ex.Message;
                ClientErrorEventCallback?.Invoke(m_errorMsg);
            }
        }
        /// <summary>
        /// This function will called when event received data occur
        /// </summary>
        /// <param name="asyn"></param>
        public void OnDataReceived(IAsyncResult asyn)
        {
            try
            {
                CSocketPacket theSocketId = (CSocketPacket)asyn.AsyncState;
                int iRx = 0;
                iRx = theSocketId.thisSocket.EndReceive(asyn);
                char[] chars = new char[iRx];
                //System.Text.Decoder decoder = System.Text.Encoding.UTF8.GetDecoder();
                //int charLen = decoder.GetChars(theSocketId.dataBuffer, 0, iRx, chars, 0);
                Encoding.UTF8.GetChars(theSocketId.dataBuffer, 0, iRx, chars, 0);
                System.String szData = new System.String(chars);
                this.m_ReceiveString = szData;

                //obj is char - m_ReceiveString is string
                ConnectionEventCallback?.Invoke(EConnectionEventClient.RECEIVEDATA, chars);
                WaitForData(true);
            }
            catch (Exception ex)
            {
                m_errorMsg = ex.Message;
                ClientErrorEventCallback?.Invoke(m_errorMsg);
            }
        }
        public void OnDataReceived2(IAsyncResult asyn)
        {
            try
            {
                CSocketPacket theSocketId = (CSocketPacket)asyn.AsyncState;
                int iRx = 0;
                iRx = theSocketId.thisSocket.EndReceive(asyn);
                char[] chars = new char[iRx];
                //System.Text.Decoder decoder = System.Text.Encoding.UTF8.GetDecoder();
                //int charLen = decoder.GetChars(theSocketId.dataBuffer, 0, iRx, chars, 0);
                Encoding.UTF7.GetChars(theSocketId.dataBuffer, 0, iRx, chars, 0);
                System.String szData = new System.String(chars);
                this.m_ReceiveString = szData;

                //obj is char - m_ReceiveString is string
                ConnectionEventCallback?.Invoke(EConnectionEventClient.RECEIVEDATA, chars);
                WaitForData(false);
            }
            catch (Exception ex)
            {
                m_errorMsg = ex.Message;
                ClientErrorEventCallback?.Invoke(m_errorMsg);
            }
        }
        #endregion

        #region Event
        public delegate void ConnectionEventHandler(EConnectionEventClient e, object obj);
        public event ConnectionEventHandler ConnectionEventCallback;

        public delegate void ErrorEventHandler(string errorMsg);
        public event ErrorEventHandler ClientErrorEventCallback;
        #endregion

        public class CSocketPacket
        {
            public Socket thisSocket;
            public byte[] dataBuffer = new byte[1024];
        }
    }
}
