using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NhatMinh_WPF_BT
{
    public class Account
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public Account(string name, string username, string password, string role)
        {
            Name = name;
            Username = username;
            Password = password;
            Role = role;
        }
    }
}
