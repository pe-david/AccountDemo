using ReactiveDomain.Messaging;
using System;
using System.Threading;

namespace AccountDemo1.Messages
{
    public class AccountCreated : DomainEvent
    {
        private static readonly int TypeId = Interlocked.Increment(ref NextMsgId);
        public override int MsgTypeId => TypeId;

        public readonly Guid AccountId;
        public readonly string Name;

        public AccountCreated(
            Guid accountId,
            string name,
            Guid correlationId,
            Guid sourceId) : base(correlationId, sourceId)
        {
            AccountId = accountId;
            Name = name;
        }
    }
}
