using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountDemo1.Messages;
using ReactiveDomain.Bus;
using ReactiveDomain.Messaging;
using Xunit;

namespace AccountDemo1.Tests
{
    // ReSharper disable once InconsistentNaming
    public sealed class when_creating_account :
                        with_account_service,
                        IHandle<AccountCreated>
    {
        static when_creating_account()
        {
            Bootstrap.Load();
        }

        protected override void When()
        {

            var accountId = Guid.NewGuid();
            Bus.Fire(new CreateAccount(
                accountId,
                "NewAccount",
                Guid.NewGuid(),
                Guid.Empty));

            ClearQueues();
        }

        [Fact]
        public void creation_succeeds()
        {

        }

        public void Handle(AccountCreated message)
        {
            throw new NotImplementedException();
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
