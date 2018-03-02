using System.Collections.Generic;

namespace ConsulService.Services
{
    public interface IValuesService
    {
        IEnumerable<string>FindAll();
        string FindId(int id);
    }
}