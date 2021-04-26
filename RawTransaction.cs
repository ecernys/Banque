using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banque
{
    class RawTransaction
    {
        public string Id { get; private set; }
        public string Date { get; private set; }
        public string Amount { get; private set; }
        public string Transmitter { get; private set; }
        public string Receiver { get; private set; }
        public Status Status { get; set; }
        public RawTransaction(string id, string date, string amount, string transmitter, string receiver)
        {
            Id = id;
            Date = date;
            Amount = amount;
            Transmitter = transmitter;
            Receiver = receiver;
            Status = Banque.Status.KO;
        }
    }
}
