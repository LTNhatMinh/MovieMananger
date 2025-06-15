using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;


namespace NhatMinh_WPF_BT
{
    public class AccountRepository : IRepository<Account>
    {
        public ObservableCollection<Account> Accounts { get; set; }

        public AccountRepository()
        {
            Load();
        }

        public void Load()
        {
            Accounts = new ObservableCollection<Account>();

            DataProvider.pathData = FilePath.Account;
            DataProvider.Open();

            XmlNodeList nodes = DataProvider.getDsNode("//Account");

            foreach (XmlNode node in nodes)
            {
                Account item = new Account(
                    node.Attributes["Name"].Value,
                    node.Attributes["Username"].Value,
                    node.Attributes["Password"].Value,
                    node.Attributes["Role"].Value
                );

                Accounts.Add(item);
            }

            DataProvider.Close();
        }

        public void Add(Account account)
        {
            Accounts.Add(account);

            DataProvider.pathData = FilePath.Account;
            DataProvider.Open();
            XmlNode newNode = DataProvider.createNode("Account");
            newNode.Attributes.Append(DataProvider.createAttr("Name")).Value = account.Name;
            newNode.Attributes.Append(DataProvider.createAttr("Username")).Value = account.Username;
            newNode.Attributes.Append(DataProvider.createAttr("Password")).Value = account.Password;
            newNode.Attributes.Append(DataProvider.createAttr("Role")).Value = account.Role;

            XmlNode parent = DataProvider.getNode("//AccountLogin");
            DataProvider.AppendNode(parent, newNode);
            DataProvider.Close();
        }

        public void Delete(Account account)
        {
            var target = Get(account.Username);
            if (target != null)
            {
                Accounts.Remove(target);

                DataProvider.pathData = FilePath.Account;
                DataProvider.Open();
                string xPath = $"//Account[@Username='{account.Username}']";
                XmlNode node = DataProvider.getNode(xPath);
                if (node?.ParentNode != null)
                    node.ParentNode.RemoveChild(node);
                DataProvider.Close();
            }
        }

        public void Update(Account account)
        {
            var target = Get(account.Username);
            if (target != null)
            {
                target.Name = account.Name;
                target.Password = account.Password;
                target.Role = account.Role;

                DataProvider.pathData = FilePath.Account;
                DataProvider.Open();
                string xPath = $"//Account[@Username='{account.Username}']";
                XmlNode node = DataProvider.getNode(xPath);

                if (node != null)
                {
                    node.Attributes["Name"].Value = account.Name;
                    node.Attributes["Password"].Value = account.Password;
                    node.Attributes["Role"].Value = account.Role;
                }

                DataProvider.Close();
            }
        }

        public Account Get(string username)
        {
            return Accounts.FirstOrDefault(x => x.Username == username);
        }

        public List<Account> Gets()
        {
            return Accounts.ToList();
        }
    }
}
