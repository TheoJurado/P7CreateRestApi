using Microsoft.AspNetCore.Identity;

namespace P7CreateRestApi.Repositories
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(IdentityUser user);
    }
}
