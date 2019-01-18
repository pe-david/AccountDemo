using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using ReactiveDomain.Bus;
using ReactiveDomain.Domain;
using ReactiveDomain.EventStore;
using ReactiveDomain.Tests.Specifications;
using Splat;

namespace AccountDemo1.Tests
{
    // ReSharper disable once InconsistentNaming
    public abstract class with_account_service : MockRepositorySpecification, IDisposable
    {
        private bool _disposed;
        protected AccountSvc acctSvc;


        protected override void Given()
        {

        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
            }

            _disposed = true;
        }
    }
}
