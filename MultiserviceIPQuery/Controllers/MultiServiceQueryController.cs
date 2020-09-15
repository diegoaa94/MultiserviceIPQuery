using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiserviceIPQuery.Interfaces;
using Newtonsoft.Json.Linq;

namespace MultiserviceIPQuery.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MultiServiceQueryController : ControllerBase
    {
        readonly IQueryService _queryService;

        public MultiServiceQueryController(IQueryService queryService)
        {
            _queryService = queryService;
        }

        [HttpGet("{ipAddress}")]
        public IActionResult Get(string ipAddress, [FromQuery] List<string> services)
        {
            try
            {
                List<JObject> result;

                if (!_queryService.IsValidIPAddress(ipAddress))
                {
                    return BadRequest(ipAddress);
                }

                result = _queryService.QueryIPAddress(ipAddress, services).Result;

                if (result.Count == 0)
                {
                    return NotFound(ipAddress);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}
