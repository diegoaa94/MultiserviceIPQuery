using DnsClient;
using Microsoft.Extensions.Configuration;
using MultiserviceIPQuery.Common;
using MultiserviceIPQuery.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace MultiserviceIPQuery.Repositories
{
    public class QueryRepository : IQueryRepository
    {
        private readonly IConfiguration _configuration;
        public Dictionary<string, Delegate> _serviceMethods;

        public QueryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
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
            string baseURL = _configuration.GetValue<string>(Constants.GeoIPBaseURL);
            string acceptType = _configuration.GetValue<string>(Constants.GeoIPAccept);
            RestClient client = new RestClient($"{baseURL}{ipAddress}");
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("Accept", acceptType);
            IRestResponse response = await client.ExecuteAsync(request);

            return GetResponseObject(Constants.GeoIPService, response.Content);
        }

        public async Task<JObject> GetRDAP(string ipAddress)
        {
            string baseURL = _configuration.GetValue<string>(Constants.RDAPBaseURL);
            string acceptType = _configuration.GetValue<string>(Constants.RDAPAccept);
            RestClient client = new RestClient($"{baseURL}{ipAddress}");
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("Accept", acceptType);
            IRestResponse response = await client.ExecuteAsync(request);

            return GetResponseObject(Constants.RDAPService, response.Content);
        }

        public async Task<JObject> GetReverseDNS(string ipAddress)
        {
            LookupClient client = new LookupClient();
            IDnsQueryResponse result = await client.QueryReverseAsync(IPAddress.Parse(ipAddress));
            DnsString hostName = result.Answers.PtrRecords().FirstOrDefault()?.PtrDomainName;

            string json = hostName != null ? JsonConvert.SerializeObject(hostName) : null;

            return GetResponseObject(Constants.ReverseDNSService, json);
        }

        public async Task<JObject> GetPing(string ipAddress)
        {
            Ping pingSender = new Ping();
            PingReply reply = await pingSender.SendPingAsync(IPAddress.Parse(ipAddress));
            var response = new {
                Address = reply?.Address.ToString(),
                AddressFamily = reply?.Address?.AddressFamily.ToString(),
                Status = reply?.Status.ToString(),
                RoundtripTime = reply?.RoundtripTime
            };

            return GetResponseObject(Constants.PingService, JsonConvert.SerializeObject(response));
        }

        public async Task<JObject> GetVirusTotal(string ipAddress)
        {
            string baseURL = _configuration.GetValue<string>(Constants.VirusTotalBaseURL);
            string acceptType = _configuration.GetValue<string>(Constants.VirusTotalAccept);
            string apiKey = _configuration.GetValue<string>(Constants.VirusTotalAPIKey);
            RestClient client = new RestClient(String.Format(baseURL, apiKey, ipAddress));
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("Accept", acceptType);
            IRestResponse response = await client.ExecuteAsync(request);

            return GetResponseObject(Constants.VirusTotalService, response.Content);
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
