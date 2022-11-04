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
using Server;
using System.Text.Json;
using System.Text.Json.Serialization;
using MESSAGE;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Runtime.Remoting.Contexts;
using COMMON;
using Client;
using Server;
using Client.Properties;
using System.Xml.Linq;


namespace Server
{
    public partial class MainForm : Form
    {
        Socket client;
        bool login;
        private string username;
        private string receiver = "";
        private string groupRecevier = "";
        private List<string> users;
        private Dictionary<string, List<string>> groups;
        Thread mainThread;
        //bug: login more 2 time, logout remove UI server, update list_user when register
        public MainForm(MESSAGE.INITDATA data,Socket socket)
        {
            InitializeComponent();
            this.username = data.username;
            this.client = socket;
            this.users = data.users;
            this.groups = data.groups;
            this.login = true;
        }
        public void ChangeAttribute(Label label, string information)
        {
            label.BeginInvoke(new MethodInvoker(() =>
            {
                label.Text = information;
            }));
        }

        private void client_Load(object sender, EventArgs e)
        {
            foreach (string username in users)
            {
                if (!username.Equals(this.username) && !username.Equals("Tất cả"))
                {
                    ItemClient client = new ItemClient
                    {
                        Socket = null,
                        ClientName = username,
                        ClientImg = Resources.programmer,
                        Status = true
                    };
                    flpUsers.Invoke((MethodInvoker)(() => flpUsers.Controls.Add(client)));
                    client.ItemClick += Client_ItemClick;
                }
            }
            if (groups.Count > 0)
            {
                foreach (KeyValuePair<string, List<string>> item in groups)
                {
                    ItemGroup itemGroup = new ItemGroup
                    {
                        GroupName = item.Key,
                        GroupImg = Resources.group_chat,
                        Members = item.Value,
                        Status = true
                    };
                    flpUsers.Invoke((MethodInvoker)(() => flpGroups.Controls.Add(itemGroup)));
                    itemGroup.ItemClick += Group_ItemClick;
                }
            }

            ChangeAttribute(lbWelcome,  "Hello " + this.username);
            mainThread = new Thread(new ThreadStart(this.ThreadTask));
            mainThread.IsBackground = true;
            mainThread.Start();
        }

        private void Client_ItemClick(object sender, EventArgs e)
        {
            ItemClient item = sender as ItemClient;
            if (this.username.Equals(item.ClientName))
                return;
            this.receiver = item.ClientName;
            this.groupRecevier = "";
            ChangeAttribute(lbReceiver, "Send to <user>: "+ this.receiver  );
        }

        private void Group_ItemClick(object sender, EventArgs e)
        {
            ItemGroup item = sender as ItemGroup;
            this.groupRecevier = item.GroupName;
            this.receiver = "";
            ChangeAttribute(lbReceiver, "Send to <group>: " + this.groupRecevier);
        }

        private void sendJson(Socket client, object obj)
        {
            byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(obj);
            client.Send(jsonUtf8Bytes, jsonUtf8Bytes.Length, SocketFlags.None);
        }
        /// <summary>
        /// Thêm cái hàm ở dưới, coi reference để biết sửa thêm chỗ nào
        /// </summary>
        private void ListView1_SelectedIndexChanged_UsingItems(object sender, System.EventArgs e)
        {

            ListView.SelectedListViewItemCollection breakfast =
                this.listView2.SelectedItems;
            foreach (ListViewItem item in breakfast)
            {
                DownloadFile(item.Text);
            }
        }

