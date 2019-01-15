using ReactiveDomain.Messaging;
using System;
using System.Threading;

namespace AccountDemo1.Messages
{
    public class ApplyDebit : Command
    {
        private static readonly int TypeId = Interlocked.Increment(ref NextMsgId);
        public override int MsgTypeId => TypeId;

        public readonly Guid AccountId;
        public readonly double Amount;

        public ApplyDebit(
            Guid accountId,
            double amount,
            Guid correlationId,
            Guid? sourceId) : base(correlationId, sourceId)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }
}
