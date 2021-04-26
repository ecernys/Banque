using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banque
{
    abstract class Client
    {
        public int Id { get; private set; }
        public int TransactionCount { get; set; }
        public Dictionary<int, Account> Accounts { get; private set; }
        public Client(int id, int transactionCount)
        {
            this.Id = id;
            this.TransactionCount = transactionCount;
        }

        public void AddAccount(Account account)
        {
            Accounts.Add(account.Id, account);
        }

        public void RemoveAccount(Account account)
        {
            Accounts.Remove(account.Id);
        }
    }
}
