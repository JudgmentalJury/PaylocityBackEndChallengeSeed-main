using Api.Database;
using Api.Dtos.Dependent;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        GetDependentDto dependent = await Data.GetDependentDto(id);

        if (dependent != null)
        {
            return new ApiResponse<GetDependentDto>
            {
                Data = dependent,
                Success = true
            };
        }
        else
            return new NotFoundResult();
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        List<GetDependentDto> dependents = await Data.GetAllDependentDtos();

        if (dependents.Count > 0)
        {
            return new ApiResponse<List<GetDependentDto>>
            {
                Data = dependents,
                Success = true
            };
        }
        else
            return new NotFoundResult();
    }
}
