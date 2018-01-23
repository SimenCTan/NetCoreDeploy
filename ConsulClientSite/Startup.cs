using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ConsulModels;
using DnsClient;
using System.Net;
using Microsoft.Extensions.Logging;

namespace ConsulClientSite
{
    public class Startup
    {
        private readonly ServiceDiscoveryOption _serviceDiscoveryOption;
        public Startup(IConfiguration configuration)
        {
            _serviceDiscoveryOption=new ServiceDiscoveryOption();
            Configuration = configuration.GetSection("ServiceDiscovery");
            Configuration.Bind(_serviceDiscoveryOption);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //add framework services
            services.AddMvc();
            //create a instance lookupclient with specified address and port
            services.AddSingleton<IDnsQuery>(new LookupClient(IPAddress.Parse(_serviceDiscoveryOption.Consul.DnsEndpoint.Address),
                _serviceDiscoveryOption.Consul.DnsEndpoint.Port));
            services.Configure<ServiceDiscoveryOption>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory)
        {
            //create logger for app
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
