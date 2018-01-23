using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using ConsulCommon;
using ConsulModels;

namespace ConsulService
{
    public class Startup
    {
        private readonly ServiceDiscoveryOption _serviceDiscoveryOption; 
        public Startup(IConfiguration configuration)
        {
            _serviceDiscoveryOption=new ServiceDiscoveryOption();
            Configuration = configuration;
            Configuration.GetSection("ServiceDiscovery").Bind(_serviceDiscoveryOption);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSwaggerGen(opt=>{
                opt.SwaggerDoc("doc",new Info(){Title="ConsulService"});
            });          
            //config the paramter of appsettings.json file
            services.AddServiceDiscovery(_serviceDiscoveryOption);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,IHostingEnvironment env,ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }            
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseSwagger();
            app.UseSwaggerUI(c=>{
                c.SwaggerEndpoint("/swagger/doc/swagger.json","ConsulService");
            });
            app.UseMvc();
            app.UseConsulRegisterService(_serviceDiscoveryOption);
        }
    }
}
