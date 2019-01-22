﻿using AccountDemo1.Messages;
using ReactiveDomain.Bus;
using System;
using Xunit;

namespace AccountDemo1.Tests
{
    //ReSharper disable once InconsistentNaming
    public sealed class when_manipulating_account :
                        with_account_service,
                        IHandle<AccountCreated>,
                        IHandle<CreditApplied>
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

        public void Handle(AccountCreated message)
        {
        }

        public void Handle(CreditApplied message)
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