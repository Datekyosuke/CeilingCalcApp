using CeilingCalc.Models;

namespace CeilingCalc.Autorization
{
    public interface IUserService
    {

        bool IsValidUserInformation(LoginModel model);
    }
}
