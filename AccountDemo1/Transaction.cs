using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDemo1
{
    public abstract class Transaction
    {
        protected string TransactionType { get; set; }

        public double Amount { get; protected set; }

        public void WriteTransaction()
        {
            Console.WriteLine($"{TransactionType} transaction: ${Amount:0.00}");
        }
    }
}
