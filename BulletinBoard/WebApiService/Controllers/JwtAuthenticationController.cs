using Authentication.AppServices.JwtAuthentication;
using Authenticaton.Contracts.Basic;
using Authenticaton.Contracts.JwtAuthentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.Contracts.DTO;

namespace WebApiService.Controllers
{
    [ApiController, Route("authentication/jwt")]
    public class JwtAuthenticationController : ControllerBase
    {
        private readonly IJwtAuthenticationService _authenticationService;

        public JwtAuthenticationController(IJwtAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<ActionResult<JwtAuthenticationToken>> Authenticate(BasicAuthenticationRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }
            var result = await _authenticationService.AuthenticateAsync(request);
            if (!result.IsSucced)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Token);
        }

        [HttpPost]
        [Route("ext")]
        public async Task<ActionResult<JwtAuthenticationToken>> AuthenticateExternal([FromBody]string UserEmail)
        {
            if (UserEmail == null)
            {
                return BadRequest();
            }
            var result = await _authenticationService.AuthenticateExternalAsync(UserEmail);
            if (!result.IsSucced)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Token);
        }
    }
}
