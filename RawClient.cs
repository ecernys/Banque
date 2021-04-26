using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banque
{
    class RawClient
    {
        public string Id { get; private set; }
        public string Type { get; private set; }
        public string TransactionCount { get; set; }
        public Status Status { get; set; }
        public RawClient(string id, string type, string transactionCount)
        {
            Id = id;
            Type = type;
            TransactionCount = transactionCount;
            Status = Banque.Status.KO;
        }
    }
}
