using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Spentir.Controllers.Base
{
    public abstract class BaseController : ControllerBase
    {
        protected string UserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? User.FindFirst("sub")?.Value
        ?? throw new UnauthorizedAccessException("User ID not found.");
    }
}