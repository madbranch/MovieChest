using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieChest;

public class MovieSerializer : IMovieSerializer
{
    private readonly Func<string, SqliteConnection> connectionFactory;

    public MovieSerializer()
        : this(path => new SqliteConnection($"Data Source='{path}'"))
    {}

    public MovieSerializer(Func<string, SqliteConnection> connectionFactory)
        => this.connectionFactory = connectionFactory;

    public IEnumerable<MovieItem> GetMovies(string path)
    {
        using SqliteConnection connection = connectionFactory(path);
        return Enumerable.Empty<MovieItem>();
    }

    public void SetMovies(string path, IEnumerable<MovieItem> movies)
    {
        Console.WriteLine($"SetMovies: {path}");
    }
}