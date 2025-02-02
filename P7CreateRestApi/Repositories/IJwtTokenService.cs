using Microsoft.AspNetCore.Identity;
using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Repositories
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(User user);
    }
}
