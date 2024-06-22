using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/debug")]
    [ApiController]
    public class DebugController : ControllerBase
    {
        private IWebScraper _webScraper;
        public DebugController(IWebScraper webScraper)
        {
            this._webScraper = webScraper;
        }

        public IActionResult GetAllRelics()
        {
            return Ok();
        }
    }
}