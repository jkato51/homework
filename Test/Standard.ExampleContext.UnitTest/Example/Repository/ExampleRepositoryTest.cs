using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Standard.ExampleContext.Domain.Models;
using Standard.ExampleContext.Infrastructure.Repositories;
using Xunit;

namespace Standard.ExampleContext.UnitTest.Example.Repository;

public class ExampleRepositoryTest
{
    [Fact]
    public async Task GetListTest()
    {
        var connection = CreateSqliteConnection();
        connection.Open();

        try
        {
            var options = SetDbContextOptionsBuilder(connection);

            await using var context = new Infrastructure.DbContext.ExampleContext(options);
            await context.Database.EnsureCreatedAsync();

            //Arrange
            var exampleOne = new ExampleContext.Domain.Entities.Example
            {
                Password = "Password01@",
                Email = "test1@test.com",
                Surname = "Surname1",
                FirstName = "FirstName1",
                Updated = DateTime.Now,
                Created = DateTime.Now
            };

            var exampleTwo = new ExampleContext.Domain.Entities.Example
            {
                Password = "Password01@",
                Email = "test2@test.com",
                Surname = "Surname2",
                FirstName = "FirstName2",
                Updated = DateTime.Now,
                Created = DateTime.Now
            };

            //Act
            var repository = new ExampleRepository(context);
            repository.Add(exampleOne);
            repository.Add(exampleTwo);
            await context.SaveChangesAsync(CancellationToken.None);
            var filter = new ExampleFilter { Email = "test" };
            var result = await repository.GetListByFilterAsync(filter);

            //Assert
            Assert.Equal(2, result.Count);
        }
        finally
        {
            connection.Close();
        }
    }

    [Fact]
    public async Task GetTest()
    {
        var connection = CreateSqliteConnection();
        connection.Open();

        try
        {
            var options = SetDbContextOptionsBuilder(connection);

            await using var context = new Infrastructure.DbContext.ExampleContext(options);
            await context.Database.EnsureCreatedAsync();

            //Arrange
            var exampleOne = new ExampleContext.Domain.Entities.Example
            {
                Password = "Password01@",
                Email = "test1@test.com",
                Surname = "Surname1",
                FirstName = "FirstName1",
                Updated = DateTime.Now,
                Created = DateTime.Now
            };

            //Act
            var repository = new ExampleRepository(context);
            repository.Add(exampleOne);
            await context.SaveChangesAsync(CancellationToken.None);
            var filter = new ExampleFilter { Email = "test1@test.com" };
            var result = await repository.GetByFilterAsync(filter);

            //Assert
            Assert.Equal("test1@test.com", result.Email);
        }
        finally
        {
            connection.Close();
        }
    }

    [Fact]
    public async Task CreateOkTest()
    {
        var connection = CreateSqliteConnection();
        connection.Open();

        try
        {
            var options = SetDbContextOptionsBuilder(connection);

            await using var context = new Infrastructure.DbContext.ExampleContext(options);
            await context.Database.EnsureCreatedAsync();

            //Arrange
            var example = new ExampleContext.Domain.Entities.Example
            {
                Password = "Password01@",
                Email = "test1@test.com",
                Surname = "Surname1",
                FirstName = "FirstName1",
                Updated = DateTime.Now,
                Created = DateTime.Now
            };

            var repository = new ExampleRepository(context);
            repository.Add(example);

            //Act

            var result = await context.SaveChangesAsync(CancellationToken.None);

            //Assert
            Assert.Equal(1, result);
        }
        finally
        {
            connection.Close();
        }
    }

    [Fact]
    public async Task DeleteTest()
    {
        var connection = CreateSqliteConnection();
        connection.Open();

        try
        {
            var options = SetDbContextOptionsBuilder(connection);

            await using var context = new Infrastructure.DbContext.ExampleContext(options);
            await context.Database.EnsureCreatedAsync();

            //Arrange
            var newExample = new ExampleContext.Domain.Entities.Example
            {
                Password = "Password01@",
                Email = "test1@test.com",
                Surname = "Surname1",
                FirstName = "FirstName1",
                Updated = DateTime.Now,
                Created = DateTime.Now
            };

            //Act
            var repository = new ExampleRepository(context);
            repository.Add(newExample);
            await context.SaveChangesAsync(CancellationToken.None);

            var filterStored = new ExampleFilter { Id = newExample.Id };
            var storedExample = await repository.GetByFilterAsync(filterStored);
            repository.Remove(storedExample);
            await context.SaveChangesAsync(CancellationToken.None);

            var filterNonExistentUser = new ExampleFilter { Id = newExample.Id };
            var nonExistentUser = await repository.GetByFilterAsync(filterNonExistentUser);

            //Assert
            Assert.Null(nonExistentUser);
        }
        finally
        {
            connection.Close();
        }
    }