        //nhận tin
        private void ThreadTask()
        {
            byte[] data = new byte[1024 * 10000];
            try
            {
                while (login)
                {
                    int recv = client.Receive(data);
                    string jsonString = Encoding.ASCII.GetString(data, 0, recv);
                    jsonString = jsonString.Replace("\0", "");
                    COMMON.COMMON com = JsonSerializer.Deserialize<COMMON.COMMON>(jsonString);
                    if (com != null)
                    {
                        switch (com.kind)
                        {
                            case "MESSAGE_ALL":
                                MESSAGE.MESSAGE_ALL mes_all = JsonSerializer.Deserialize<MESSAGE.MESSAGE_ALL>(com.content);
                                AddMessage(mes_all.UsernameSender + " >> " + mes_all.Content);
                                break;
                            case "MESSAGE":
                                MESSAGE.MESSAGE mes = JsonSerializer.Deserialize<MESSAGE.MESSAGE>(com.content);
                                if (mes.UsernameReceiver != this.username)
                                {
                                    AddMessage(mes.UsernameSender + " to " + mes.UsernameReceiver + " <group>: " + mes.Content);
                                } else
                                {
                                    AddMessage(mes.UsernameSender + " to " + mes.UsernameReceiver + " <me>: " + mes.Content);
                                }
                                break;
                            case "UploadFile":
                                MESSAGE.FILE ufile = JsonSerializer.Deserialize<MESSAGE.FILE>(com.content);
                                AddMessage(ufile.fname);
                                break;
                            case "DownLoadFile":
                                MESSAGE.FILE file = JsonSerializer.Deserialize<MESSAGE.FILE>(com.content);
                                try
                                {

                                    string receivedPath = @"E:\document\downloadfile\";
                                    int recvdata = Buffer.ByteLength(file.data);
                                    int fileNameLen = BitConverter.ToInt32(file.data, 0);


                                    BinaryWriter bWrite = new BinaryWriter(File.Open(receivedPath + file.fname, FileMode.Append));
                                    bWrite.Write(file.data, 4 + fileNameLen, recvdata - 4 - fileNameLen);
                                    bWrite.Close();
                                }
                                catch
                                {

                                }
                                break;
                            case "CheckUser":
                                
                                if (com.content == null)
                                {
                                    MessageBox.Show("Username không tồn tại hoặc không chính xác");    
                                }
                                else
                                {
                                    MESSAGE.CHECKUSER check = JsonSerializer.Deserialize<MESSAGE.CHECKUSER>(com.content);
                                    AddUser(check.username);          
                                }
                                break;
                            case "ADDGROUP":

                                if (com.content == null)
                                {
                                    MessageBox.Show("Nhóm đã tồn tại, tạo thất bại!");
                                }
                                else
                                {
                                    MESSAGE.ADDGROUP group = JsonSerializer.Deserialize<MESSAGE.ADDGROUP>(com.content);
                                    ItemGroup itemGroup = new ItemGroup
                                    {
                                        GroupName = group.groupName,
                                        GroupImg = Resources.group_chat,
                                        Members = group.members,
                                        Status = true
                                    };
                                    itemGroup.ItemClick += Group_ItemClick;
                                    flpUsers.Invoke((MethodInvoker)(() => flpGroups.Controls.Add(itemGroup)));
                            
                                }
                                break;
                            case "ADDUSER":
                                MESSAGE.USERADD useradd = JsonSerializer.Deserialize<MESSAGE.USERADD>(com.content);
                                ItemClient newclient = new ItemClient
                                {
                                    Socket = null,
                                    ClientName = useradd.username,
                                    ClientImg = Resources.programmer,
                                    Status = true
                                };
                                newclient.ItemClick += Client_ItemClick;
                                flpUsers.Invoke((MethodInvoker)(() => flpUsers.Controls.Add(newclient)));
                                break;
                            default:
                                break;

                        }

                    }
                }
                client.Disconnect(true);
                client.Close();
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
            listView2.Items.Add(listViewItem);
        }
        //phân mảnh data -> byte
        byte[] Serialize(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            return stream.ToArray();
        }

        //gom mảnh data 
        object Deserialize(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();

            return formatter.Deserialize(stream);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }



        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txbMessage.Text == string.Empty) return;
            if (this.receiver != "")
            {
                MESSAGE.MESSAGE mes = new MESSAGE.MESSAGE(this.username, this.receiver, txbMessage.Text);
                string jsonString = JsonSerializer.Serialize(mes);
                COMMON.COMMON common = new COMMON.COMMON("MESSAGE", jsonString);
                sendJson(client, common);
                AddMessage(this.username + " to " + receiver + " >> " + txbMessage.Text);
            } else if(this.groupRecevier != "")
            {
                MESSAGE.MESSAGE mes = new MESSAGE.MESSAGE(this.username, this.groupRecevier, txbMessage.Text);
                string jsonString = JsonSerializer.Serialize(mes);
                COMMON.COMMON common = new COMMON.COMMON("MESSAGE", jsonString);
                sendJson(client, common);
            }     
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MESSAGE.LOGOUT logout = new MESSAGE.LOGOUT(this.username);
            string jsonString = JsonSerializer.Serialize(logout);
            COMMON.COMMON common = new COMMON.COMMON("LOGOUT", jsonString);
            sendJson(client, common);
            login= false;
            this.Close();
        }


