using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ImageManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://localhost:5000/")
                .UseKestrel(options => options.Limits.MaxRequestBodySize = long.MaxValue)
                .Build();
        }
    }
}