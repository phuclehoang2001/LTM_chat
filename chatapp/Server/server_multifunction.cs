using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Server.Properties;
using System.Text.Json;
using System.Text.Json.Serialization;
using MESSAGE;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Net.NetworkInformation;

namespace Server
{
    public partial class server_multifunction : Form
    {
        IPEndPoint IP;
        Socket server;
        Dictionary<string, string> ListAccount;// username, pass
        Dictionary<string, ItemClient> ListClient;// username, viewcontrol (infor_user: ava,socket,fullname...)
        Dictionary<string, List<string>> ListGroup;// groupname, members

        private const int PORT_NUMBER = 2008;

        public server_multifunction()
        {
            InitializeComponent();
        }

       

        private void CreateUsers()
        {
            ListAccount = new Dictionary<string, string>();
            for (int i = 0; i < 9; i++)
                ListAccount.Add("user" + i.ToString(), "123");

            // tạo group với groupname: groupdemo, members: user1,user2,user3
            ListGroup = new Dictionary<string, List<string>>();
            ListGroup.Add("groupdemo", new List<string>
            {
                ListAccount.ElementAt(1).Key,
                ListAccount.ElementAt(2).Key,
                ListAccount.ElementAt(3).Key,
            });

        }
        // Thiết lập ip từ máy chủ hiện tại khi load form
        private void server_multifunction_Load(object sender, EventArgs e)
        {
            CreateUsers();
            //string hostName = Dns.GetHostName();
            //string myIP = null;// = Dns.GetHostByName(hostName).AddressList[0].ToString();
            ////IPAddress[] MangIP = Dns.GetHostByName(hostName).AddressList;
            //foreach (IPAddress IP in MangIP)
            //    if (IP.ToString().Contains("."))
            //    {
            //        myIP = IP.ToString();
            //        break;
            //    }
            string myIP = null;
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {      
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 )
                {
                    if (ni.Name.Equals("Wi-Fi"))
                    {
                        foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                myIP = ip.Address.ToString();
                        
                            }
                        }
                    }         
                }
            }
        
            if (myIP == null) this.Close();
            IPServer.Text = myIP;
            PortServer.Text = PORT_NUMBER.ToString();
            ListClient = new Dictionary<string, ItemClient>();
            //Gán giá trị mặc định cho combobox ( Chọn tất cả )
            ItemClient initValue = new ItemClient
            {
                ClientName = "Tất cả",
                ClientIP = "0000000",
            };
            ListClient.Add("Tất cả", initValue);
            cbSelectToSend.DataSource = new BindingSource(ListClient, null);
            cbSelectToSend.DisplayMember = "Key";
            cbSelectToSend.ValueMember = "Value";
            Connect();
        }


        // kết nối -> tạo server
        void Connect()
        {
            //IP: địa chỉ của server từ máy hiên tại
            IP = new IPEndPoint(IPAddress.Parse(IPServer.Text), PORT_NUMBER);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(IP);
            server.Listen(100);
            /// thread đợi connect từ client
            Thread listen = new Thread(ThreadListen);
            listen.IsBackground = true;
            listen.Start(server);
        }

        private void ThreadListen(object obj)
        {
            Socket server = obj as Socket;
            try
            {
                while (true)
                {
                    Socket socket = server.Accept();
                    Thread receive = new Thread(ThreadReceive);
                    receive.IsBackground = true;
                    receive.Start(socket);
                }
            }
            catch
            {
                IP = new IPEndPoint(IPAddress.Parse(IPServer.Text), 2008);
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            }
        }

        private void sendJson(Socket client, object obj)
        {
            byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(obj);
            client.Send(jsonUtf8Bytes, jsonUtf8Bytes.Length, SocketFlags.None);
        }

      
        void ThreadReceive(object obj)
        {
            bool active = false;
            Socket socket = obj as Socket;
            try
            {
                while (!active)
                {
                    byte[] data = new byte[1024 * 10000];
                    int recv = socket.Receive(data);
                    if (recv == 0) return;
                    string jsonString = Encoding.ASCII.GetString(data, 0, recv);
                    COMMON.COMMON com = JsonSerializer.Deserialize<COMMON.COMMON>(jsonString);
                    if (com != null)
                    {
                        if (com.content != null)
                        {
                            switch (com.kind)
                            {
                                case "LOGIN":
                                    MESSAGE.LOGIN login = JsonSerializer.Deserialize<LOGIN>(com.content);
                                    if (login != null && login.username != null && login.password != null
                                        && ListAccount.Keys.Contains(login.username)
                                        && login.password.Equals(ListAccount[login.username])
                                        && !ListClient.Keys.Contains(login.username))
                                    {

                                        ItemClient client = new ItemClient
                                        {
                                            Socket = socket,
                                            ClientName = login.username,
                                            ClientIP = socket.RemoteEndPoint.ToString(),
                                            ClientImg = Resources.programmer,
                                            Status = true
                                        };

                                        // Hàm invoke dùng để thay đổi control vào UI trong 1 thread
                                        flpUsers.Invoke((MethodInvoker)(() => flpUsers.Controls.Add(client)));
                                        // thêm vào list
                                        ListClient.Remove(login.username);
                                        ListClient.Add(login.username, client);
                                        ///Thêm vào datasource
                                        cbSelectToSend.DataSource = new BindingSource(ListClient, null);
                                        AddMessage("\t\t\tClient " + login.username + " đã tham gia");


                                        //// transfer data to client ListAccount.Keys.ToList();
                                        Dictionary<string, bool> userInit = new Dictionary<string, bool>();
                                        foreach (KeyValuePair<string, string> item in ListAccount)
                                        {
                                            bool onlineStatus = false;
                                            if (ListClient.ContainsKey(item.Key))
                                            {
                                                onlineStatus = true;
                                            }
                                            userInit.Add(item.Key, onlineStatus);
                                        }
                                        Dictionary<string, List<string>> groups = new Dictionary<string, List<string>>();
                                        foreach (KeyValuePair<string, List<string>> item in ListGroup)
                                        {
                                            if (item.Value.Contains(login.username))
                                            {
                                                groups.Add(item.Key, item.Value);
                                            }
                                        }
                                        // du lieu khoi tao
                                        MESSAGE.INITDATA initData = new INITDATA(userInit, groups, login.username);
                                        string jsonLoginResult = JsonSerializer.Serialize(initData);
                                        com = new COMMON.COMMON("LOGIN_RESULT", jsonLoginResult);
                                        sendJson(socket, com);

                                        // trang thai online
                                        MESSAGE.ONLINE online = new ONLINE(login.username, true);
                                        string jsonOnline = JsonSerializer.Serialize(online);
                                        com = new COMMON.COMMON("ONLINE", jsonOnline);
                                        foreach (KeyValuePair<string, ItemClient> item in ListClient)
                                        {
                                            if (item.Value.ClientName != "Tất cả" && item.Value.ClientName != login.username)
                                            {
                                                Socket friend = item.Value.Socket;
                                                sendJson(friend, com);
                                            }
                                        }
                                        active = true;
                                    }
                                    else
                                    {
                                        com = new COMMON.COMMON("LOGIN_RESULT", "FAILED");
                                        sendJson(socket, com);
                                    }
                                    break;
                                case "REGISTER":
                                    {
                                        MESSAGE.LOGIN register = JsonSerializer.Deserialize<LOGIN>(com.content);
                                        if (register != null && register.username != null && !ListAccount.Keys.Contains(register.username))
                                        {
                                            ListAccount.Add(register.username, register.password);
                                            com = new COMMON.COMMON("REPLY", "OK");
                                            sendJson(socket, com);


                                            MESSAGE.USERADD useradd = new USERADD(register.username);
                                            string jsonuseradd = JsonSerializer.Serialize(useradd);
                                            com = new COMMON.COMMON("ADDUSER", jsonuseradd);
                                            foreach (KeyValuePair<string, ItemClient> item in ListClient)
                                            {
                                                if (item.Value.ClientName != "Tất cả")
                                                {
                                                    Socket friend = item.Value.Socket;
                                                    sendJson(friend, com);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            com = new COMMON.COMMON("REPLY", "CANCEL");
                                            sendJson(socket, com);
                                            return;
                                        }
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            com = new COMMON.COMMON("REPLY", "CANCEL");
                            sendJson(socket, com);
                        }
                    }

                }
            }
            catch
            {

            }

            // login success
            try
            {
                while (active)
                {
                    byte[] data = new byte[1024 * 10000];
                    int recv = socket.Receive(data);
                    if (recv == 0) continue;
                    string s = Encoding.ASCII.GetString(data, 0, recv);
                    COMMON.COMMON com = JsonSerializer.Deserialize<COMMON.COMMON>(s);
                    if (com != null && com.content != null)
                    {
                        switch (com.kind)
                        {
                            case "LOGOUT":
                                MESSAGE.LOGOUT logout = JsonSerializer.Deserialize<MESSAGE.LOGOUT>(com.content);
                                flpUsers.Invoke((MethodInvoker)(() => flpUsers.Controls.Remove(ListClient[logout.username])));
                                AddMessage("\t\t\tClient " + logout.username + " đã rời chat");
                                ListClient[logout.username].Socket.Close();
                                ListClient.Remove(logout.username);
                                cbSelectToSend.DataSource = new BindingSource(ListClient, null);


                                // trang thai online
                                MESSAGE.ONLINE online = new ONLINE(logout.username, false);
                                string jsonOnline = JsonSerializer.Serialize(online);
                                com = new COMMON.COMMON("ONLINE", jsonOnline);
                                foreach (KeyValuePair<string, ItemClient> item in ListClient)
                                {
                                    if (item.Value.ClientName != "Tất cả" && item.Value.ClientName != logout.username)
                                    {
                                        Socket friend = item.Value.Socket;
                                        sendJson(friend, com);
                                    }
                                }
                                active = false;
                                break;
                            case "MESSAGE":
                                MESSAGE.MESSAGE mes = JsonSerializer.Deserialize<MESSAGE.MESSAGE>(com.content);
                                if (mes != null && mes.UsernameReceiver != null)
                                {
                                    if (ListClient.Keys.Contains(mes.UsernameReceiver))
                                    {
                                        AddMessage(mes.UsernameSender + " to " + mes.UsernameReceiver + " : " + mes.Content);
                                        Socket friend = ListClient[mes.UsernameReceiver].Socket;
                                        friend.Send(data, recv, SocketFlags.None);
                                    }
                                    else if (ListGroup.Keys.Contains(mes.UsernameReceiver))
                                    {
                                        List<string> members = ListGroup[mes.UsernameReceiver];
                                        foreach (string client in members)
                                        {
                                            if (ListClient.Keys.Contains(client))
                                            {
                                                Socket friend = ListClient[client].Socket;
                                                if (friend != null)
                                                {
                                                    friend.Send(data, recv, SocketFlags.None);
                                                    AddMessage(mes.UsernameSender + " to  " + mes.UsernameReceiver + " <group>: " + mes.Content);
                                                }
                                            }

                                        }
                                    }
                                }
                                break;
                            case "UploadFile":
                                {
                                    MESSAGE.FILE file = JsonSerializer.Deserialize<MESSAGE.FILE>(com.content);
                                    if (file != null && file.usernameReceiver != null)
                                    {
                                        if (ListClient.Keys.Contains(file.usernameReceiver))
                                        {
                                            string[] duoihinh = { ".jpeg", ".jpg", ".pnj", ".gif" };
                                            AddMessage(file.usernameSender + " to " + file.usernameReceiver + " : " + file.fname + Environment.NewLine);
                                            Socket friend = ListClient[file.usernameReceiver].Socket;
                                            friend.Send(data, recv, SocketFlags.None);
                                            try
                                            {

                                                string receivedPath = @"E:\document\uploadfile\";
                                                byte[] clientData = new byte[1024 * 10000];
                                                int recvdata = Buffer.ByteLength(file.data);
                                                int fileNameLen = BitConverter.ToInt32(file.data, 0);

                                                BinaryWriter bWrite = new BinaryWriter(File.Open(receivedPath + file.fname, FileMode.Append));
                                                bWrite.Write(file.data, 4 + fileNameLen, recvdata - 4 - fileNameLen);
                                                bWrite.Close();
                                            }
                                            catch
                                            {

                                            }
                                        }

                                        if (ListGroup.Keys.Contains(file.usernameReceiver))
                                        {
                                            List<string> members = ListGroup[file.usernameReceiver];
                                            foreach (string client in members)
                                            {
                                                if (ListClient.Keys.Contains(client))
                                                {
                                                    Socket ulfriend = ListClient[client].Socket;
                                                    if (ulfriend != null)
                                                    {
                                                        ulfriend.Send(data, recv, SocketFlags.None);
                                                        AddMessage(file.usernameSender + " to " + file.usernameReceiver + " <group>: " + file.fname);
                                                    }
                                                }

                                            }
                                            try
                                            {

                                                string receivedPath = @"E:\document\uploadfile\";
                                                byte[] clientData = new byte[1024 * 10000];
                                                int recvdata = Buffer.ByteLength(file.data);
                                                int fileNameLen = BitConverter.ToInt32(file.data, 0);

                                                BinaryWriter bWrite = new BinaryWriter(File.Open(receivedPath + file.fname, FileMode.Append));
                                                bWrite.Write(file.data, 4 + fileNameLen, recvdata - 4 - fileNameLen);
                                                bWrite.Close();
                                            }
                                            catch
                                            {

                                            }
                                        }


                                    }
                                }
                                break;
                            ////////////////////////////Hàm Down 
                            case "DownLoadFile":
                                {
                                    MESSAGE.DOWNFILE dfile = JsonSerializer.Deserialize<MESSAGE.DOWNFILE>(com.content);

                                    byte[] fileNameByte = Encoding.ASCII.GetBytes(dfile.fname);

                                    byte[] fileData = File.ReadAllBytes(@"E:\document\uploadfile\" + dfile.fname);
                                    byte[] clientData = new byte[4 + fileNameByte.Length + fileData.Length];
                                    byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);

                                    fileNameLen.CopyTo(clientData, 0);
                                    fileNameByte.CopyTo(clientData, 4);
                                    fileData.CopyTo(clientData, 4 + fileNameByte.Length);

                                    MESSAGE.FILE df = new MESSAGE.FILE(dfile.usernameSender, dfile.usernameReceiver, @"D:\C free\" + dfile.fname, dfile.fname, dfile.path, clientData);
                                    string jsonStringFile = JsonSerializer.Serialize(df);
                                    COMMON.COMMON common = new COMMON.COMMON("DownLoadFile", jsonStringFile);
                                    sendJson(socket, common);
                                }
                                break;
                            case "CheckUser":
                                MESSAGE.CHECKUSER check = JsonSerializer.Deserialize<MESSAGE.CHECKUSER>(com.content);
                                string jsonCheck = JsonSerializer.Serialize(check);
                                if (check != null && check.username != null
                                    && ListAccount.Keys.Contains(check.username))
                                {
                                    com = new COMMON.COMMON("CheckUser", jsonCheck);
                                    sendJson(socket, com);
                                }
                                else
                                {
                                    com = new COMMON.COMMON("CheckUser", null);
                                    sendJson(socket, com);
                                }
                                break;
                            case "ADDGROUP":
                                MESSAGE.ADDGROUP group = JsonSerializer.Deserialize<MESSAGE.ADDGROUP>(com.content);
                                string jsongroup = JsonSerializer.Serialize(group);
                                if (group == null || ListGroup.Keys.Contains(group.groupName))
                                {
                                    com = new COMMON.COMMON("ADDGROUP", null);
                                }
                                else
                                {
                                    ListGroup.Add(group.groupName, group.members);
                                    foreach (string client in group.members)
                                    {
                                        if (ListClient.Keys.Contains(client))
                                        {
                                            Socket friend = ListClient[client].Socket;
                                            com = new COMMON.COMMON("ADDGROUP", jsongroup);
                                            sendJson(friend, com);
                                        }

                                    }

                                }
                                break;

                        }
                    }
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message); 
            }

        }

        void AddMessage(string s)
        {
            if (InvokeRequired)
            {
                try { this.Invoke(new Action<string>(AddMessage), new object[] { s }); }
                catch (Exception) { }
                return;
            }

            var listViewItem = new ListViewItem(s);
            lsvMessage.Items.Add(listViewItem);
            txbMessage.Text = "";
            txbMessage.Focus();
        }
       

        private void button1_Click(object sender, EventArgs e)
        {
            Connect();
        }


        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txbMessage.Text == string.Empty) return;
            if (cbSelectToSend.SelectedIndex == 0)
            {
                foreach (KeyValuePair<string, ItemClient> item in ListClient)
                {
                    if (!item.Key.Equals("Tất cả"))
                        Send(item.Value, "MESSAGE_ALL");
                }
                AddMessage("Server >> " + txbMessage.Text);
            }
            else
            {
                KeyValuePair<string, ItemClient> item = (KeyValuePair<string, ItemClient>)cbSelectToSend.SelectedItem;
                Send(item.Value, "MESSAGE");
                AddMessage("Server to " + item.Key + " : " + txbMessage.Text);
            }
            txbMessage.Clear();
        }

        //gửi tin
        void Send(ItemClient client, string kind)
        {
            string jsonString;
            COMMON.COMMON common;
            switch (kind)
            {
                case "MESSAGE_ALL":
                    MESSAGE.MESSAGE_ALL mes_all = new MESSAGE.MESSAGE_ALL("Server", txbMessage.Text);
                    jsonString = JsonSerializer.Serialize(mes_all);
                    common = new COMMON.COMMON(kind, jsonString);
                    sendJson(client.Socket, common);
                    break;
                case "MESSAGE":
                    MESSAGE.MESSAGE mes = new MESSAGE.MESSAGE("Server", client.ClientName, txbMessage.Text);
                    jsonString = JsonSerializer.Serialize(mes);
                    common = new COMMON.COMMON(kind, jsonString);
                    sendJson(client.Socket, common);
                    break;
                default:
                    break;
            }

        }
    }
}
