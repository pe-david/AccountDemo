using AccountDemo1.Messages;
using EventStore.ClientAPI;
using ReactiveDomain.Bus;
using ReactiveDomain.EventStore;
using System;
using System.IO;

namespace AccountDemo1
{
    public class Application
    {
        private EventStoreLoader _es;
        private GetEventStoreRepository _esRepository;
        private ICommandBus _bus;
        public IEventStoreConnection EsConnection { get; private set; }

        public void Bootstrap()
        {
            _es = new EventStoreLoader();
            _es.SetupEventStore(new DirectoryInfo(@"C:\Users\rosed18169\source\EventStore-OSS-Win-v3.9.4"));
            EsConnection = _es.Connection;
            _esRepository = new GetEventStoreRepository(_es.Connection);
            _bus = new CommandBus("testBus", false);
        }

        public void Run()
        {
            Console.WriteLine("Hit return on an empty line to cancel...");
            Console.WriteLine("Enter a value. Negative values are debits, positive are credits.");
            var accountId = Guid.NewGuid();
            var svc = new AccountSvc(_bus, _esRepository);
            _bus.Fire(new CreateAccount(
                                accountId,
                                "TheAccount",
                                Guid.NewGuid(),
                                Guid.Empty));

            while (true)
            {
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                if (double.TryParse(line, out var val))
                {
                    try
                    {
                        if (val < 0)
                        {
                            val *= -1;
                            _bus.Fire(new ApplyDebit(
                                accountId,
                                val,
                                Guid.NewGuid(),
                                Guid.Empty));
                        }
                        else
                        {
                            _bus.Fire(new ApplyCredit(
                                accountId,
                                val,
                                Guid.NewGuid(),
                                Guid.Empty));
                        }
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine(e.Message.Split('\r', '\n')[0]);
                    }
                }
                else
                {
                    Console.WriteLine("Unable to process transaction.");
                }
            }
        }
    }


    //IEventStoreConnection esConnection = EventStoreConnection.Create("ConnectTo=tcp://admin:changeit@localhost:1113");
    //conn = new EventStoreConnectionWrapper(esConnection);
    //esConnection.Connected += (_, __) => Console.WriteLine("Connected");
    //esConnection.ConnectAsync().Wait();
    //IStreamNameBuilder namer = new PrefixedCamelCaseStreamNameBuilder();
    //IEventSerializer ser = new JsonMessageSerializer();
    //repo = new StreamStoreRepository(namer, conn, ser);
    //Account acct = null;
    //try
    //{
    //    repo.Save(new Account(_accountId));
    //}
    //catch (Exception e)
    //{
    //}
    //IListener listener = new StreamListener("Account", conn, namer, ser);
}