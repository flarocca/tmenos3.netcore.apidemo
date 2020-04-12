using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using TMenos3.NetCore.ApiDemo.Infrastructure.HostBuilder;

namespace TMenos3.NetCore.ApiDemo.API
{
    public class Program
    {
        public static void Main(string[] args) =>
            CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            HostBuilderUtils.CreateWebHostBuilder<Startup>(args);
    }
}
