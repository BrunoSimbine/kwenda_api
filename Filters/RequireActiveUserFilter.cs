using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using bilhete24.Repository.UserRepository;

namespace bilhete24.Filters;

public class RequireActiveUserFilter : Attribute, IAsyncAuthorizationFilter
{
    private readonly IUserRepository _userRepository;

    public RequireActiveUserFilter(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        // Verifica se o usuário está autenticado
        if (user?.Identity?.IsAuthenticated == true)
        {
            var isDeleted = await _userRepository.IsUserDeleted();

            if (isDeleted)
            {
                context.Result = new BadRequestObjectResult(new {
                    type = "availability",
                    error = "User are deleted!",
                    solution = "Recover your account to proceed."
                });
            }
        }
        else
        {
            context.Result = new UnauthorizedResult();
        }
    }
}