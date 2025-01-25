using MovieChest.Movies;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MovieChest;

public class FakeMovieCollectionSerializer : IMovieCollectionSerializer
{
    private Movie[] _movies =
    [
        new(0, "Kung Pow"),
    ];

    public IEnumerable<Movie> Read(Stream from)
        => _movies;

    public void Write(IEnumerable<Movie> movies, Stream to)
        => _movies = movies.ToArray();
}