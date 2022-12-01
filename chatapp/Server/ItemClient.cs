using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class ItemClient : UserControl
    {
        private Image clientImg;
        private string clientName;
        private string clientIP;
        private Socket socket;
        private bool status;
        private bool pressed = false;
        public ItemClient()
        {
            InitializeComponent();
            
        }

        public Image ClientImg
        {
            get { return clientImg; }
            set {
                clientImg = value;
                pictureBox.Image = value;
            }
        }

        public string ClientIP
        {
            get { return clientIP; }
            set {
                clientIP = value;
            }
        }

        public string ClientName
        {
            get { return clientName; }
            set {
                clientName = value;
                lbName.Text = value;
            }
        }

        public Socket Socket
        {
            get { return socket; }
            set { 
                socket = value; 
            }
        }

        public bool Status
        {
            get { return status; }
            set
            {
                try
                {
                    status = value;
                    this.isOnline();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                
            }
        }

        private void isOnline()
        {
           
            if (this.IsHandleCreated)
            { 
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(this.isOnline));     
                    return;
                }
                if (status)
                {
                    imgStatus.Visible = true;
                }
                else
                {
                    imgStatus.Visible = false;
                }
            } 
           
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void ItemClient_MouseEnter(object sender, EventArgs e)
        {
    
        }

        private void ItemClient_MouseLeave(object sender, EventArgs e)
        {
          
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            this.panel1.BackColor = Color.Silver;
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            this.panel1.BackColor = Color.Transparent;
        }

        public event EventHandler ItemClick;

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ItemClick(this, new EventArgs());     
        }

        private void lbName_Click(object sender, EventArgs e)
        {

        }

        private void ItemClient_Load(object sender, EventArgs e)
        {
            this.isOnline();
        }
    }
}
