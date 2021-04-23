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

        public enum TransactionStatus
        {
            OK,
            KO
        }

        public TransactionType Type { get; set; }
        public int Id { get; private set; }
        public double Amount { get; private set; }
        public int Transmitter { get; private set; }
        public int Recipient { get; private set; }
        public TransactionStatus Status { get; set; }
        public string InputData { get; set; }
        public string OriginalId { get; private set; }

        public Transaction(
            TransactionType type,
            TransactionStatus status,
            string originalId,
            string inputData,
            int id = 0,
            double amount = 0,
            int transmitter = 0,
            int recipient = 0)
        {
            this.Type = type;
            this.Status = status;
            this.OriginalId = originalId;
            this.InputData = inputData;
            this.Id = id;
            this.Amount = amount;
            this.Transmitter = transmitter;
            this.Recipient = recipient;
        }
    }
}
