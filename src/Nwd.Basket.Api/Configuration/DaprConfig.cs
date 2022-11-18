namespace Nwd.Basket.Api.Configuration
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
            });
        }
    }
}
