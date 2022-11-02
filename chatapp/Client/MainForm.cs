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


namespace Server
{
    public partial class MainForm : Form
    {
        Socket client;
        bool login;
        private string username;
        private string receiver = "";
        Thread mainThread;

        public MainForm(string username, Socket client)
        {
            InitializeComponent();
            this.username = username;
            this.login = true;
            this.client = client;
        }
        public void ChangeAttribute(Label label, string username, string sub)
        {
            label.BeginInvoke(new MethodInvoker(() =>
            {
                label.Text = sub + username;
            }));
        }

        private void client_Load(object sender, EventArgs e)
        {
            for (int i = 1; i < 5; i++)
            {
                ItemClient client = new ItemClient
                {
                    Socket = null,
                    ClientName = "user" + i,
                    ClientIP = "ip" + i,
                    ClientImg = Resources.programmer,
                    Status = true
                };
                flpUsers.Invoke((MethodInvoker)(() => flpUsers.Controls.Add(client)));
                client.ItemClick += Client_ItemClick;
            }

            for (int i = 1; i < 5; i++)
            {
                ItemClient group = new ItemClient
                {
                    Socket = null,
                    ClientName = "group" + i,
                    ClientIP = "..." + i,
                    ClientImg = Resources.programmer,
                    Status = true
                };
                flpUsers.Invoke((MethodInvoker)(() => flpGroups.Controls.Add(group)));
            }

            ChangeAttribute(lbWelcome, this.username, "Hello ");
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
            ChangeAttribute(lbReceiver, this.receiver, "Send to ");
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
                                AddMessage(mes.UsernameSender + " to " + mes.UsernameReceiver + " (me) >> " + mes.Content);
                                break;
                            ////////////////////////////////////////Thêm case
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
            if (this.receiver == string.Empty || txbMessage.Text == string.Empty) return;
            MESSAGE.MESSAGE mes = new MESSAGE.MESSAGE(this.username, this.receiver, txbMessage.Text);
            string jsonString = JsonSerializer.Serialize(mes);
            COMMON.COMMON common = new COMMON.COMMON("MESSAGE", jsonString);
            sendJson(client, common);
            AddMessage(this.username + " to " + receiver + " >> " + txbMessage.Text);
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

            MESSAGE.FILE mes = new MESSAGE.FILE(this.username, this.receiver, fi.Name, fi.DirectoryName, clientData);
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
                    //mainThread.Abort();
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
    }
}
