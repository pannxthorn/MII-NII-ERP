using ERP.Application.company;
using ERP.Application.company.queries;
using ERP.Application.users;
using ERP.ApplicationDTO.company;
using ERP.Shared._base;
using ERP.Shared._base.BaseResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CompanyController(IMediator mediator) { _mediator = mediator; }

        [HttpGet]
        [Route("GetAllCompany")]
        public async Task<IActionResult> GettAllCompany([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var paginationRequest = new PaginationRequest(pageNumber, pageSize);
            var result = await _mediator.Send(new QueryGetAllCompany(paginationRequest));
            return Ok(result);
        }

        [HttpGet]
        [Route("GetCompanyById")]
        public async Task<IActionResult> GetCompanyById(string companyId)
        {
            var users = await _mediator.Send(new QueryGetCompanyById(companyId));
            return Ok(users);
        }

        [HttpPost]
        [Route("CreateCompany")]
        public async Task<ActionResult<BaseResponse<CompanyDTO>>> CreateCompany([FromBody] CreateCompanyDTO companyDTO)
        {
            return await _mediator.Send(new CommandCreateCompany(companyDTO));
        }
    }
}
