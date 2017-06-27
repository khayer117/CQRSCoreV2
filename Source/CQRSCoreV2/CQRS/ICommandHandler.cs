namespace CQRSCoreV2
{
    using System.Threading.Tasks;

    public interface ICommandHandler<in TCommand, TCommandResult>
    {
        Task<TCommandResult> Handle(TCommand command);
    }
}