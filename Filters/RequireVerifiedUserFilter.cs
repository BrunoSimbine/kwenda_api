using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using bilhete24.Repository.UserRepository;

namespace bilhete24.Filters;

public class RequireVerifiedUserFilter : Attribute, IAsyncAuthorizationFilter
{
    private readonly IUserRepository _userRepository;

    public RequireVerifiedUserFilter(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        // Verifica se o usuário está autenticado
        if (user?.Identity?.IsAuthenticated == true)
        {
            var isVerified = await _userRepository.IsUserVerified();

            if (!isVerified)
            {
                context.Result = new BadRequestObjectResult(new {
                    type = "verification",
                    error = "User not verified!",
                    solution = "Verify your account to proceed."
                });
            }
        }
        else
        {
            context.Result = new UnauthorizedResult();
        }
    }
}