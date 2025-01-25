using MovieChest.Movies;
using System.Collections.Generic;
using System.IO;

namespace MovieChest;

public interface IMovieCollectionSerializer
{
    IEnumerable<Movie> Read(Stream from);
    void Write(IEnumerable<Movie> movies, Stream to);
}