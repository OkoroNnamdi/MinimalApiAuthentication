using jwtMinimalAPi.Model;

namespace jwtMinimalAPi.Repository
{
    public class UserRepository
    {
       public static  List<User> user = new List<User>()
        {
            new User(){ Username="okoronnamdi", Surname ="Okoro", EmailAddress ="Okoronnamdi4044@gmail.com", GivenName ="Nnamdi", Password = "12345", Role ="Admin"},
             new User(){ Username="ItumaEmeka ", Surname ="Ituma", EmailAddress ="Okoronnamdi4044@gmail.com", GivenName ="Emeka", Password = "12345", Role ="User"}
        };
    }
}
