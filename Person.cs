using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banque
{
    class Person : Client
    {
        public static double TRANSACTION_FEE = 0.01;
        public double transactionFee { get; set; }
        public Person(int id, int transactionCount) : base(id, transactionCount)
        {
            transactionFee = TRANSACTION_FEE;
        }
    }
}
