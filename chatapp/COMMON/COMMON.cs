using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace COMMON
{
    public class COMMON
    {
        public COMMON(string kind, string content)
        {
            this.kind = kind;
            this.content = content;
        }

        public COMMON()
        {

        }
        public string kind { get; set; }
        public string content { get; set; }
        public string LOGIN = "LOGIN";
        public string REPLY = "REPLY";
        public string LOGOUT = "LOGOUT";
        public string REGISTER = "REGISTER";
        /*kind:
         * LOGIN: username, pass
         * MESSAGE_ALL: sender, content
         * MESSAGE: sender, receiver, content
         * REPLY: content(OK,CANCEL)
         * LOGOUT: username
         * CREATEGROUP
         * ADDGROUP
         */
    }
}
