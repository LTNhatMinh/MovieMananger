using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NhatMinh_WPF_BT
{
    public class AccountService
    {
        private static UnitOfWork _unitOfWork = UnitOfWork.Instance;

        private ObservableCollection<Account> accounts;

        public AccountService()
        {
            accounts = new ObservableCollection<Account>(_unitOfWork.AccountRepository.Gets());
        }

        public Account isExistAccountByUsernamePassword(string username, string password)
        {
            foreach (Account account in accounts)
            {
                if (account.Username.Equals(username) &&
                account.Password.Equals(password))
                    return account;
            }
            return null;
        }
    }
}
