using MultiserviceIPQuery.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MultiserviceIPQuery.Common;
using Newtonsoft.Json;

namespace MultiserviceIPQueryTest.Fakes
{
    class QueryRepositoryFake : IQueryRepository
    {
        public Dictionary<string, Delegate> _serviceMethods;

        public QueryRepositoryFake()
        {
            _serviceMethods = new Dictionary<string, Delegate>
            {
                [Constants.GeoIPService] = new Func<string, Task<JObject>>(GetGeoIp),
                [Constants.RDAPService] = new Func<string, Task<JObject>>(GetRDAP),
                [Constants.ReverseDNSService] = new Func<string, Task<JObject>>(GetReverseDNS),
                [Constants.PingService] = new Func<string, Task<JObject>>(GetPing),
                [Constants.VirusTotalService] = new Func<string, Task<JObject>>(GetVirusTotal)
            };
        }

        public Dictionary<string, Delegate> ServiceMethods
        {
            get => _serviceMethods;
        }

        public async Task<JObject> GetGeoIp(string ipAddress)
        {
            var response = new
            {
                status = "success",
                country = "United States",
                countryCode = "US",
                region = "WA",
                regionName = "Washington",
                city = "Seattle",
                zip = "98109",
                lat = 47.6222,
                lon = -122.337,
                timezone = "America/Los_Angeles",
                isp = "Amazon Technologies Inc.",
                org = "Amazon Technologies Inc",
                As = "AS11664 Techtel LMDS Comunicaciones Interactivas S.A.",
                query = "3.3.3.3"
            };

            return GetResponseObject(Constants.GeoIPService, JsonConvert.SerializeObject(response));
        }

        public Task<JObject> GetRDAP(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public async Task<JObject> GetReverseDNS(string ipAddress)
        {
            var response = new
            {
                Original = "dns.google.",
                Value = "dns.google."
            };

            return GetResponseObject(Constants.ReverseDNSService, JsonConvert.SerializeObject(response));
        }

        public async Task<JObject> GetPing(string ipAddress)
        {
            var response = new
            {
                Address = "8.8.4.4",
                AddressFamily = "InterNetwork",
                Status = "Success",
                RoundtripTime = 53
            };

            return GetResponseObject(Constants.PingService, JsonConvert.SerializeObject(response));
        }

        public Task<JObject> GetVirusTotal(string ipAddress)
        {
            throw new NotImplementedException();
        }

        private JObject GetResponseObject(string serviceName, string json)
        {
            JObject responseObject = new JObject
            {
                { serviceName, (json != null ? JObject.Parse(json) : new JObject()) }
            };
            return responseObject;
        }
    }
}
