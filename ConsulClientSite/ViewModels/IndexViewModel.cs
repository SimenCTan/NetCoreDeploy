using DnsClient;

namespace ConsulClientSite.ViewModels
{
    public class IndexViewModel
    {
        public ServiceHostEntry[] DnsResult{get;set;}
        public string ServiceResult{get;set;}
    }
}