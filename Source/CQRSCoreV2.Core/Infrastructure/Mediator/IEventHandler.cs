namespace CQRSCoreV2.Core
{
    using System.Threading.Tasks;

    public interface IEventHandler<in TEvent>
    {
        Task Handle(TEvent @event);
    }
}