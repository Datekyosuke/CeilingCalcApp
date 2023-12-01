using WebApiDB.Context;

namespace CeilingCalc.Autorization
{
    public class UserService : IUserService
    {
        private readonly AplicationContext _context;

        public UserService(AplicationContext context)
        {
            _context = context;
        }


        public bool IsValidUserInformation(LoginModel model)
        {
            var user = (_context.Operators.FirstOrDefault(x => x.Login == model.UserName));

            if( user.Password.Equals(model.Password)) return true;
             else return false;
        }
    }
}
