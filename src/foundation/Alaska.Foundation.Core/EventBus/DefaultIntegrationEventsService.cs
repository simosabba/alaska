using Alaska.Foundation.Core.EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Core.EventBus
{
    public class DefaultIntegrationEventsService : IIntegrationEventsService
    {
        //private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus _eventBus;
        //private readonly IIntegrationEventLogService _eventLogService;

        public DefaultIntegrationEventsService(IEventBus eventBus
            //Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory
            )
        {
            //_integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            //_eventLogService = _integrationEventLogServiceFactory(_catalogContext.Database.GetDbConnection());
        }

        public void PublishThroughEventBus(IntegrationEvent evt)
        {
            //await SaveEventAndOrderingContextChangesAsync(evt);
            _eventBus.Publish(evt);
            //await _eventLogService.MarkEventAsPublishedAsync(evt);
        }

        //private async Task SaveEventAndOrderingContextChangesAsync(IntegrationEvent evt)
        //{
        //    //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
        //    //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency            
        //    await ResilientTransaction.New(_orderingContext)
        //        .ExecuteAsync(async () => {
        //            // Achieving atomicity between original ordering database operation and the IntegrationEventLog thanks to a local transaction
        //            await _orderingContext.SaveChangesAsync();
        //            await _eventLogService.SaveEventAsync(evt, _orderingContext.Database.CurrentTransaction.GetDbTransaction());
        //        });
        //}
    }
}
