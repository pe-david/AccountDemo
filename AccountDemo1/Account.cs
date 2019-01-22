using AccountDemo1.Messages;
using EventStore.Common.Utils;
using ReactiveDomain.Domain;
using System;

namespace AccountDemo1
{
    public class Account : AggregateBase
    {
        public Account(
            Guid accountId,
            string name,
            Guid correlationId,
            Guid sourceId) : base()
        {
            Ensure.NotEmptyGuid(accountId, "accountId");
            Ensure.NotNullOrEmpty(name, "name");
            Ensure.NotEmptyGuid(correlationId, "correlationId");
            Ensure.NotEmptyGuid(sourceId, "sourceId");

            RaiseEvent(new AccountCreated(
                accountId,
                name,
                Balance,
                correlationId,
                sourceId));
        }

        public Account()
        {
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            Register<AccountCreated>(Apply);
            Register<CreditApplied>(Apply);
            Register<DebitApplied>(Apply);
        }

        private void Apply(DebitApplied @event)
        {
            Balance -= @event.Amount;
        }

        private void Apply(CreditApplied @event)
        {
            Balance += @event.Amount;
        }

        private void Apply(AccountCreated @event)
        {
            Id = @event.AccountId;
        }

        public double Balance { get; private set; }

        public void ApplyCredit(double amount, Guid corrId, Guid sourceId)
        {
            RaiseEvent(new CreditApplied(
                Id,
                amount,
                corrId,
                sourceId: sourceId));
        }

        public void ApplyDebit(double amount, Guid corrId, Guid sourceId)
        {
            RaiseEvent(new DebitApplied(
                Id,
                amount,
                corrId,
                sourceId: sourceId));
        }
    }
}
