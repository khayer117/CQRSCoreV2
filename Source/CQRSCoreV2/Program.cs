using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Serilog;
using Serilog.Events;

namespace CQRSCoreV2
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = CreateContainer();

            (new TestCqrs(container)).Run().Wait();

            Console.ReadKey();
        }

        private static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

            var assemblies = AssembliesProvider.Instance.Assemblies.ToArray();
            builder.RegisterAssemblyModules(assemblies);

            var container = builder.Build();

            return container;
        }

        
    }
}
