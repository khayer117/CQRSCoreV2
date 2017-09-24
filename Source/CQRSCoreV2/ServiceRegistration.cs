using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using System.IO;
using System.Reflection;
using CQRSCoreV2.Core;
using CQRSCoreV2.PaymentGetway;
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

            //RegisterMediator(builder, assemblies);

            RegisterLogger(builder);

            builder.Register<TestReslover>();

            CompressorRegistration(builder, assemblies);

            PaymentClientRegistration(builder, assemblies);

        }

        private void PaymentClientRegistration(ContainerBuilder builder, Assembly[] assemblies)
        {
            // https://stackoverflow.com/questions/3409207/autofac-resolving-component-with-parameters-dynamically
            //http://rahulrajatsingh.com/2015/02/understanding-and-implementing-factory-pattern-in-c/
            // http://docs.autofac.org/en/latest/resolve/relationships.html#dynamic-instantiation-func-b

            builder.RegisterAssemblyTypes(assemblies)
                .AssignableTo<IPaymentClient>()
                .AsSelf()
                .AsImplementedInterfaces();
            builder.Register<Func<PaymentMethod, IPaymentClient>>(c =>
            {
                var context = c.Resolve<IComponentContext>();
                var paymentServices = context.Resolve<IEnumerable<IPaymentClient>>();

                return paymentMethod => paymentServices.FirstOrDefault(s => s.PaymentMethod == paymentMethod);
            });

            builder.Register<PaymentService>();
        }

        private void CompressorRegistration(ContainerBuilder builder, Assembly[] assemblies)
        {
            // service collection
            builder.RegisterAssemblyTypes(assemblies)
                .AssignableTo<ICompress>()
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<CompressorService>();
        }

        //private void RegisterMediator(ContainerBuilder builder, Assembly[] assemblies)
        //{
        //    builder.RegisterClosingTypes(typeof(ICommandHandler<,>), assemblies)
        //        .PreserveExistingDefaults();

        //    //builder.RegisterClosingTypes(typeof(IEventHandler<>), assemblies);

        //    builder.Register<Func<Type, object>>(c =>
        //    {
        //        var ctx = c.Resolve<IComponentContext>();
        //        return ctx.Resolve;
        //    });

        //    builder.Register<CommandBus>();
        //    builder.Register<EventBus>();

        //}
        private void RegisterLogger(ContainerBuilder builder)
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
