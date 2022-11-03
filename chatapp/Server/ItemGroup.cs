using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class ItemGroup : UserControl
    {
        private Image groupImg;
        private string groupName;
        private bool status;
        private bool pressed = false;
        private List<string> members;
        public ItemGroup()
        {
            InitializeComponent();
        }

        public Image GroupImg
        {
            get { return groupImg; }
            set {
                groupImg = value;
                pictureBox.Image = value;
            }
        }

        public List<string> Members
        {
            get { return members; }
            set
            {
                members = value;
            }
        }

        public string GroupName
        {
            get { return groupName; }
            set {
                groupName = value;
                lbName.Text = value;
            }
        }

        

        public bool Status
        {
            get { return status; }
            set
            {
                status = value;
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
    }
}
