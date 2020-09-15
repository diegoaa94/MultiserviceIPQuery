using Xunit;
using MultiserviceIPQuery.Services;
using MultiserviceIPQuery.Interfaces;
using MultiserviceIPQueryTest.Fakes;
using System.Collections.Generic;
using MultiserviceIPQuery.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MultiserviceIPQueryTest
{
    public class MultiServiceQueryControllerTest
    {
        MultiServiceQueryController _controller;

        public MultiServiceQueryControllerTest()
        {
            IQueryRepository queryRepository = new QueryRepositoryFake();
            IQueryService queryService = new QueryService(queryRepository);
            _controller = new MultiServiceQueryController(queryService);
        }

        [Fact]
        public void Get_OkResult()
        {
            string ipAddress = "8.8.8.8";
            List<string> services = new List<string> { "GeoIP", "ReverseDNS" };
            var result = _controller.Get(ipAddress, services);
            
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Get_BadRequestResult()
        {
            string ipAddress = "8.8.8.8.8";
            List<string> services = new List<string> { "GeoIP" };
            var result = _controller.Get(ipAddress, services);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Get_NotFoundResult()
        {
            string ipAddress = "4.4.4.4";
            List<string> services = new List<string> { "OpenPorts", "WebsiteStatus" };
            var result = _controller.Get(ipAddress, services);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Get_InternalServerError()
        {
            string ipAddress = "3.3.3.3";
            List<string> services = new List<string> { "VirusTotal", "WebsiteStatus" };
            var result = _controller.Get(ipAddress, services);
            var objectResponse = result as ObjectResult;

            Assert.Equal(StatusCodes.Status500InternalServerError, objectResponse.StatusCode);
        }
    }
}
