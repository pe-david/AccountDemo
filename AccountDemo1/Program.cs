using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountDemo1;
using ReactiveDomain.EventStore;

namespace AccountDemo1
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new Application();
            app.Bootstrap();
            app.Run();
        }
    }
}
