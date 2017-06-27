using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using System.IO;
using System.Reflection;
using Module = Autofac.Module;

namespace CQRSCoreV2.Core
{
    public class ServiceRegistration:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = AssembliesProvider.Instance
               .Assemblies
               .ToArray();

            RegisterMediator(builder, assemblies);

        }
        private static void RegisterMediator(ContainerBuilder builder, Assembly[] assemblies)
        {
            builder.RegisterClosingTypes(typeof(ICommandHandler<,>), assemblies)
                .PreserveExistingDefaults();

            //builder.RegisterClosingTypes(typeof(IEventHandler<>), assemblies);

            builder.Register<Func<Type, object>>(c =>
            {
                var ctx = c.Resolve<IComponentContext>();
                return ctx.Resolve;
            });

            builder.Register<CommandBus>();
            builder.Register<EventBus>();

        }
 }
}
