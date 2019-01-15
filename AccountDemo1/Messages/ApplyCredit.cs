﻿using ReactiveDomain.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AccountDemo1.Messages
{
    public class ApplyCredit : Command
    {
        private static readonly int TypeId = Interlocked.Increment(ref NextMsgId);
        public override int MsgTypeId => TypeId; public readonly double Amount;

        public ApplyCredit(
            double amount,
            Guid correlationId,
            Guid? sourceId) : base(correlationId, sourceId)
        {
            Amount = amount;
        }
    }
}