using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banque
{
    class Account
    {
        public const int MAX_WITHDRAWAL = 1000;
        private int Id { get; set; }

        public double Balance { get; private set; }

        private int MaxWithdrawal { get; set; }

        private List<Transaction> transactions { get; set; }

        public Account(int id, double balance = 0)
        {
            this.Id = id;
            this.Balance = balance;
            this.MaxWithdrawal = MAX_WITHDRAWAL;
            this.transactions = new List<Transaction>();
        }

        /// <summary>
        /// Method to Execute deposit transaction and add Transaction to history
        /// </summary>
        /// <param name="amount"> Amount of transaction </param>
        /// <param name="transaction"> 
        /// Transaction (all Levy transactions has duplicate Transfer Transaction in transmitter account)
        /// </param>
        /// <returns>Bool if succeeded </returns>
        public bool Deposit(double amount, Transaction transaction)
        {
            if (amount > 0)
            {
                Balance += amount;
                AddTransaction(transaction);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method to Execute withdrawal transaction and add Transaction to history
        /// </summary>
        /// <param name="amount"> Amount of transaction </param>
        /// <param name="transaction">
        /// Transaction (all Transfer transactions has duplicate Levy Transaction in recipient account) 
        /// </param>
        /// <returns>Bool if succeeded</returns>
        public bool Withdraw(double amount, Transaction transaction)
        {
            if (amount > 0)
            {
                if (this.Balance - amount > 0 && NotExceedMax(amount))
                {
                    this.Balance -= amount;
                    AddTransaction(transaction);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if last 10 Transactions do not exeed Maximum withdrawal limit
        /// </summary>
        /// <param name="amount">Current transaction amount </param>
        /// <returns>Bool if do not exeeds maxWithdrawal</returns>
        private bool NotExceedMax(double amount)
        {
            if (amount > MaxWithdrawal)
                return false;
            double sum = 0;
            int trCount = transactions.Count;
            int startFrom = 0;
            if (trCount > 10)
                startFrom = trCount - 10;
            for (int i = startFrom; i < trCount; i++)
            {
                if (transactions[i].Type == Transaction.TransactionType.Transfer
                    || transactions[i].Type == Transaction.TransactionType.Withdrawal)
                {
                    sum += transactions[i].Amount;
                }
            }
            return sum + amount <= MaxWithdrawal;
        }

        private void AddTransaction(Transaction transaction)
        {
            this.transactions.Add(transaction);
        }
    }
}
