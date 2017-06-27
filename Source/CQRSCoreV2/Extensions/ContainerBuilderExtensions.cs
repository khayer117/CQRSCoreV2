using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;

namespace CQRSCoreV2
{
    public static class ContainerBuilderExtensions
    {
        public static IRegistrationBuilder<TService, ConcreteReflectionActivatorData, SingleRegistrationStyle>
            Register<TService>(this ContainerBuilder instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            return instance.RegisterType<TService>()
                .AsSelf()
                .AsImplementedInterfaces();
        }
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterClosingTypes(
            this ContainerBuilder instance,
            Type type,
            params Assembly[] assemblies)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            return instance.RegisterAssemblyTypes(assemblies)
                .AsSelf()
                .AsClosedTypesOf(type)
                .AsImplementedInterfaces();
        }

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterSubclassTypes(
            this ContainerBuilder instance,
            Type type,
            params Assembly[] assemblies)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            return instance.RegisterAssemblyTypes(assemblies)
                    .Where(t => t.IsSubclassOf(type) && !t.IsAbstract)
                    .AsSelf()
                    .As(type);
        }
    }
}
