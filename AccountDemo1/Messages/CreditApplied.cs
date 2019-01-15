using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveDomain.Messaging;

namespace AccountDemo1.Messages
{
    public class CreditApplied : DomainEvent
    {
        public readonly double Amount;

        protected CreditApplied(
            double amount,
            Guid correlationId,
            Guid sourceId) : base(correlationId, sourceId)
        {
            Amount = amount;
        }
    }
}
