using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banque
{
    class RawAccount
    {

        public string Id { get; private set; }
        public string Date { get; private set; }
        public string Balance { get; private set; }
        public string Entry { get; private set; }
        public string Exit { get; private set; }
        public Status Status { get; set; }

        public RawAccount(string id, string date, string balance, string entry, string exit)
        {
            Id = id;
            Date = date;
            Balance = balance;
            Entry = entry;
            Exit = exit;
            Status = Banque.Status.KO;
        }

    }
}
