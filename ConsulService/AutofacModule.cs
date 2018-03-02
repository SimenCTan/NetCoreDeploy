using Autofac;
using ConsulService.Services;
using Microsoft.Extensions.Logging;

namespace ConsulService
{
    public class AutofacModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c=>new ValuesService(c.Resolve<ILogger<ValuesService>>()))
                    .As<IValuesService>()
                    .InstancePerLifetimeScope();
        }
    }
} 