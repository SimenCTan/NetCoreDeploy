namespace ConsulModels
{
    
    public class ServiceDiscoveryOption
    {
        public string ServiceName{get;set;}
        public ConsulOption Consul{get;set;}
        public string HealthCheckTemplate{get;set;}
        public string[] Endpoints{get;set;}
    }
}