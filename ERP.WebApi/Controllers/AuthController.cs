using ERP.Application.users;
using ERP.ApplicationDTO.users;
using ERP.Shared._base.BaseResponse;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<BaseResponse<LoginResponseDTO>>> Login([FromBody] LoginDTO loginDTO)
        {
            var response = await _mediator.Send(new CommandLogin(loginDTO));

            if (!response.IsSuccess)
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }
    }
}
