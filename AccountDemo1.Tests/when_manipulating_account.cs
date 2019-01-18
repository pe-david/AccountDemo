using AccountDemo1.Messages;
using ReactiveDomain.Bus;
using System;
using Xunit;

namespace AccountDemo1.Tests
{
    //ReSharper disable once InconsistentNaming
    public sealed class when_manipulating_account :
                        with_account_service,
                        IHandle<AccountCreated>
    {
        protected override void When()
        {

        }

        [Fact]
        public void can_create_account()
        {
            var accountId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();
            Bus.Fire(new CreateAccount(
                accountId,
                "NewAccount",
                 correlationId,
                null));

            BusCommands.AssertNext<CreateAccount>(correlationId, out var cmd)
                       .AssertEmpty();

            RepositoryEvents.AssertNext<AccountCreated>(correlationId, out var evt)
                            .AssertEmpty();

            Assert.Equal(accountId, evt.AccountId);

        }

        public void Handle(AccountCreated message)
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