    [Fact]
    public async Task DuplicatedEmailTest()
    {
        var connection = CreateSqliteConnection();
        connection.Open();

        try
        {
            var options = SetDbContextOptionsBuilder(connection);

            await using var context = new Infrastructure.DbContext.ExampleContext(options);
            await context.Database.EnsureCreatedAsync();

            //Arrange
            var exampleOne = new ExampleContext.Domain.Entities.Example
            {
                Password = "Password01@",
                Email = "test1@test.com",
                Surname = "Surname1",
                FirstName = "FirstName1",
                Created = DateTime.Now
            };

            var exampleTwo = new ExampleContext.Domain.Entities.Example
            {
                Password = "Password01@",
                Email = "test1@test.com",
                Surname = "Surname2",
                FirstName = "FirstName2",
                Created = DateTime.Now
            };

            //Act
            var repository = new ExampleRepository(context);
            repository.Add(exampleOne);
            await context.SaveChangesAsync(CancellationToken.None);
            repository.Add(exampleTwo);
            var exception =
                await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync(CancellationToken.None));

            //Assert
            Assert.NotNull(exception);
        }
        finally
        {
            connection.Close();
        }
    }

    [Fact]
    public async Task CreateInvalidEmailTest()
    {
        var connection = CreateSqliteConnection();
        connection.Open();

        try
        {
            var options = SetDbContextOptionsBuilder(connection);

            await using var context = new Infrastructure.DbContext.ExampleContext(options);
            await context.Database.EnsureCreatedAsync();

            //Arrange
            var example = new ExampleContext.Domain.Entities.Example
            {
                Password = "Password01@",
                Email = null,
                Surname = "Surname1",
                FirstName = "FirstName1",
                Created = DateTime.Now
            };

            //Act
            var repository = new ExampleRepository(context);
            repository.Add(example);
            var exception =
                await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync(CancellationToken.None));

            //Assert
            Assert.NotNull(exception);
        }
        finally
        {
            connection.Close();
        }
    }

    [Fact]
    public async Task CreateInvalidPasswordTest()
    {
        var connection = CreateSqliteConnection();
        connection.Open();

        try
        {
            var options = SetDbContextOptionsBuilder(connection);

            await using var context = new Infrastructure.DbContext.ExampleContext(options);
            await context.Database.EnsureCreatedAsync();

            //Arrange
            var example = new ExampleContext.Domain.Entities.Example
            {
                Password = null,
                Email = "test@test.com",
                Surname = "Surname1",
                FirstName = "FirstName1",
                Created = DateTime.Now
            };

            //Act
            var repository = new ExampleRepository(context);
            repository.Add(example);

            var exception =
                await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync(CancellationToken.None));

            //Assert
            Assert.NotNull(exception);
        }
        finally
        {
            connection.Close();
        }
    }

    [Fact]
    public async Task CreateInvalidFirstNameTest()
    {
        var connection = CreateSqliteConnection();
        connection.Open();

        try
        {
            var options = SetDbContextOptionsBuilder(connection);

            await using var context = new Infrastructure.DbContext.ExampleContext(options);
            await context.Database.EnsureCreatedAsync();

            //Arrange
            var example = new ExampleContext.Domain.Entities.Example
            {
                Password = "Passw0rd1",
                Email = "test@test.com",
                Surname = "Surname1",
                FirstName = null,
                Created = DateTime.Now
            };

            //Act
            var repository = new ExampleRepository(context);
            repository.Add(example);
            var exception =
                await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync(CancellationToken.None));

            //Assert
            Assert.NotNull(exception);
        }
        finally
        {
            connection.Close();
        }
    }

    [Fact]
    public async Task CreateInvalidSurnameTest()
    {
        var connection = CreateSqliteConnection();
        connection.Open();

        try
        {
            var options = SetDbContextOptionsBuilder(connection);

            await using var context = new Infrastructure.DbContext.ExampleContext(options);
            await context.Database.EnsureCreatedAsync();

            //Arrange
            var example = new ExampleContext.Domain.Entities.Example
            {
                Password = "Passw0rd1",
                Email = "test@test.com",
                Surname = null,
                FirstName = "FirstName",
                Created = DateTime.Now
            };

            //Act
            var repository = new ExampleRepository(context);
            repository.Add(example);

            var exception =
                await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync(CancellationToken.None));

            //Assert
            Assert.NotNull(exception);
        }
        finally
        {
            connection.Close();
        }
    }

    private static DbContextOptions<Infrastructure.DbContext.ExampleContext> SetDbContextOptionsBuilder(
        DbConnection connection)
    {
        return new DbContextOptionsBuilder<Infrastructure.DbContext.ExampleContext>()
            .UseSqlite(connection)
            .Options;
    }

    private static SqliteConnection CreateSqliteConnection()
    {
        return new SqliteConnection("DataSource=:memory:");
    }
}