using Microsoft.AspNetCore.Mvc;

namespace Cep.App.Host.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class BaseController : ControllerBase
{
}