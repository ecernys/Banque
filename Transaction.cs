using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banque
{
    class Transaction
    {
        public enum TransactionType
        {
            Deposition,
            Withdrawal,
            Transfer,
            Levy
        }

        public TransactionType Type { get; set; }
        public int Id { get; private set; }
        public DateTime Date { get; set; }
        public double Amount { get; private set; }
        public int Transmitter { get; private set; }
        public int Receiver { get; private set; }

        public Transaction(
            TransactionType type,
            int id,
            DateTime date,
            double amount,
            int transmitter,
            int recipient)
        {
            this.Type = type;
            this.Id = id;
            this.Date = date;
            this.Amount = amount;
            this.Transmitter = transmitter;
            this.Receiver = recipient;
        }
    }
}
