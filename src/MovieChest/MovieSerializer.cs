using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieChest;

public class MovieSerializer : IMovieSerializer
{
    public IEnumerable<MovieItem> GetMovies(string path)
    {
        //using SqliteConnection connection = new();
        return Enumerable.Empty<MovieItem>();
    }

    public void SetMovies(string path, IEnumerable<MovieItem> movies)
    {
        Console.WriteLine($"SetMovies: {path}");
    }
}