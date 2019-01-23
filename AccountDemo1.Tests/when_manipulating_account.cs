using AccountDemo1.Messages;
using ReactiveDomain.Bus;
using System;
using Xunit;

namespace AccountDemo1.Tests
{
    //ReSharper disable once InconsistentNaming
    public sealed class when_manipulating_account :
                        with_account_service,
                        IHandle<AccountCreated>,
                        IHandle<CreditApplied>,
                        IHandle<DebitApplied>
    {
        protected override void When()
        {

        }

        [Fact]
        public void can_create_account()
        {
            var accountId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();
            Bus.Fire(
                new CreateAccount(
                        accountId,
                        "NewAccount",
                         correlationId,
                        null),
                responseTimeout: TimeSpan.FromMilliseconds(1500));

            BusCommands.AssertNext<CreateAccount>(correlationId, out var cmd)
                       .AssertEmpty();

            RepositoryEvents.AssertNext<AccountCreated>(correlationId, out var evt)
                            .AssertEmpty();

            Assert.Equal(accountId, evt.AccountId);
        }

        [Fact]
        public void cannot_create_duplicate_account()
        {
            var accountId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();
            Bus.Fire(
                new CreateAccount(
                    accountId,
                    "NewAccount",
                    correlationId,
                    null),
                responseTimeout: TimeSpan.FromMilliseconds(1500));

            Assert.Throws<CommandException>(
                () =>
                {
                    Bus.Fire(
                        new CreateAccount(
                            accountId,
                            "DuplicateAccount",
                            correlationId,
                            null),
                        responseTimeout: TimeSpan.FromMilliseconds(1500));
                });
        }

        [Fact]
        public void can_apply_credit()
        {
            var accountId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();
            Bus.Fire(
                new CreateAccount(
                    accountId,
                    "NewAccount",
                    correlationId,
                    null),
                responseTimeout: TimeSpan.FromMilliseconds(3000));

            const double amountCredited = 123.45;
            Bus.Fire(new ApplyCredit(
                    accountId,
                    amountCredited,
                    correlationId,
                    Guid.Empty),
                responseTimeout: TimeSpan.FromSeconds(60));

            BusCommands.DequeueNext<CreateAccount>();
            RepositoryEvents.DequeueNext<AccountCreated>();

            BusCommands.AssertNext<ApplyCredit>(correlationId, out var cmd)
                .AssertEmpty();

            RepositoryEvents.AssertNext<CreditApplied>(correlationId, out var evt)
                .AssertEmpty();

            Assert.Equal(amountCredited, evt.Amount);
        }

        [Fact]
        public void cannot_apply_debit_with_wrong_id()
        {
            var accountId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();
            Bus.Fire(
                new CreateAccount(
                    accountId,
                    "NewAccount",
                    correlationId,
                    null),
                responseTimeout: TimeSpan.FromMilliseconds(3000));

            const double amountDebited = 123.45;
            Assert.Throws<CommandException>(
                () =>
                {
                    Bus.Fire(new ApplyCredit(
                            Guid.NewGuid(),
                            amountDebited,
                            correlationId,
                            Guid.Empty),
                        responseTimeout: TimeSpan.FromSeconds(60));
                });
        }

        [Fact]
        public void cannot_apply_credit_with_wrong_id()
        {
            var accountId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();
            Bus.Fire(
                new CreateAccount(
                    accountId,
                    "NewAccount",
                    correlationId,
                    null),
                responseTimeout: TimeSpan.FromMilliseconds(3000));

            const double amountCredited = 123.45;
            Assert.Throws<CommandException>(
                () =>
                {
                    Bus.Fire(new ApplyCredit(
                            Guid.NewGuid(),
                            amountCredited,
                            correlationId,
                            Guid.Empty),
                        responseTimeout: TimeSpan.FromSeconds(60));
                });
        }

        [Fact]
        public void can_apply_debit()
        {
            var accountId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();
            Bus.Fire(
                new CreateAccount(
                    accountId,
                    "NewAccount",
                    correlationId,
                    null),
                responseTimeout: TimeSpan.FromMilliseconds(3000));

            const double amountDebited = 123.45;
            Bus.Fire(new ApplyDebit(
                    accountId,
                    amountDebited,
                    correlationId,
                    Guid.Empty),
                responseTimeout: TimeSpan.FromSeconds(60));

            BusCommands.DequeueNext<CreateAccount>();
            RepositoryEvents.DequeueNext<AccountCreated>();

            BusCommands.AssertNext<ApplyDebit>(correlationId, out var cmd)
                .AssertEmpty();

            RepositoryEvents.AssertNext<DebitApplied>(correlationId, out var evt)
                .AssertEmpty();

            Assert.Equal(amountDebited, evt.Amount);
        }

        [Fact]
        public void cannot_debit_a_negative_amount()
        {
            var accountId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();
            Bus.Fire(
                new CreateAccount(
                    accountId,
                    "NewAccount",
                    correlationId,
                    null),
                responseTimeout: TimeSpan.FromMilliseconds(3000));

            const double amountDebited = -123.45;
            Assert.Throws<ArgumentOutOfRangeException>(
                () =>
                {
                    Bus.Fire(new ApplyDebit(
                            accountId,
                            amountDebited,
                            correlationId,
                            Guid.Empty),
                        responseTimeout: TimeSpan.FromSeconds(60));
                });
        }

        [Fact]
        public void cannot_credit_a_negative_amount()
        {
            var accountId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();
            Bus.Fire(
                new CreateAccount(
                    accountId,
                    "NewAccount",
                    correlationId,
                    null),
                responseTimeout: TimeSpan.FromMilliseconds(3000));

            const double amountCredited = -123.45;
            Assert.Throws<ArgumentOutOfRangeException>(
                () =>
                {
                    Bus.Fire(new ApplyCredit(
                            accountId,
                            amountCredited,
                            correlationId,
                            Guid.Empty),
                        responseTimeout: TimeSpan.FromSeconds(60));
                });
        }

        [Fact]
        public void debit_fails_when_wrong_account_id()
        {
            var accountId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();
            Bus.Fire(
                new CreateAccount(
                    accountId,
                    "NewAccount",
                    correlationId,
                    null),
                responseTimeout: TimeSpan.FromMilliseconds(3000));

            const double amountDebited = 123.45;
            Assert.Throws<CommandException>(
                () =>
                {
                    Bus.Fire(new ApplyDebit(
                            Guid.NewGuid(),
                            amountDebited,
                            correlationId,
                            Guid.Empty),
                        responseTimeout: TimeSpan.FromSeconds(60));
                });
        }

        [Fact]
        public void credit_fails_when_wrong_account_id()
        {
            var accountId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();
            Bus.Fire(
                new CreateAccount(
                    accountId,
                    "NewAccount",
                    correlationId,
                    null),
                responseTimeout: TimeSpan.FromMilliseconds(3000));

            const double amountCredited = 123.45;
            Assert.Throws<CommandException>(
                () =>
                {
                    Bus.Fire(new ApplyCredit(
                            Guid.NewGuid(),
                            amountCredited,
                            correlationId,
                            Guid.Empty),
                        responseTimeout: TimeSpan.FromSeconds(60));
                });
        }

        public void Handle(AccountCreated message)
        {
        }

        public void Handle(CreditApplied message)
        {
        }

        public void Handle(DebitApplied message)
        {
        }

        //public void Dispose()
        //{
        //    Dispose(true);
        //}

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (_disposed) return;
        //    if (disposing)
        //    {
        //    }

        //    _disposed = true;
        //}
    }
}
