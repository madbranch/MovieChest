using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieChest;

public class MovieSerializer : IMovieSerializer
{
    public IEnumerable<MovieItem> GetMovies(Uri path)
    {
        Console.WriteLine($"GetMovies: {path.AbsolutePath}");
        return Enumerable.Empty<MovieItem>();
    }

    public void SetMovies(Uri path, IEnumerable<MovieItem> movies)
    {
        Console.WriteLine($"SetMovies: {path.AbsolutePath}");
    }
}
