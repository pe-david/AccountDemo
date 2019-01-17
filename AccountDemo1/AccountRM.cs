using AccountDemo1.Messages;
using ReactiveDomain.Bus;
using System;
using System.Windows;
using ReactiveDomain.Domain;
using ReactiveDomain.EventStore;
using Splat;

namespace AccountDemo1
{
    public class AccountRM : TransientSubscriber,
                             IHandle<AccountCreated>,
                             IHandle<CreditApplied>
                             //IHandle<DebitApplied>
    {
        private Guid _accountId;
        
        private IListener _listener;
        public AccountRM(
               ISubscriber bus,
               Guid accountId) : base(bus)
        {
            _accountId = accountId;

            _listener = Locator.Current.GetService<IRepository>().GetListener($"ImageCache: account-{_accountId:N}", true);

            _listener.EventStream.Subscribe<AccountCreated>(this);
            _listener.EventStream.Subscribe<CreditApplied>(this);
            //_listener.EventStream.Subscribe<DebitApplied>(this);

            _listener.Start<Account>(_accountId, null,  true);
        }

        public void Handle(DebitApplied message)
        {
            Console.WriteLine("DebitApplied - Got here!!!!!!");
        }

        public void Handle(AccountCreated message)
        {
            Console.WriteLine("Got here!!!!!!");
        }

        public void Handle(CreditApplied message)
        {
            Console.WriteLine("CreditApplied - Got here!!!!!!");
        }
    }
}
