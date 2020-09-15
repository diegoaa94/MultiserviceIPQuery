using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiserviceIPQuery.Interfaces
{
    public interface IQueryRepository
    {
        Dictionary<string, Delegate> ServiceMethods { get; }
        Task<JObject> GetGeoIp(string ipAddress);
        Task<JObject> GetRDAP(string ipAddress);
        Task<JObject> GetReverseDNS(string ipAddress);
        Task<JObject> GetPing(string ipAddress);
        Task<JObject> GetVirusTotal(string ipAddress);
    }
}
