using System;
using System.Collections.Generic;

namespace MovieChest;

public interface IMovieSerializer
{
    IEnumerable<MovieItem> GetMovies(Uri path);
    void SetMovies(Uri path, IEnumerable<MovieItem> movies);
}
