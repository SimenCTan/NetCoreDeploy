using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ConsulClientSite.Models;
using DnsClient;
using ConsulClientSite.ViewModels;
using Microsoft.Extensions.Options;
using ConsulModels;
using System.Net.Http;

namespace ConsulClientSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDnsQuery _dns;
        private readonly ServiceDiscoveryOption _serviceDiscoveryOption;

        //Dependency injection transmit value
        public HomeController(IDnsQuery dns,IOptions<ServiceDiscoveryOption>serviceDiscoveryOption)
        {
            _dns=dns??throw new ArgumentException(nameof(dns));
            _serviceDiscoveryOption=serviceDiscoveryOption.Value;
        }
        public async Task<IActionResult> Index()
        {
            IndexViewModel model=new IndexViewModel()
            {
                DnsResult=await _dns.ResolveServiceAsync("service.consul",_serviceDiscoveryOption.ServiceName)
            };
            if(model.DnsResult.Length>0)
            {
                var firstAddress=model.DnsResult.First().AddressList.FirstOrDefault();
                if(firstAddress!=null)
                {
                    var port=model.DnsResult.First().Port;
                    using(var client=new HttpClient())
                    {
                        model.ServiceResult=await client.GetStringAsync($"http://{firstAddress}:{port}/Values");
                    }
                }    
            }
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
