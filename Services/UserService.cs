using jwtMinimalAPi.Model;
using jwtMinimalAPi.Repository;

namespace jwtMinimalAPi.Services
{
    public class UserService : IUserService
    {
        public User Get(UserLogin userLogin)
        {
            User user = UserRepository.user.FirstOrDefault(o =>
            o.Username.Equals(userLogin.UserName, StringComparison.OrdinalIgnoreCase) &&
            (o.Password.Equals(userLogin.Password)));
            return user;
        }
    }
}
