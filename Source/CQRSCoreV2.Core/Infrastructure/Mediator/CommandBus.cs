using System.Collections.Generic;
using System.Diagnostics;

namespace CQRSCoreV2.Core
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;

    public class CommandBus : ICommandBus
    {
        private static readonly Type GenericCommandHandlerType = typeof(ICommandHandler<,>);

        private readonly Func<Type, object> resolver;
        private readonly ILogger logger;

        public CommandBus(Func<Type, object> resolver, ILogger logger)
        {
            this.resolver = resolver;
            this.logger = logger;
        }

        public async Task<TResult> Send<TResult>(object command, CancellationToken cancellationToken)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            using (this.logger.MeasureTime($"CommandBus sending command {command.GetType().Name}"))
            {
                cancellationToken.ThrowIfCancellationRequested();

                var handlerType = GenericCommandHandlerType.MakeGenericType(command.GetType(), typeof(TResult));

                dynamic handler = this.resolver(handlerType);

                if (handler == null)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Unable to find any handler for command \"{0}\"!", command.GetType()));
                }
                var handlerName = ((Type)handler.GetType()).ReadableName();

                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    dynamic result;
                    using (this.logger.MeasureTime($"CommandBus processed command {command.GetType().Name}"))
                    {
                        result = await handler.Handle((dynamic)command);
                    }

                    LogHandlerResult(command, result, handlerName);

                    var disposable = handler as IDisposable;
                    disposable?.Dispose();

                    return result;
                }
                catch (Exception e)
                {
                    this.logger.Error(e, "CommandBus- Handler {Handler} generates error.", handlerName);
                    throw;
                }
            }
        }

        private void LogHandlerResult(object command, dynamic result, string handlerName)
        {
#if DEBUG
            using (this.logger.MeasureTime($"CommandBus Logged command {command.GetType().Name}", 100))
            {
                var watch = Stopwatch.StartNew();
                {
                    this.logger.Info("CommandBus: {@Command} handled by {Handler} and returns {@Result}", command, handlerName, result);
                }
                watch.Stop();

                (result as IDevItemLoggable)?.DevItems.Add("CommandBus: Log HandlerResult", $"in {watch.Elapsed} ({watch.ElapsedMilliseconds} ms)");
            }
#endif
#if !DEBUG
            this.logger.Debug("CommandBus: {@Command} handled by {Handler} and returns {@Result}", command, handlerName, result);
#endif
        }
    }

    public interface IDevItemLoggable
    {
        /// <summary>
        /// Do not use this property to implement any business logic.
        /// </summary>
        IDictionary<string, object> DevItems { get; set; }
    }
}