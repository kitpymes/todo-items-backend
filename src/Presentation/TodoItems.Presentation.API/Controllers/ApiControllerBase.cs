using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TodoItems.Presentation.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public abstract class ApiControllerBase : ControllerBase
{
#nullable disable

    private ISender _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

#nullable restore
}
