using AccountDemo1.Messages;
using ReactiveDomain.Bus;
using System;
using ReactiveDomain.EventStore;

namespace AccountDemo1
{
    public class AccountRM : ReadModelBase,
                             IHandle<AccountCreated>,
                             IHandle<CreditApplied>,
                             IHandle<DebitApplied>
    {
        public AccountRM(Func<IListener> getListener) : base(getListener)
        {
        }

        public void Handle(DebitApplied message)
        {
            throw new NotImplementedException();
        }

        public void Handle(AccountCreated message)
        {
            throw new NotImplementedException();
        }

        public void Handle(CreditApplied message)
        {
            throw new NotImplementedException();
        }
    }
}
