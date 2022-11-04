using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MESSAGE
{
    public class MESSAGE
    {
        public string UsernameSender { get; set; }
        public string UsernameReceiver { get; set; }
        public string Content { get; set; }
        public MESSAGE(string usernameSender, string usernameReceiver, string content)
        {
            UsernameSender = usernameSender;
            UsernameReceiver = usernameReceiver;
            Content = content;
        }  
    }

    public class MESSAGE_ALL
    {
        public string UsernameSender { get; set; }
        public string Content { get; set; }
       
        public MESSAGE_ALL(string usernameSender, string content)
        {
            UsernameSender = usernameSender;
            Content = content;
        }
    }
    public class LOGIN
    {
        public string username { get; set; }
        public string password { get; set; }
        public LOGIN(string username, string password)
        {
            this.username = username;
            this.password = password;
        }   
    }

    public class LOGOUT
    {
        public string username { get; set; }
        public LOGOUT(string username)
        {
            this.username = username;
        }
    }
    public class CHECKUSER
    {
        public string username { get; set; }
        public CHECKUSER(string username)
        {
            this.username = username;
        }
        
    }

    public class USERADD
    {
        public string username { get; set; }
        public USERADD(string username)
        {
            this.username = username;
        }

    }
    public class CREATEGROUP
    {
        public string groupname { get; set; }
        public CREATEGROUP(string groupname)
        {
            this.groupname = groupname;
        }
    }

    public class ADDGROUP
    {
        public string groupName { get; set; }  
        public List<string> members { get; set; }
        public ADDGROUP(string groupName, List<string> members)
        {
            this.groupName = groupName;
            this.members = members;
        }   
    }
    public class FILE
    {
        public FILE(string usernameSender, string usernameReceiver,string fullname, string fname, string path, byte[] data)
        {
            this.usernameSender = usernameSender;
            this.usernameReceiver = usernameReceiver;
            this.fullname = fullname;
            this.fname = fname;
            this.path = path;
            this.data = data;
        }
        public string fullname { get; set; }
        public string usernameSender { get; set; }
        public string usernameReceiver { get; set; }
        public string fname { get; set; }
        public string path { get; set; }
        public byte[] data { get; set; }
    }
    public class DOWNFILE
    {
        public DOWNFILE(string usernameSender, string usernameReceiver, string fname)
        {
            this.usernameSender = usernameSender;
            this.usernameReceiver = usernameReceiver;
            this.fname = fname;
        }
        public string usernameSender { get; set; }
        public string usernameReceiver { get; set; }
        public string fname { get; set; }
    }

    public class INITDATA
    {
        public INITDATA(List<string> users, Dictionary<string, List<string>> groups,  string username)
        {
            this.users = users;
            this.groups = groups;
            this.username = username;
        }
        public INITDATA() { }
        public List<string> users { get; set; }
        public Dictionary<string, List<string>> groups { get; set; }
        public string username  { get; set; }
    }
}
