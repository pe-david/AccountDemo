using ReactiveDomain.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDemo1.Messages
{
    public class ApplyCredit : Command
    {
        public readonly double Amount;

        public ApplyCredit(
            double amount,
            Guid correlationId,
            Guid? sourceId) : base(correlationId, sourceId)
        {
            Amount = amount;
        }
    }
}
