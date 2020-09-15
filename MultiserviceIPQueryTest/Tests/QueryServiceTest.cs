using Xunit;
using MultiserviceIPQuery.Services;
using MultiserviceIPQuery.Interfaces;
using MultiserviceIPQueryTest.Fakes;
using System.Collections.Generic;

namespace MultiserviceIPQueryTest
{
    public class QueryServiceTest
    {
        QueryService _queryService;

        public QueryServiceTest()
        {
            IQueryRepository queryRepository = new QueryRepositoryFake();
            _queryService = new QueryService(queryRepository);
        }

        [Fact]
        public void IsValidIPAddress_True()
        {
            string ipAddress = "8.8.4.4";
            bool result = _queryService.IsValidIPAddress(ipAddress);
            Assert.True(result);
        }

        [Fact]
        public void IsValidIPAddress_False_InvalidFormat()
        {
            string ipAddress = ".8.8.4.5.6";
            bool result = _queryService.IsValidIPAddress(ipAddress);
            Assert.False(result);
        }

        [Fact]
        public void IsValidIPAddress_False_Null()
        {
            string ipAddress = null;
            bool result = _queryService.IsValidIPAddress(ipAddress);
            Assert.False(result);
        }

        [Fact]
        public void IsValidIPAddress_False_Empty()
        {
            string ipAddress = "";
            bool result = _queryService.IsValidIPAddress(ipAddress);
            Assert.False(result);
        }

        [Fact]
        public void QueryIPAddress_GetGeoIp()
        {
            string ipAddress = "3.3.3.3";
            List<string> services = new List<string> { "geoip" };
        var result = _queryService.QueryIPAddress(ipAddress, services);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public void QueryIPAddress_GetReverseDNS()
        {
            string ipAddress = "8.8.8.8";
            List<string> services = new List<string> { "ReverseDNS" };
            var result = _queryService.QueryIPAddress(ipAddress, services);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public void QueryIPAddress_GetPing()
        {
            string ipAddress = "8.8.4.4";
            List<string> services = new List<string> { "PING" };
            var result = _queryService.QueryIPAddress(ipAddress, services);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public void QueryIPAddress_ServicesNotSupported()
        {
            string ipAddress = "8.8.8.8";
            List<string> services = new List<string> { "OpenPorts", "WebsiteStatus", "DomainAvailability" };
            var result = _queryService.QueryIPAddress(ipAddress, services);
            Assert.Empty(result.Result);
        }
    }
}
