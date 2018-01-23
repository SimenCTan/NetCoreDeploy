using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ConsulModels;
using Microsoft.Extensions.Options;
using Consul;
using DnsClient;
using System.Net;

namespace ConsulCommon
{
    //Main job register consulclient and dnslookup to container
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceDiscovery(this IServiceCollection services,ServiceDiscoveryOption serviceDiscoveryOption)
        {
            
            //implement the consulclient and add to service container
            services.AddSingleton<IConsulClient>(p=> new ConsulClient(
                cfg=>{
                    if(!string.IsNullOrEmpty(serviceDiscoveryOption.Consul.HttpEndpoint))
                        cfg.Address=new Uri(serviceDiscoveryOption.Consul.HttpEndpoint);
                }
            ));

            //implement the dns lookup and register to service container
            services.AddSingleton<IDnsQuery>(p => {
                var client=new LookupClient(IPAddress.Parse("127.0.0.1"),8600);
                if(serviceDiscoveryOption.Consul.DnsEndpoint!=null)
                {
                    client=new LookupClient(serviceDiscoveryOption.Consul.DnsEndpoint.ToIPEndPoint());                   
                }
                client.EnableAuditTrail=false;
                client.UseCache=true;
                client.MinimumCacheTimeout=TimeSpan.FromSeconds(1);
                return client; 
            });

            return services;
        }
    }
}
