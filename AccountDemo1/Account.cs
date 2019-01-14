using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountDemo1.Messages;
using EventStore.Common.Utils;
using ReactiveDomain.Domain;

namespace AccountDemo1
{
    public class Account : AggregateBase
    {
        static Account()
        {
            Console.WriteLine("Initial balance: $0.00");
        }

        public Account(
            Guid accountId,
            string name,
            Guid correlationId,
            Guid sourceId) : this()
        {
            Ensure.NotEmptyGuid(accountId, "studyId");
            Ensure.NotNullOrEmpty(name, "name");
            Ensure.NotEmptyGuid(correlationId, "correlationId");
            Ensure.NotEmptyGuid(sourceId, "sourceId");

            RaiseEvent(new AccountCreated(
                accountId,
                name,
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
            Register<ApplyCredit>(Apply);
        }

        private void Apply(AccountCreated @event)
        {
            Console.WriteLine($"Account created: {@event.Name}");
        }

        private void Apply(ApplyCredit @event)
        {
            Console.WriteLine($"Credit amount: {@event.Amount}");
        }

        public double Balance { get; private set; }

        public void ApplyCredit(double amount)
        {
            Balance += amount;
            WriteBalance();
        }

        public void ApplyDebit(double amount)
        {
            var temp = Balance + amount;
            if (temp < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Balance cannot be below 0.");
            }

            Balance = temp;
            WriteBalance();
        }

        public void WriteBalance()
        {
            Console.WriteLine($"Balance: ${Balance:0.00}");
        }
    }
}
