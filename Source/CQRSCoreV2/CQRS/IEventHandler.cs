namespace CQRSCoreV2
{
    using System.Threading.Tasks;

    public interface IEventHandler<in TEvent>
    {
        Task Handle(TEvent @event);
    }
}