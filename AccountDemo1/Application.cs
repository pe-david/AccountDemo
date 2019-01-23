using AccountDemo1.Messages;
using EventStore.ClientAPI;
using ReactiveDomain.Bus;
using ReactiveDomain.EventStore;
using System;
using System.IO;
using ReactiveDomain.Domain;
using Splat;

namespace AccountDemo1
{
    public class Application
    {
        //private EventStoreLoader _es;
        //private GetEventStoreRepository _esRepository;
        private IGeneralBus _bus;
        private AccountRM _accountReadModel;
        //public IEventStoreConnection EsConnection { get; private set; }

        //public void Bootstrap()
        //{
        //    _es = new EventStoreLoader();
        //    _es.SetupEventStore(new DirectoryInfo(@"C:\Users\rosed18169\source\EventStore-OSS-Win-v3.9.4"));
        //    EsConnection = _es.Connection;
        //    _esRepository = new GetEventStoreRepository(_es.Connection);
        //    Locator.CurrentMutable.RegisterConstant(_esRepository, typeof(IRepository));
        //    _bus = new CommandBus("testBus", false);

        //}

        public void Run()
        {
            Console.WriteLine("Hit return on an empty line to cancel...");
            Console.WriteLine("Enter a value. Negative values are debits, positive are credits.");
            var accountId = Guid.NewGuid();
            _bus = new CommandBus("testBus", false);
            Bootstrap.Configure(_bus);
            //var svc = new AccountSvc(_bus, _esRepository);
            _bus.Fire(new CreateAccount(
                                accountId,
                                "TheAccount",
                                Guid.NewGuid(),
                                Guid.Empty));

            _accountReadModel = new AccountRM(accountId);

            while (true)
            {
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                try
                {
                    var input = new UserInput(line);
                    if (input.Type == UserInput.InputType.Debit)
                    {
                        _bus.Fire(new ApplyDebit(
                            accountId,
                            input.Amount,
                            Guid.NewGuid(),
                            Guid.Empty));
                    }
                    else
                    {
                        _bus.Fire(new ApplyCredit(
                                accountId,
                                input.Amount,
                                Guid.NewGuid(),
                                Guid.Empty),
                            responseTimeout: TimeSpan.FromSeconds(60));
                    }
                }
                catch (CommandException e)
                {
                    var msg = e.InnerException == null ? e.Message : e.InnerException.Message;
                    Console.WriteLine(msg);
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}