using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiserviceIPQuery.Interfaces
{
    public interface IQueryService
    {
        Task<List<JObject>> QueryIPAddress(string ipAddress, List<string> services);
        bool IsValidIPAddress(string ipAddress);
    }
}
