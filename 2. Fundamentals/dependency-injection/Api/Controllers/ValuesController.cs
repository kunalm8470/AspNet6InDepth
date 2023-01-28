using Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private readonly ISingletonOperation _singletonOperation;
    private readonly IScopedOperation _scopedOperation;
    private readonly ITransitiveOperation _transitiveOperation;
    private readonly IDummyService _dummyService;

    public ValuesController(
        ISingletonOperation singletonOperation,
        IScopedOperation scopedOperation,
        ITransitiveOperation transitiveOperation,
        IDummyService dummyService
    )
    {
        _singletonOperation = singletonOperation;
        _scopedOperation = scopedOperation;
        _transitiveOperation = transitiveOperation;
        _dummyService = dummyService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _dummyService.PrintOperationId();

        var response = new
        {
            singleton = _singletonOperation.OperationId,
            scoped = _scopedOperation.OperationId,
            transitive = _transitiveOperation.OperationId
        };

        return Ok(response);
    }
}
