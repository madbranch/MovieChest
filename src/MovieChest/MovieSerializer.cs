using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieChest;

public class MovieSerializer : IMovieSerializer
{
    private readonly Func<SqliteConnectionStringBuilder, string, SqliteConnectionStringBuilder> createConnectionString;

    public MovieSerializer()
        : this(CreateDefaultConnectionString)
    {}

    public MovieSerializer(Func<SqliteConnectionStringBuilder, string, SqliteConnectionStringBuilder> createConnectionString)
        => this.createConnectionString = createConnectionString;

    public IEnumerable<MovieItem> GetMovies(string path)
    {
        using SqliteConnection connection = CreateReadOnlyConnection(path);
        return Enumerable.Empty<MovieItem>();
    }

    private SqliteConnection CreateReadOnlyConnection(string path)
    {
        SqliteConnectionStringBuilder builder = new();
        builder.Mode = SqliteOpenMode.ReadOnly;
        builder = createConnectionString(builder, path);        
        return new(builder.ConnectionString);
    }

    public void SetMovies(string path, IEnumerable<MovieItem> movies)
    {
        using SqliteConnection connection = CreateWriteConnection(path);
        connection.Open();
        using var transaction = connection.BeginTransaction();
        CreateMovieTableIfNotExists(connection);
        ClearMovieTable(connection);
        InsertMovies(connection, movies);
    }

    private void CreateMovieTableIfNotExists(SqliteConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandText = """create table if not exists Movie(id integer primary key, title text not null, description text not null, tags text not null, path text, volume_label text)""";
        command.ExecuteNonQuery();
    }

    private void ClearMovieTable(SqliteConnection connection)
    {
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = """delete from Movie""";
        command.ExecuteNonQuery();
    }
    
    private void InsertMovies(SqliteConnection connection, IEnumerable<MovieItem> movies)
    {
        foreach (MovieItem movie in movies)
        {
            SqliteCommand command = connection.CreateCommand();
            if (movie.Path is not null)
            {
                if (movie.VolumeLabel is null)
                {
                    throw new InvalidOperationException("Volume label should not be null whenever the path is valid.");
                }
                command.CommandText = """
                    insert into Movie (title, description, tags, path, volume_label)
                        values ($title, $description, $tags, $path, $volume_label)
                    """;
                command.Parameters.AddWithValue("$title", movie.Title);
                command.Parameters.AddWithValue("$description", movie.Description);
                command.Parameters.AddWithValue("$tags", movie.Tags);
                command.Parameters.AddWithValue("$path", movie.Path);
                command.Parameters.AddWithValue("$volume_label", movie.VolumeLabel);
            }
            else
            {
                command.CommandText = """
                    insert into Movie (title, description, tags)
                        values ($title, $description, $tags)
                    """;
                command.Parameters.AddWithValue("$title", movie.Title);
                command.Parameters.AddWithValue("$description", movie.Description);
                command.Parameters.AddWithValue("$tags", movie.Tags);
            }
            command.ExecuteNonQuery();
        }
    }

    private SqliteConnection CreateWriteConnection(string path)
    {
        SqliteConnectionStringBuilder builder = new()
        {
            Mode = SqliteOpenMode.ReadWriteCreate,
        };
        builder = createConnectionString(builder, path);        
        return new(builder.ConnectionString);
    }

    private static SqliteConnectionStringBuilder CreateDefaultConnectionString(SqliteConnectionStringBuilder builder, string path)
    {
        builder.DataSource = path;
        return builder;
    }
}