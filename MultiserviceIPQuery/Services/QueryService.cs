using MultiserviceIPQuery.Common;
using MultiserviceIPQuery.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MultiserviceIPQuery.Services
{
    public class QueryService : IQueryService
    {
        private IQueryRepository _queryRepository;

        public QueryService(IQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        public async Task<List<JObject>> QueryIPAddress(string ipAddress, List<string> services)
        {
            List<string> serviceList = 
                services != null && services.Count > 0 ? services : Constants.DefaultServices;
            List<JObject> serviceResults = new List<JObject>();
            List <Task<JObject>> tasks = new List<Task<JObject>>();
            Task<JObject> task;

            foreach (string service in serviceList) {

                if (IsSupportedService(service))
                {
                    task = Task.Run(() =>
                    {
                        return (Task<JObject>)_queryRepository.
                            ServiceMethods[service.ToLower().Trim()].DynamicInvoke(ipAddress);
                    });
                    tasks.Add(task);
                }
            }

            if (tasks.Count > 0) {
                Task.WaitAll(tasks.ToArray());

                foreach (var taskResult in tasks)
                {
                    serviceResults.Add(taskResult.Result);
                }
            }

            return serviceResults;
        }

        public bool IsValidIPAddress(string ipAddress)
        {
            Regex check = new Regex(Constants.IPAddressRegEx); 
            return check.IsMatch(ipAddress, 0);
        }

        private bool IsSupportedService(string serviceName)
        {
            return Constants.SupportedServices.Contains(serviceName?.ToLower().Trim());
        }
    }
}
