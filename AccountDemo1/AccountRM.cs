using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountDemo1.Messages;
using EventStore.Core.Bus;
using ReactiveDomain.EventStore;

namespace AccountDemo1
{
    public class AccountRM //: ReadModelBase,
                             //IHandle<CreditApplied>
    {
        public AccountRM(Func<IListener> getListener) //: base(getListener)
        {
        }

        public void Handle(CreditApplied message)
        {
            throw new NotImplementedException();
        }
    }
}
