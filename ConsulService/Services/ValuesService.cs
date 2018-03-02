using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace ConsulService.Services
{
    public class ValuesService : IValuesService
    {
        private readonly ILogger<ValuesService>_logger;

        public ValuesService(ILogger<ValuesService>logger)
        {
            _logger=logger;
        }
        public IEnumerable<string> FindAll()
        {
            _logger.LogDebug("{method} called",nameof(FindAll));
            return new[]{"Value1","Value2"};
        }

        public string FindId(int id)
        {
            _logger.LogDebug("{method} called with {id}", nameof(FindId), id);

            return $"value{id}";
        }
    }
}