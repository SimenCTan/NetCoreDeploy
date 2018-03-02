using Autofac;
using ConsulService.Services;
using Microsoft.Extensions.Logging;

namespace ConsulService
{
    public class AutofacModule:Module
    {
        //register service to middle pipeline so can called by every dependent component
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c=>new ValuesService(c.Resolve<ILogger<ValuesService>>()))
                    .As<IValuesService>()
                    .InstancePerLifetimeScope();
        }
    }
}