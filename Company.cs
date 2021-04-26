using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banque
{
    class Company : Client
    {
        public static double TRANSACTION_FEE = 10;
        public double transactionFee { get; set; }
        public Company(int id, int transactionCount) : base(id, transactionCount)
        {
            transactionFee = TRANSACTION_FEE;
        }
    }
}
