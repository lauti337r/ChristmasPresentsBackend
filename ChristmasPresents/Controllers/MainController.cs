using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChristmasPresents.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace ChristmasPresents.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class MainController : ControllerBase
    {
        public MainController()
        {
        }

        [HttpGet("Authenticate")]
        [Authorize]
        public async Task<ActionResult<bool>> Authenticate()
        {
            return true;
        }
    }
}
