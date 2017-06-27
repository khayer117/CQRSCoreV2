using System;
using System.Linq;
using Autofac;
using CQRSCoreV2.Core;

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
