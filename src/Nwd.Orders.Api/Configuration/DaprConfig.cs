namespace Nwd.Orders.Api.Configuration
{
    public static class DaprConfig
    {
        public static void UseDapr(this IApplicationBuilder app)
        {
            app.UseCloudEvents();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapSubscribeHandler();              
            });
        }
    }
}
