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
        private readonly Guid accountGuid = Guid.Parse("{FDAFEE94-B2C4-4F09-B6BC-7734CE862CA8}");

        private Account account;
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
            //try
            //{
            //    account = _repo.GetById<Account>(accountGuid);
            //    return;
            //}
            //catch
            //{
            //}

            account = new Account(
                accountGuid,
                "TheAccount",
                new Guid(),
                new Guid());

            _repo.Save(account, accountGuid);
        }

        public CommandResponse Handle(CreateAccount command)
        {
            Console.WriteLine("got here!!!");
            return command.Succeed();
        }
    }
}
