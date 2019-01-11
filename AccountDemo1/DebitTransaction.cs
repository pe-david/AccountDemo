using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDemo1
{
    public class DebitTransaction : Transaction
    {
        public DebitTransaction(double amount)
        {
            Amount = amount;
            TransactionType = "Debit";
        }
    }
}
