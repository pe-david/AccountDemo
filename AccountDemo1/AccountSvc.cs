using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountDemo1.Messages;
using ReactiveDomain.Bus;
using ReactiveDomain.Domain;
using ReactiveDomain.Messaging;

namespace AccountDemo1
{
    public class AccountSvc : 
        IHandleCommand<CreateAccount>,
        IHandleCommand<ApplyCredit>
    {
        private readonly IRepository _repo;
        private readonly ICommandBus _bus;

        public AccountSvc(ICommandBus bus, IRepository repo)
        {
            _repo = repo;
            _bus = bus;

            _bus.Subscribe<CreateAccount>(this);
            _bus.Subscribe<ApplyCredit>(this);
            TryCreateAccount();
        }

        public void TryCreateAccount()
        {
            try
            {
                // if this succeeds, no need to create the account - it exists
                _repo.GetById<Account>(Constants.AccountId);
                return;
            }
            catch
            {
            }

            _bus.Fire(new CreateAccount(
                Constants.AccountId,
                "TheAccount",
                Guid.NewGuid(),
                Guid.Empty));
        }

        public CommandResponse Handle(CreateAccount command)
        {
            var account = new Account(
            command.AccountId,
            command.Name,
            command.CorrelationId,
            command.SourceId);

            var commitId = Guid.NewGuid();
            _repo.Save(account, commitId);
            return command.Succeed();
        }

        public CommandResponse Handle(ApplyCredit command)
        {
            var account = _repo.GetById<Account>(command.AccountId);
            account.ApplyCredit(command.Amount);

            _repo.Save(account, Guid.NewGuid());
            return command.Succeed();
        }
    }
}
