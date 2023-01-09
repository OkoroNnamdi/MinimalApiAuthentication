using jwtMinimalAPi.Model;

namespace jwtMinimalAPi.Services
{
    public interface IUserService
    {
        public User Get (UserLogin userLogin);
    }
}
