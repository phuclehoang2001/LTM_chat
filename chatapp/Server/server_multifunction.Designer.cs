namespace Server
{
    partial class server_multifunction
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(server_multifunction));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.flpUsers = new System.Windows.Forms.FlowLayoutPanel();
            this.PortServer = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.IPServer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.imgIconChat = new System.Windows.Forms.ImageList(this.components);
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbSelectToSend = new System.Windows.Forms.ComboBox();
            this.txbMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.lsvMessage = new System.Windows.Forms.ListView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.imgIconTabs = new System.Windows.Forms.ImageList(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImageList = this.imgIconTabs;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(994, 574);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Controls.Add(this.flpUsers);
            this.tabPage1.Controls.Add(this.PortServer);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.IPServer);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.ImageIndex = 0;
            this.tabPage1.Location = new System.Drawing.Point(4, 31);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(986, 539);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Trang chủ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(390, 56);
            this.panel2.TabIndex = 14;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Gainsboro;
            this.panel3.Location = new System.Drawing.Point(5, 50);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(280, 3);
            this.panel3.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Server.Properties.Resources.online__1_;
            this.pictureBox1.Location = new System.Drawing.Point(5, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(44, 36);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(55, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(218, 25);
            this.label4.TabIndex = 1;
            this.label4.Text = "Người dùng đang online";
            // 
            // flpUsers
            // 
            this.flpUsers.AutoScroll = true;
            this.flpUsers.BackColor = System.Drawing.Color.Transparent;
            this.flpUsers.Location = new System.Drawing.Point(3, 62);
            this.flpUsers.Name = "flpUsers";
            this.flpUsers.Size = new System.Drawing.Size(390, 474);
            this.flpUsers.TabIndex = 13;
            // 
            // PortServer
            // 
            this.PortServer.Enabled = false;
            this.PortServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PortServer.Location = new System.Drawing.Point(513, 79);
            this.PortServer.Name = "PortServer";
            this.PortServer.Size = new System.Drawing.Size(276, 30);
            this.PortServer.TabIndex = 12;
            this.PortServer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(412, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 25);
            this.label3.TabIndex = 11;
            this.label3.Text = "Port:";
            // 
            // IPServer
            // 
            this.IPServer.Enabled = false;
            this.IPServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IPServer.Location = new System.Drawing.Point(513, 17);
            this.IPServer.Name = "IPServer";
            this.IPServer.Size = new System.Drawing.Size(276, 30);
            this.IPServer.TabIndex = 9;
            this.IPServer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(412, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 25);
            this.label1.TabIndex = 8;
            this.label1.Text = "IP server:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.cbSelectToSend);
            this.tabPage2.Controls.Add(this.txbMessage);
            this.tabPage2.Controls.Add(this.btnSend);
            this.tabPage2.Controls.Add(this.lsvMessage);
            this.tabPage2.ImageIndex = 1;
            this.tabPage2.Location = new System.Drawing.Point(4, 31);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(986, 539);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Nhắn tin";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Location = new System.Drawing.Point(227, 459);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(203, 50);
            this.panel1.TabIndex = 11;
            // 
            // button4
            // 
            this.button4.AutoEllipsis = true;
            this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.ImageIndex = 2;
            this.button4.ImageList = this.imgIconChat;
            this.button4.Location = new System.Drawing.Point(152, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(48, 44);
            this.button4.TabIndex = 2;
            this.button4.UseVisualStyleBackColor = true;
            // 
            // imgIconChat
            // 
            this.imgIconChat.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgIconChat.ImageStream")));
            this.imgIconChat.TransparentColor = System.Drawing.Color.Transparent;
            this.imgIconChat.Images.SetKeyName(0, "folder.png");
            this.imgIconChat.Images.SetKeyName(1, "picture.png");
            this.imgIconChat.Images.SetKeyName(2, "happiness.png");
            this.imgIconChat.Images.SetKeyName(3, "send-message.png");
            // 
            // button3
            // 
            this.button3.AutoEllipsis = true;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ImageIndex = 1;
            this.button3.ImageList = this.imgIconChat;
            this.button3.Location = new System.Drawing.Point(77, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(48, 44);
            this.button3.TabIndex = 1;
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.AutoEllipsis = true;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ImageIndex = 0;
            this.button2.ImageList = this.imgIconChat;
            this.button2.Location = new System.Drawing.Point(3, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(48, 44);
            this.button2.TabIndex = 0;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 459);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 16);
            this.label2.TabIndex = 10;
            this.label2.Text = "Chọn người gửi";
            // 
            // cbSelectToSend
            // 
            this.cbSelectToSend.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbSelectToSend.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbSelectToSend.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSelectToSend.FormattingEnabled = true;
            this.cbSelectToSend.IntegralHeight = false;
            this.cbSelectToSend.Location = new System.Drawing.Point(37, 478);
            this.cbSelectToSend.Name = "cbSelectToSend";
            this.cbSelectToSend.Size = new System.Drawing.Size(168, 24);
            this.cbSelectToSend.TabIndex = 9;
            // 
            // txbMessage
            // 
            this.txbMessage.AcceptsReturn = true;
            this.txbMessage.AcceptsTab = true;
            this.txbMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbMessage.Location = new System.Drawing.Point(450, 459);
            this.txbMessage.Multiline = true;
            this.txbMessage.Name = "txbMessage";
            this.txbMessage.Size = new System.Drawing.Size(423, 50);
            this.txbMessage.TabIndex = 7;
            // 
            // btnSend
            // 
            this.btnSend.FlatAppearance.BorderSize = 0;
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.ImageIndex = 3;
            this.btnSend.ImageList = this.imgIconChat;
            this.btnSend.Location = new System.Drawing.Point(879, 459);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(62, 50);
            this.btnSend.TabIndex = 8;
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lsvMessage
            // 
            this.lsvMessage.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lsvMessage.HideSelection = false;
            this.lsvMessage.HoverSelection = true;
            this.lsvMessage.Location = new System.Drawing.Point(37, 20);
            this.lsvMessage.Name = "lsvMessage";
            this.lsvMessage.Size = new System.Drawing.Size(904, 403);
            this.lsvMessage.TabIndex = 6;
            this.lsvMessage.UseCompatibleStateImageBehavior = false;
            this.lsvMessage.View = System.Windows.Forms.View.List;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tabControl2);
            this.tabPage3.ImageIndex = 2;
            this.tabPage3.Location = new System.Drawing.Point(4, 31);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(986, 539);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Nhóm";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Multiline = true;
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(986, 539);
            this.tabControl2.TabIndex = 2;
            // 
            // imgIconTabs
            // 
            this.imgIconTabs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgIconTabs.ImageStream")));
            this.imgIconTabs.TransparentColor = System.Drawing.Color.Transparent;
            this.imgIconTabs.Images.SetKeyName(0, "home.png");
            this.imgIconTabs.Images.SetKeyName(1, "chat.png");
            this.imgIconTabs.Images.SetKeyName(2, "group.png");
            // 
            // server_multifunction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 574);
            this.Controls.Add(this.tabControl1);
            this.Name = "server_multifunction";
            this.Text = "server_multifunction";
            this.Load += new System.EventHandler(this.server_multifunction_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TextBox IPServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbSelectToSend;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txbMessage;
        private System.Windows.Forms.ListView lsvMessage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox PortServer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ImageList imgIconChat;
        private System.Windows.Forms.ImageList imgIconTabs;
        private System.Windows.Forms.FlowLayoutPanel flpUsers;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel3;
    }
}