namespace CQRSCoreV2
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class EventBus : IEventBus
    {
        private static readonly Type GenericIEnumerableType = typeof(IEnumerable<>);
        private static readonly Type GenericEventHandlerType = typeof(IEventHandler<>);

        private readonly Func<Type, object> resolver;

        public EventBus(Func<Type, object> resolver)
        {
            this.resolver = resolver;
        }

        public async Task Publish(object @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException("event");
            }

            var handlerType = GenericEventHandlerType.MakeGenericType(@event.GetType());
            var handlersType = GenericIEnumerableType.MakeGenericType(handlerType);

            var handlers = (IEnumerable)this.resolver(handlersType);

            foreach (dynamic handler in handlers)
            {
                await handler.Handle((dynamic)@event);
            }
        }
    }
}