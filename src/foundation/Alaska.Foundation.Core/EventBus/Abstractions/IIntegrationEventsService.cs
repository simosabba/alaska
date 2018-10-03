using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Core.EventBus.Abstractions
{
    public interface IIntegrationEventsService
    {
        void PublishThroughEventBus(IntegrationEvent evt);
    }
}
