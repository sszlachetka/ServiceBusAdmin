using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBusAdmin.CommandHandlers
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommandHandlers(this IServiceCollection services)
        {
            services.AddSingleton<ServiceBusClientFactory>();
            services.AddMediatR(typeof(ServiceCollectionExtensions));
        }
    }
}