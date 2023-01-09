using jwtMinimalAPi.Model;

namespace jwtMinimalAPi.Repository
{
    public class MovieRepository
    {
        public static List<Movie> movie = new List<Movie>()
        {
            new Movie(){Id = 1, Title = "External", Description = "The lord is our strength in the time of need", Rating = 6.8},
            new Movie{ Id =2, Title ="The Ghost", Description = " The display of the power of the ghost of heaven", Rating =8.0},
            new Movie { Id = 3, Title = " Eucharistic Miracle", Description = "The display of the power of God in the Eucharistic celebration", Rating = 9.0}
        };
    }
}
