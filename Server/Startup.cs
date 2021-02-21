using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Serilog;

namespace Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<PrimesNumbers>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    Log.Information("Start point to show info ");
                    await context.Response.WriteAsync("HomeWork 9 \n");
                    await context.Response.WriteAsync("Student of PM tech Academy \n");
                    await context.Response.WriteAsync("Zinchenko Bohdan \n");
                    Log.Information("Showed information about program and developer");

                });

                endpoints.MapGet("/primes/{number:int}", async context =>
                {
                    if (!int.TryParse((string)context.Request.RouteValues["number"], out var resultNumber))
                    {
                        Log.Error("Cant parse a number, return status code 400");
                        context.Response.StatusCode = 400;
                        return;
                    }

                    if (resultNumber < 0)
                    {
                        Log.Error("Cant parse a number, return status code 400");
                        context.Response.StatusCode = 400;
                        return;
                    }
                    Log.Information("Start to detect the number for its primaries");
                    var findPrime = context.RequestServices.GetRequiredService<PrimesNumbers>();
                    var result = await findPrime.OneNumberPrimeAsync(resultNumber);
                    context.Response.StatusCode = (int)result;
                    Log.Information("Shown information about this number");
                });
                endpoints.MapGet("/primes", async context =>
                {
                    Log.Information("Start to find all primaries in range");
                    var findPrime = context.RequestServices.GetRequiredService<PrimesNumbers>();
                    var from = context.Request.Query["from"].FirstOrDefault();
                    if (!int.TryParse(from, out var fromResult))
                    {
                        Log.Error("Cant parse a (From), return status code 400");
                        context.Response.StatusCode = 400;
                        return;
                    }

                    var to = context.Request.Query["to"].FirstOrDefault();
                    if (!int.TryParse(to, out var toResult))
                    {
                        Log.Error("Cant parse a (To), return status code 400");
                        context.Response.StatusCode = 400;
                        return;
                    }

                    if (fromResult > toResult)
                    {
                        Log.Information("From higher then to, end with status code 200");
                        context.Response.StatusCode = 200;
                        return;
                    }
                    var result = await findPrime.PrimeNumbersFindAsync(fromResult, toResult);
                    if (result.Count == 0)
                    {
                        Log.Information("Not founded prime numbers, end with status code 200");
                        context.Response.StatusCode = 200;
                        return;
                    }
                    Log.Information("Founded primes numbers ");
                    await context.Response.WriteAsync(string.Join(",", result));
                });
            });
        }
    }
}
