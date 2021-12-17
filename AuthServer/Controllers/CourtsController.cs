using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Contracts;

namespace CourtsCheckSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourtsController : Controller
    {
        private readonly ILogger<CourtsController> logger;
        private readonly ICourtService courtService;

        public CourtsController(ILogger<CourtsController> _logger, ICourtService _courtService)
        {
            logger = _logger;
            courtService = _courtService;
        }
    }
}