using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveDomain.Bus;
using ReactiveDomain.Domain;

namespace AccountDemo1
{
    public class AccountSvc : IHandleCommand<>
    {
        private Account account = new Account();
        private IRepository _repo;

        public AccountSvc(ICommandBus bus, IRepository repo)
        {
            _repo = repo;
        }

        public double GetBalance()
        {
            return account.Balance;
        }

        public void ApplyCredit(CreditTransaction trans)
        {
            if (trans.Amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(trans.Amount), "Amount cannot be less than 0.");
            }

            account.ApplyCredit(trans.Amount);
        }

        public void ApplyDebit(DebitTransaction trans)
        {
            if (trans.Amount >= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(trans.Amount), "Amount cannot be greater than or equal to 0.");
            }

            account.ApplyDebit(trans.Amount);
        }

        public void CreateAccount()
        {

        }
    }
}
