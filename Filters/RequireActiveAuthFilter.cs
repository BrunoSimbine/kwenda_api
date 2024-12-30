using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using bilhete24.Repository.AuthRepository;

namespace bilhete24.Filters;

public class RequireActiveAuthFilter : Attribute, IAsyncAuthorizationFilter
{
    private readonly IAuthRepository _authRepository;

    public RequireActiveAuthFilter(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        // Verifica se o usuário está autenticado
        if (user?.Identity?.IsAuthenticated == true)
        {
            var isDeleted = await _authRepository.IsDeleted();

            if (isDeleted)
            {
                context.Result = new BadRequestObjectResult(new {
                    type = "authentication",
                    error = "Your authentication are invalid!",
                    solution = "Create a new auth to proceed."
                });
            }
        }
        else
        {
            context.Result = new UnauthorizedResult();
        }
    }
}