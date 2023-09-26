using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TwitterLogIn_BackEnd.Data.Interface;

namespace TwitterLogIn_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwitterClientController : ControllerBase
    {
        private readonly ITwitterAuthRepository _twitterAuth;
        //private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        //private readonly DataContext _context;

        public TwitterClientController(ITwitterAuthRepository twitterAuth, IConfiguration config)//, IMapper mapper , DataContext context
        {
            //_context = context;
            _config = config;
            //_mapper = mapper;
            _twitterAuth = twitterAuth;

        }

        [HttpGet("GetRequestToken")]
        public async Task<IActionResult> GetRequestToken()
        {
            //call made to /oauth/request_token
            var response = await _twitterAuth.GetRequestToken();

            return Ok(response);
        }
    }
}
