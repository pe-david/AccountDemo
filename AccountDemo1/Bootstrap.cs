﻿using System.Diagnostics;
using System.IO;
using System.Reflection;
using EventStore.ClientAPI;
using ReactiveDomain.Bus;
using ReactiveDomain.Domain;
using ReactiveDomain.EventStore;
using Splat;

namespace AccountDemo1
{
    public class Bootstrap
    {
        private static string _assemblyName;
        private static AccountSvc _acctSvc;
        private static EventStoreLoader _es;
        private static GetEventStoreRepository _esRepository;
        //private static IGeneralBus _bus;
        private static IEventStoreConnection _esConnection;

        public Bootstrap()
        {
            var fullName = Assembly.GetExecutingAssembly().FullName;
            _assemblyName = fullName.Split(',')[0];
        }

        public static void Load()
        {
            Debug.WriteLine($"{_assemblyName} has been loaded.");
        }

        public static void Configure(IGeneralBus bus)
        {
            _es = new EventStoreLoader();
            _es.SetupEventStore(new DirectoryInfo(@"C:\Users\rosed18169\source\EventStore-OSS-Win-v3.9.4"));
            _esConnection = _es.Connection;
            _esRepository = new GetEventStoreRepository(_es.Connection);
            Locator.CurrentMutable.RegisterConstant(_esRepository, typeof(IRepository));

            _acctSvc = new AccountSvc(bus, _esRepository);
        }
    }
}
