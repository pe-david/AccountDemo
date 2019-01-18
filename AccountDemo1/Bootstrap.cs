using System.Diagnostics;
using System.Reflection;

namespace AccountDemo1
{
    public class Bootstrap
    {
        private static string _assemblyName;

        public Bootstrap()
        {
            var fullName = Assembly.GetExecutingAssembly().FullName;
            _assemblyName = fullName.Split(',')[0];
        }

        public static void Load()
        {
            Debug.WriteLine($"{_assemblyName} has been loaded.");
        }

        public static void Configure()
        {
        }
    }
}
