using ERP.Application.users;
using ERP.ApplicationDTO.users;
using ERP.Shared._base.BaseResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator) { _mediator = mediator; }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _mediator.Send(new QueryGetAllUsers());
            return Ok(users);
        }

        [HttpGet]
        [Route("GetUserById")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var users = await _mediator.Send(new QueryGetUserById(userId));
            return Ok(users);
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<ActionResult<BaseResponse<UserDTO>>> CreateUser ([FromBody] CreateUserDTO userDTO)
        {
            return await _mediator.Send(new CommandCreateUser(userDTO));
        }
    }
}
