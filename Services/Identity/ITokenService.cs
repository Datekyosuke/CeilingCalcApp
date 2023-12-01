using CeilingCalc.Autorization;
using Microsoft.AspNetCore.Identity;

namespace CeilingCalc.Services.Identity
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user, List<IdentityRole<long>> role);

    }
}
