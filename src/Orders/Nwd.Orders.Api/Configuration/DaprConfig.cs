using Nwd.Orders.Application.Actors;

namespace Nwd.Orders.Api.Configuration
{
    public static class DaprConfig
    {
        public static void UseDapr(this IApplicationBuilder app)
        {
            app.UseCloudEvents();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapSubscribeHandler();
                endpoints.MapControllers();
                endpoints.MapActorsHandlers();
            });

        }

        public static void AddDaprActors(this IServiceCollection services)
        {
            services.AddActors(options =>
            {
                options.Actors.RegisterActor<OrderProcessorActor>();
            });
        }
    }
}