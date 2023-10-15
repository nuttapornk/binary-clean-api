using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _COMPONENT_.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    private ISender _mediator = null!;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
