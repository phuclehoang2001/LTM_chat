using System;
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
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using COMMON;
using MESSAGE;
using Server;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Client
{
    public partial class LoginForm : Form
    {
        IPEndPoint IP;
        Socket client;
        Thread register;
        bool login = false;
        public LoginForm()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
    
        }

        private void Client_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void sendJson(object obj)
        {
            byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(obj);
            client.Send(jsonUtf8Bytes, jsonUtf8Bytes.Length, SocketFlags.None);
        }



        //kết nối tới server
        void Connect()
        {
            if (IPServer.Text == string.Empty || login == true)
                return; 
            //IP: địa chỉ của server
            IP = new IPEndPoint(IPAddress.Parse(IPServer.Text), int.Parse(Port.Text));
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                client.Connect(IP);
            }
            catch
            {
                MessageBox.Show("Không thể kết nối server!", "lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("Đã connect đến server");
            ChangeAttribute(btnLogin, true);
            ChangeAttribute(btnRegister, true);
            ChangeAttribute(btnConnect, false);

        }


        public void ChangeAttribute(Button btn, bool value)
        {
            btn.BeginInvoke(new MethodInvoker(() =>
            {
                btn.Enabled = value;
            }));
        }

        //nhận tin
        void ThreadReceive()
        {
            //gui
            byte[] data = new byte[1024];
            MESSAGE.LOGIN login = new MESSAGE.LOGIN(username.Text, password.Text);
            string jsonString = JsonSerializer.Serialize(login);
            COMMON.COMMON common = new COMMON.COMMON("LOGIN", jsonString);
            sendJson(common);

            // nhan
            int recv = client.Receive(data);
            jsonString = Encoding.ASCII.GetString(data, 0, recv);
            jsonString.Replace("\\u0022", "\"");
            COMMON.COMMON com = JsonSerializer.Deserialize<COMMON.COMMON>(jsonString);
            try
            {
                if (com != null && com.kind.Equals(com.REPLY))
                {
                    if (com.content == "OK")
                    {
                        MessageBox.Show("Đăng nhập thành công");
                        this.login = true;
                        this.Hide(); // ẩn form login
                        MainForm form = new MainForm(username.Text, client);
                        form.ShowDialog();
                        this.Close();
                    }
                    else 
                    {
                        MessageBox.Show("Đăng nhập thất bại");
                    }
                }
            } catch (Exception e)
            {
                MessageBox.Show(e.Message);
                client.Disconnect(true);
                client.Close();
            }
            
        }

        



        //phân mảnh
        byte[] Serialize(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, obj);
            
            return stream.ToArray();
        }

        //gom mảnh lại
        object Deserialize(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();

            return formatter.Deserialize(stream);
        }

        //Đóng kết nối khi đóng form
        private void Client_FormClosed(object sender, FormClosedEventArgs e)
        {
         
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Thread receive = new Thread(ThreadReceive);
            receive.IsBackground = true;
            receive.SetApartmentState(ApartmentState.STA);
            receive.Start();
            receive.Join();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
    
        }

        private void ThreadRegister()
        {
            byte[] data = new byte[1024];
            MESSAGE.LOGIN login = new MESSAGE.LOGIN(username.Text, password.Text);

            string jsonString = JsonSerializer.Serialize(login);

            COMMON.COMMON common = new COMMON.COMMON("REGISTER", jsonString);
            sendJson(common);

            int recv = client.Receive(data);
            jsonString = Encoding.ASCII.GetString(data, 0, recv);
            COMMON.COMMON com = JsonSerializer.Deserialize<COMMON.COMMON>(jsonString);

            try
            {
                if (com != null && com.kind.Equals(com.REPLY))
                {
                    if (com.content == "OK")
                    {
                        MessageBox.Show("Đăng ký thành công");
                    }
                    else
                    {
                        MessageBox.Show("Đăng ký thất bại");
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
 
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            register = new Thread(new ThreadStart(this.ThreadRegister));
            register.IsBackground = true;
            register.Start();
        }
    }
}
