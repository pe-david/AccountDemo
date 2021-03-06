﻿using AccountDemo1.Messages;
using ReactiveDomain.Bus;
using System;
using System.Windows;
using ReactiveDomain.Domain;
using ReactiveDomain.EventStore;
using Splat;

namespace AccountDemo1
{
    public class AccountRM : ReadModelBase,
                             IHandle<AccountCreated>,
                             IHandle<CreditApplied>,
                             IHandle<DebitApplied>
    {
        private double balance;

        public AccountRM(Guid accountId)
            : base(() =>
                Locator
                    .Current
                    .GetService<IRepository>()
                    .GetListener(
                        $"ImageCache: account-{accountId:N}",
                        true))
        {
            EventStream.Subscribe<AccountCreated>(this);
            EventStream.Subscribe<CreditApplied>(this);
            EventStream.Subscribe<DebitApplied>(this);

            Start<Account>(accountId, null,  true);
        }

        public void Handle(AccountCreated message)
        {
            balance = message.Balance;
            Console.WriteLine();
            Console.WriteLine($"Account created: {message.Name}, {message.AccountId}");
            Console.WriteLine($"Initial balance: ${message.Balance:0.00}");
            Console.WriteLine("Enter cr <amount> to enter a credit.");
            Console.WriteLine("Enter db <amount> to enter a debit.");
        }

        public void Handle(CreditApplied message)
        {
            balance += message.Amount;
            Console.WriteLine();
            Console.WriteLine($"CreditApplied: ${message.Amount:0.00}");
            Console.WriteLine($"Balance: ${balance:0.00}");
        }

        public void Handle(DebitApplied message)
        {
            balance -= message.Amount;
            Console.WriteLine();
            Console.WriteLine($"DebitApplied: ${message.Amount:0.00}");
            Console.WriteLine($"Balance: ${balance:0.00}");
        }
    }
}
