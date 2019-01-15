using System;

namespace AccountDemo1
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new Application();
            app.Bootstrap();
            app.Run();

            Console.ReadKey();
        }
    }
}
