using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

namespace MovieChest.Tests;

public class MovieSerializerTests
{
    [Test]
    public async Task GetMovies_EmptyDatabse_ShouldBeEmpty()
    {
        MovieSerializer movieSerializer = new(CreateConnection);
        await Assert.That(movieSerializer.GetMovies("whatever")).IsEmpty();
    }
    [Test]
    public async Task GetMovies_FilledDatabse_ShouldNotBeEmpty()
    {
        SqliteConnectionStringBuilder builder = CreateConnection(new(), "whatever");
        using SqliteConnection connection = new(builder.ConnectionString);
        MovieItem[] movies =
        [
            new("Some Title", "Some Description", "Some Tags", null, ""),
            new("Some Title 2", "Some Description 2", "Some Tags 2", "/Some/Path", "Strawberry"),
        ];

        MovieSerializer movieSerializer = new(CreateConnection);
        movieSerializer.SetMovies("whatever", movies);
        await Assert.That(movieSerializer.GetMovies("whatever")).IsNotEmpty();
    }

    private static SqliteConnectionStringBuilder CreateConnection(SqliteConnectionStringBuilder builder, string path)
    {
        // We ignore the provided path and create a connection to an in-memory database.
        builder.DataSource="InMemorySample";
        builder.Mode = SqliteOpenMode.Memory;
        builder.Cache = SqliteCacheMode.Shared;
        return builder;
    }
}