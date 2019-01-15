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
    public class AccountSvc : IHandleCommand<CreateAccount>
    {
        private Account _account;
        private readonly IRepository _repo;
        private readonly ICommandBus _bus;

        public AccountSvc(ICommandBus bus, IRepository repo)
        {
            _repo = repo;
            _bus = bus;

            _bus.Subscribe<CreateAccount>(this);
        }

        //public double GetBalance()
        //{
        //    return account.Balance;
        //}

        public void ApplyCredit(CreditTransaction trans)
        {
            //if (trans.Amount < 0)
            //{
            //    throw new ArgumentOutOfRangeException(nameof(trans.Amount), "Amount cannot be less than 0.");
            //}

            //account.ApplyCredit(trans.Amount);
        }

        //public void ApplyDebit(DebitTransaction trans)
        //{
        //    //if (trans.Amount >= 0)
        //    //{
        //    //    throw new ArgumentOutOfRangeException(nameof(trans.Amount), "Amount cannot be greater than or equal to 0.");
        //    //}

        //    //account.ApplyDebit(trans.Amount);
        //}

        public void GetOrCreateAccount()
        {
            try
            {
                _account = _repo.GetById<Account>(Constants.AccountId);
                return;
            }
            catch
            {
            }

            _account = new Account(
                Constants.AccountId,
                "TheAccount",
                Guid.NewGuid(),
                Guid.Empty);

            _repo.Save(_account, Constants.AccountId);
        }

        public CommandResponse Handle(CreateAccount command)
        {
            _account = new Account(
            command.AccountId,
            command.Name,
            command.CorrelationId,
            command.SourceId);

            var commitId = Guid.NewGuid();
            _repo.Save(_account, commitId);
            return command.Succeed();
        }
    }
}
