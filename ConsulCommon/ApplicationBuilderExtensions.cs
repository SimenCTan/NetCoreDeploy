using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using System;
using ConsulModels;
using Consul;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace ConsulCommon
{
    public static class ApplicationBuilderExtensions
    {
        //the method check the service discovery parameter register ipaddress to generate service agent
        //those service agents will deregister when the app stop 
        public static IApplicationBuilder UseConsulRegisterService(this IApplicationBuilder app,ServiceDiscoveryOption serviceDiscoveryOption)
        {
            var applife=app.ApplicationServices.GetRequiredService<IApplicationLifetime>()??
                throw new ArgumentException("Missing Dependency",nameof(IApplicationLifetime));
            if(serviceDiscoveryOption.Consul==null)
                throw new ArgumentException("Missing Dependency",nameof(serviceDiscoveryOption.Consul));
            var consul=app.ApplicationServices.GetRequiredService<IConsulClient>()??throw new ArgumentException("Missing dependency",nameof(IConsulClient));

            //create logger to record the important information
            var loggerFactory=app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var logger=loggerFactory.CreateLogger("ConsulCommonServiceBuilder");
            
            if(string.IsNullOrEmpty(serviceDiscoveryOption.ServiceName))
                throw new ArgumentException("service name must be configure",nameof(serviceDiscoveryOption.ServiceName));
            IEnumerable<Uri>addresses=null;
            if(serviceDiscoveryOption.Endpoints!=null&&serviceDiscoveryOption.Endpoints.Length>0)
            {
                logger.LogInformation($"Using {serviceDiscoveryOption.Endpoints.Length} configured endpoints for service registration");
                addresses=serviceDiscoveryOption.Endpoints.Select(p=>new Uri(p));
            }
            else
            {
                logger.LogInformation($"Trying to use server.Features to figure out the service endpoint for registration.");
                var features=app.Properties["server.Features"]as  FeatureCollection;
                addresses=features.Get<IServerAddressesFeature>().Addresses.Select(p=>new Uri(p)).ToArray();
            }
            logger.LogInformation($"Found{addresses.Count()} endpoints:{string.Join(",",addresses.Select(p=>p.OriginalString))}.");
            foreach(var address in addresses)
            {
                var serviceID=$"{serviceDiscoveryOption .ServiceName}_{address.Host}:{address.Port}";
                logger.LogInformation($"Registering service {serviceID} for address {address}.");
                var serviceChecks=new List<AgentServiceCheck>();
                if(!string.IsNullOrEmpty(serviceDiscoveryOption.HealthCheckTemplate))
                {
                    var healthCheckUri=new  Uri(address,serviceDiscoveryOption.HealthCheckTemplate).OriginalString;
                    serviceChecks.Add(new AgentServiceCheck(){
                        Status=HealthStatus.Passing,
                        DeregisterCriticalServiceAfter=TimeSpan.FromMinutes(1),
                        Interval=TimeSpan.FromSeconds(5),
                        HTTP=healthCheckUri
                    });
                    logger.LogInformation($"Adding healthcheck for {serviceID},checking {healthCheckUri}");
                }

                //generate the agent service
                var registration=new AgentServiceRegistration(){
                    Checks=serviceChecks.ToArray(),
                    Address=address.Host,
                    ID=serviceID,
                    Name=serviceDiscoveryOption.ServiceName,
                    Port=address.Port
                };           
                consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();
                applife.ApplicationStopping.Register(()=>{
                    consul.Agent.ServiceDeregister(serviceID).GetAwaiter().GetResult();  
                });
            }
            return app;
        }
    }
}