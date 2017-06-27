namespace CQRSCoreV2.Core
{
    using System.Threading.Tasks;

    public interface IEventBus
    {
        Task Publish(object @event);
    }
}