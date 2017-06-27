namespace CQRSCoreV2.Core
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public static class CommandBusExtensions
    {
        public static Task<TResult> Send<TResult>(
            this ICommandBus instance,
            object command)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            return instance.Send<TResult>(command, CancellationToken.None);
        }

        public static Task Send(
            this ICommandBus instance,
            object command)
        {
            return Send<NoCommandResult>(instance, command);
        }
    }
}