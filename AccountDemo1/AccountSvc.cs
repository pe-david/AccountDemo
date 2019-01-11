using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDemo1
{
    public class AccountSvc
    {
        private Account account = new Account();

        public AccountSvc()
        {
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
    }
}
