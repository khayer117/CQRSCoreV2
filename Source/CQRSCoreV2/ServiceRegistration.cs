using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using System.IO;
using System.Reflection;
using Serilog;
using Module = Autofac.Module;

namespace CQRSCoreV2
{
    public class ServiceRegistration:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = AssembliesProvider.Instance
               .Assemblies
               .ToArray();

            RegisterMediator(builder, assemblies);
            RegisterLogger(builder);

            builder.Register<TestReslover>();

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
        private static void RegisterLogger(ContainerBuilder builder)
        {
            const string MessageTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} " +
                                           "{MachineName}->{ProcessId}->{ThreadId} " +
                                           "[{Level}] " +
                                           "{Message}{NewLine}{Exception}";

            var logFormat = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs") +
                Path.DirectorySeparatorChar + "{Date}.txt";

            var seriLogger = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .WriteTo.RollingFile(logFormat, outputTemplate: MessageTemplate)
                .WriteTo.LiterateConsole(outputTemplate: MessageTemplate)
                //.WriteTo.Email(
                //    new EmailConnectionInfo
                //    {
                //        FromEmail = ApplicationInfo.Email,
                //        ToEmail = ConfigurationManager.AppSettings["app.errorEmail"],
                //        EmailSubject = "Attention!!! " + ApplicationInfo.FullName + " Fatal Error"
                //    },
                //    restrictedToMinimumLevel: LogEventLevel.Fatal,
                //    batchPostingLimit: 1)
                .Enrich.FromLogContext()
                //.Enrich.WithMachineName()
                //.Enrich.WithProcessId()
                //.Enrich.WithThreadId()
                .CreateLogger();


            builder.RegisterInstance(seriLogger);

            builder.Register<Logger>();
            //builder.RegisterType<Logger>().AsSelf().AsImplementedInterfaces();

            Log.Logger = seriLogger;
        }
    }
}
