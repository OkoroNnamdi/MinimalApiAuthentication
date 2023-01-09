using jwtMinimalAPi.Model;
using jwtMinimalAPi.Repository;

namespace jwtMinimalAPi.Services
{
    public class MovieService : IMovieService
    {
        public Movie Create(Movie movie)
        {

            movie.Id = MovieRepository.movie.Count + 1;
            MovieRepository.movie.Add(movie);
            return movie;


        }

        public bool Delete(int id)
        {
           var oldMovie = MovieRepository.movie.FirstOrDefault(x => x.Id == id);
            if(oldMovie == null)
            {
            return false;
            }
            MovieRepository.movie.Remove(oldMovie);
            return true;
        }

        public Movie Get(int id)
        {
           var movie = MovieRepository.movie .FirstOrDefault (x => x.Id == id);
            if (movie == null)
                return null;
            return movie;
        }

        public List<Movie> List()
        {
            var movie = MovieRepository.movie.ToList();
            return movie;
        }

        public Movie Update(Movie movie)
        {
          var oldMovie = MovieRepository.movie.FirstOrDefault (x => x.Id == movie.Id);
            if (oldMovie == null)
                return null;
            movie.Title = oldMovie.Title;
            movie .Description = oldMovie.Description;
            movie.Rating = oldMovie.Rating;
            return movie;
        }
    }
}
