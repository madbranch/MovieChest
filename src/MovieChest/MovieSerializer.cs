using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace MovieChest;

public class MovieSerializer(Func<SqliteConnectionStringBuilder, string, SqliteConnectionStringBuilder> createConnectionString) : IMovieSerializer
{
    private readonly Func<SqliteConnectionStringBuilder, string, SqliteConnectionStringBuilder> createConnectionString = createConnectionString;

    public MovieSerializer()
        : this(CreateDefaultConnectionString)
    { }

    public IEnumerable<MovieItem> GetMovies(string path)
    {
        using SqliteConnection connection = CreateReadOnlyConnection(path);
        connection.Open();
        if (!HasMovieTable(connection))
        {
            yield break;
        }
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = """select title, description, tags, path, volume_label from Movie""";
        using SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            if (reader.IsDBNull(3) || reader.IsDBNull(4))
            {
                yield return new MovieItem(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    null,
                    null
                );
            }
            else
            {
                yield return new MovieItem(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4)
                );
            }
        }
    }

    private static bool HasMovieTable(SqliteConnection connection)
      => TableExists(connection, "Movie");

    private static bool TableExists(SqliteConnection connection, string tableName)
    {
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = $"select name from sqlite_master where type='table' and name='{tableName}';";
        using SqliteDataReader reader = command.ExecuteReader();
        return reader.HasRows;
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
        using SqliteTransaction transaction = connection.BeginTransaction();
        CreateMovieTableIfNotExists(connection);
        ClearMovieTable(connection);
        InsertMovies(connection, movies);
        transaction.Commit();
    }

    private static void CreateMovieTableIfNotExists(SqliteConnection connection)
    {
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = """create table if not exists Movie(id integer primary key, title text not null, description text not null, tags text not null, path text, volume_label text)""";
        command.ExecuteNonQuery();
    }

    private static void ClearMovieTable(SqliteConnection connection)
    {
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = """delete from Movie""";
        command.ExecuteNonQuery();
    }

    private static void InsertMovies(SqliteConnection connection, IEnumerable<MovieItem> movies)
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