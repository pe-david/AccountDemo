using AccountDemo1.Messages;
using ReactiveDomain.Bus;
using ReactiveDomain.Domain;
using ReactiveDomain.Messaging;
using System;

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

            //var test = _repo.GetById<Account>(Guid.Parse("{b3a325b7-7cb0-448d-9af1-4f3578c0eaef}"));
        }


        public CommandResponse Handle(CreateAccount command)
        {
            var account = new Account(
                                command.AccountId,
                                command.Name,
                                command.CorrelationId,
                                command.MsgId);

            var commitId = Guid.NewGuid();
            _repo.Save(account, commitId);
            return command.Succeed();
        }

        public CommandResponse Handle(ApplyCredit command)
        {
            var account = _repo.GetById<Account>(command.AccountId);
            account.ApplyCredit(command.Amount, command.CorrelationId, command.MsgId);

            _repo.Save(account, Guid.NewGuid());
            return command.Succeed();
        }
    }
}
