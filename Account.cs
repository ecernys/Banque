using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banque
{
    class Account
    {
        public const int WITHDRAWAL_LIMIT = 2000;
        public readonly TimeSpan TRANSACTION_LIMIT_PERIOD = new TimeSpan(7, 0, 0, 0);
        public int Id { get; private set; }
        public double Balance { get; private set; }
        private int _withdrawalLimit { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ClosureDate { get; set; }
        public List<Transaction> Transactions { get; private set; }
        public int ClientId { get; set; }


        public Account(int id, DateTime creationDate, int clientId, double balance = 0)
        {
            this.Id = id;
            this.CreationDate = creationDate;
            this.ClientId = clientId;
            this.Balance = balance;
            this.ClosureDate = null;
            this._withdrawalLimit = WITHDRAWAL_LIMIT;
            this.Transactions = new List<Transaction>();
        }

        /// <summary>
        /// Method to Execute deposit transaction and add Transaction to history
        /// </summary>
        /// <param name="transaction"> 
        /// Transaction (all Levy transactions has duplicate Transfer Transaction in transmitter account)
        /// </param>
        /// <returns>Bool if succeeded </returns>
        public bool Deposit(Transaction transaction)
        {
            if (transaction.Date >= CreationDate
                && (ClosureDate == null || transaction.Date < ClosureDate)
                && transaction.Amount > 0)
            {
                Balance += transaction.Amount;
                AddTransaction(transaction);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method to Check if deposit transaction is valid
        /// </summary>
        /// <param name="transaction"> 
        /// Transaction
        /// </param>
        /// <returns>Bool if transaction is valid </returns>
        public bool CheckDeposit(Transaction transaction)
        {
            if (transaction.Date >= CreationDate
                && (ClosureDate == null || transaction.Date < ClosureDate)
                && transaction.Amount > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method to Execute withdrawal transaction and add Transaction to history
        /// </summary>
        /// <param name="transaction">
        /// Transaction (all Transfer transactions has duplicate Levy Transaction in recipient account) 
        /// </param>
        /// <returns>Bool if succeeded</returns>
        public bool Withdraw(Transaction transaction)
        {
            if (transaction.Date >= CreationDate
                && (ClosureDate == null || transaction.Date < ClosureDate)
                && transaction.Amount > 0)
            {
                if (this.Balance - transaction.Amount > 0
                    && NotExceedMax(transaction.Amount, transaction.Date))
                {
                    this.Balance -= transaction.Amount;
                    AddTransaction(transaction);
                    return true;
                }
            }
            return false;
        }        
        
        /// <summary>
        /// Method to Check withdrawal transaction validity
        /// </summary>
        /// <param name="transaction">
        /// Transaction
        /// </param>
        /// <returns>Bool if transaction is valid</returns>
        public bool CheckWithdrawal(Transaction transaction)
        {
            if (transaction.Date >= CreationDate
                && (ClosureDate == null || transaction.Date < ClosureDate)
                && transaction.Amount > 0)
            {
                if (this.Balance - transaction.Amount > 0
                    && NotExceedMax(transaction.Amount, transaction.Date))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if withdrawal limit is not exceeded during withdrawal limit period
        /// </summary>
        /// <param name="amount">Current transaction amount </param>
        /// <param name="date"> Current transaction date </param>
        /// <returns>Bool if do not exceeds withdrawal limit</returns>
        private bool NotExceedMax(double amount, DateTime date)
        {
            if (amount > _withdrawalLimit)
                return false;
            double sum = 0;
            foreach (var transaction in Transactions)
            {
                if ((transaction.Type == Transaction.TransactionType.Transfer
                    || transaction.Type == Transaction.TransactionType.Withdrawal)
                    && transaction.Date >= (date - TRANSACTION_LIMIT_PERIOD)
                    && transaction.Date <= date)
                {
                    sum += transaction.Amount;
                }
            }
            return sum + amount <= _withdrawalLimit;
        }

        private void AddTransaction(Transaction transaction)
        {
            this.Transactions.Add(transaction);
        }
    }
}