        private void UploadFile(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog() { Multiselect = true, ValidateNames = true, Filter = "All Files|*.*" };
            ofd.ShowDialog();
            
            FileInfo fi = new FileInfo(ofd.FileName);
            //AppendTextBox(fi.Name);

            byte[] fileNameByte = Encoding.ASCII.GetBytes(fi.Name);

            byte[] fileData = File.ReadAllBytes(fi.DirectoryName + fi.Name);
            byte[] clientData = new byte[4 + fileNameByte.Length + fileData.Length];
            byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);

            fileNameLen.CopyTo(clientData, 0);
            fileNameByte.CopyTo(clientData, 4);
            fileData.CopyTo(clientData, 4 + fileNameByte.Length);

            MESSAGE.FILE mes = new MESSAGE.FILE(this.username, this.receiver,fi.FullName, fi.Name, fi.DirectoryName, clientData);
            string jsonString = JsonSerializer.Serialize(mes);
            COMMON.COMMON common = new COMMON.COMMON("UploadFile", jsonString);
            sendJson(client, common);
        }


        
        private void DownloadFile(string fname)
        {
            MESSAGE.DOWNFILE mes = new MESSAGE.DOWNFILE(this.username, this.receiver, fname);
            string jsonString = JsonSerializer.Serialize(mes);
            COMMON.COMMON common = new COMMON.COMMON("DownLoadFile", jsonString);
            sendJson(client, common);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            try
            {
                if (mainThread != null)
                {
                    Application.Exit();
                }
                    
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
            login = false;
        }

        private void showUsers(FlowLayoutPanel panel)
        {
            if (panel.Visible == false)
            {
                panel.Visible = true;
            }
            else
            {
                panel.Visible = false;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            showUsers(flpUsers);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            showUsers(flpGroups);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ListView.SelectedListViewItemCollection icons =
        this.listView1.SelectedItems;

            string icon="";
            foreach (ListViewItem item in icons)
            {
                icon += item.Text;
            }

            // Output the price to TextBox1.
            txbMessage.Text += icon;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(listView1.Visible==false)
                listView1.Visible = true;
            else if(listView1.Visible == true)
                listView1.Visible = false;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void listUserGroup_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        void AddUser(string userName)
        {
            if (InvokeRequired)
            {
                try { this.Invoke(new Action<string>(AddUser), new object[] { userName }); }
                catch (Exception) { }
                return;
            }

            var listViewItem = new ListViewItem(userName);
            listUserGroup.Items.Add(listViewItem);

        }

        private void button9_Click(object sender, EventArgs e)
        {
            string userName = inputName.Text;
            if (userName == this.username) return;
            for (int i = 0; i < listUserGroup.Items.Count; i++)
            {
                if (userName.Equals(listUserGroup.Items[i].Text))
                {
                    MessageBox.Show("Đã thêm " + userName + " !");
                    return;
                }         
            }
            MESSAGE.CHECKUSER check = new MESSAGE.CHECKUSER(userName);
            string jsonString = JsonSerializer.Serialize(check);
            COMMON.COMMON common = new COMMON.COMMON("CheckUser", jsonString);
            sendJson(client, common);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string groupName = GroupName.Text;
            if (GroupName.Text == "")
            {
                MessageBox.Show("Hãy nhập tên nhóm!");
                return;
            }
            if(listUserGroup.Items.Count <2)
            {
                MessageBox.Show("Thêm ít nhất 2 thành viên để tạo nhóm");
            } else
            {
                List<string> members = new List<string>();
                for(int i = 0; i< listUserGroup.Items.Count; i++)
                {
                    members.Add(listUserGroup.Items[i].Text);
                }
                members.Add(this.username);

                MESSAGE.ADDGROUP newGroup = new MESSAGE.ADDGROUP(groupName, members);
                string jsonString = JsonSerializer.Serialize(newGroup);
                COMMON.COMMON common = new COMMON.COMMON("ADDGROUP", jsonString);
                sendJson(client, common);       
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            listUserGroup.Invoke((MethodInvoker)(() => listUserGroup.Items.Clear()));
            GroupName.Text = "";
            inputName.Text = "";

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            login = false;
            MESSAGE.LOGOUT logout = new MESSAGE.LOGOUT(this.username);
            string jsonString = JsonSerializer.Serialize(logout);
            COMMON.COMMON common = new COMMON.COMMON("LOGOUT", jsonString);
            sendJson(client, common);         
        }
    }
}
