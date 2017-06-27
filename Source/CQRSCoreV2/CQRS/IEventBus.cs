namespace CQRSCoreV2
{
    using System.Threading.Tasks;

    public interface IEventBus
    {
        Task Publish(object @event);
    }
}