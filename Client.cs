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
        public int TransactonLimit { get; set; }
        public double fees { get; private set; }
        public Dictionary<int, Account> Accounts { get; private set; }
        public Client(int id, int transactionCount)
        {
            this.Id = id;
            this.TransactonLimit = transactionCount;
            this.fees = 0;
            Accounts = new Dictionary<int, Account>();
        }

        public void AddAccount(Account account)
        {
            Accounts.Add(account.Id, account);
        }

        public void RemoveAccount(Account account)
        {
            Accounts.Remove(account.Id);
        }

        public void addFee(double amount)
        {
            this.fees += amount;
        }
    }
}
