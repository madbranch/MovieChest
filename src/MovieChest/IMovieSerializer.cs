using System.Collections.Generic;

namespace MovieChest;

public interface IMovieSerializer
{
    IEnumerable<MovieItem> GetMovies(string path);
    void SetMovies(string path, IEnumerable<MovieItem> movies);
}