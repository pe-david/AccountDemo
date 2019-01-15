using AccountDemo1.Messages;
using EventStore.Common.Utils;
using ReactiveDomain.Domain;
using System;

namespace AccountDemo1
{
    public class Account : AggregateBase
    {
        //public Account()
        //{
        //    Console.WriteLine("Initial balance: $0.00");
        //}

        public Account(
            Guid accountId,
            string name,
            Guid correlationId,
            Guid? sourceId) : base()
        {
            Ensure.NotEmptyGuid(accountId, "studyId");
            Ensure.NotNullOrEmpty(name, "name");
            Ensure.NotEmptyGuid(correlationId, "correlationId");

            Id = accountId;
            RaiseEvent(new AccountCreated(
                accountId,
                name,
                Guid.NewGuid(),
                correlationId));
        }

        public Account()
        {
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            Register<AccountCreated>(Apply);
            Register<CreditApplied>(Apply);
        }

        private void Apply(CreditApplied @event)
        {
            Balance += @event.Amount;
            Console.WriteLine($"Credit Applied: {@event.Amount}");

        }

        private void Apply(AccountCreated @event)
        {
            Id = @event.AccountId;
            Console.WriteLine($"Account created: {@event.Name}");
        }

        public double Balance { get; private set; }

        public void ApplyCredit(double amount, Guid corrId, Guid sourceId )
        {
            //Balance += amount;

            RaiseEvent(new CreditApplied(
                Id,
                amount,
                corrId,
                sourceId: sourceId));
        }

        public void WriteBalance()
        {
            Console.WriteLine($"Balance: ${Balance:0.00}");
        }
    }
}
