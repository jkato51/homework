using Moq;
using Standard.ExampleContext.Domain.DbContext;
using Standard.ExampleContext.Domain.Exceptions;
using Standard.ExampleContext.Domain.Models;
using Standard.ExampleContext.Domain.Repositories;
using Standard.ExampleContext.Domain.Services;
using Standard.ExampleContext.Domain.Services.Interfaces;
using Xunit;

namespace Standard.ExampleContext.UnitTest.Example.Domain;

public class ExampleServiceTest
{
    [Fact]
    public async Task GetTest()
    {
        //Arrange
        var example = new ExampleContext.Domain.Entities.Example
        {
            Id = 1,
            Password = "Password@01",
            Email = "test@test.com",
            Surname = "Surname",
            FirstName = "FirstName",
            Updated = DateTime.Now,
            Created = DateTime.Now
        };

        var mockExampleContext = new Mock<IExampleContext>();
        var mockContextFactory = new Mock<IDbContextFactory>();
        var mockRepository = new Mock<IExampleRepository>();
        var mockPassword = new Mock<IPasswordHasherService>();
        mockContextFactory.Setup(x => x.CreateContext()).Returns(mockExampleContext.Object);
        mockContextFactory.Setup(x => x.CreateExampleRepository(mockExampleContext.Object))
            .Returns(mockRepository.Object);
        var filter = new ExampleFilter { Id = 1 };
        mockRepository.Setup(x => x.GetByFilterAsync(filter)).ReturnsAsync(example);
        var mockService = new ExampleService(mockContextFactory.Object, mockPassword.Object);

        //Act
        var result = await mockService.GetByFilterAsync(filter);

        //Assert
        Assert.Equal(example.Id, result.Id);
    }

    [Fact]
    public async Task GetListTest()
    {
        //Arrange
        var exampleOne = new ExampleContext.Domain.Entities.Example
        {
            Id = 1,
            Password = "Password@01",
            Email = "test1@test.com",
            Surname = "Surname",
            FirstName = "FirstName",
            Updated = DateTime.Now,
            Created = DateTime.Now
        };

        var exampleTwo = new ExampleContext.Domain.Entities.Example
        {
            Id = 2,
            Password = "Password@01",
            Email = "test2@test.com",
            Surname = "Surname",
            FirstName = "FirstName",
            Updated = DateTime.Now,
            Created = DateTime.Now
        };

        var examples = new List<ExampleContext.Domain.Entities.Example>
        {
            exampleOne,
            exampleTwo
        };

        var mockExampleContext = new Mock<IExampleContext>();
        var mockContextFactory = new Mock<IDbContextFactory>();
        var mockRepository = new Mock<IExampleRepository>();
        var mockPassword = new Mock<IPasswordHasherService>();
        mockContextFactory.Setup(x => x.CreateContext()).Returns(mockExampleContext.Object);
        mockContextFactory.Setup(x => x.CreateExampleRepository(mockExampleContext.Object))
            .Returns(mockRepository.Object);
        var filter = new ExampleFilter { PageSize = 10, CurrentPage = 1 };
        mockRepository.Setup(x => x.CountByFilterAsync(filter))
            .ReturnsAsync(examples.Count);
        mockRepository.Setup(x => x.GetListByFilterAsync(filter))
            .ReturnsAsync(examples);
        var mockService = new ExampleService(mockContextFactory.Object, mockPassword.Object);

        //Act
        var result = await mockService.GetListByFilterAsync(filter);

        //Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task CreateInvalidFirstNameMinLengthTest()
    {
        //Arrange
        var example = new ExampleContext.Domain.Entities.Example
        {
            Password = "Password@01",
            Email = "test1@test.com",
            Surname = "Surname",
            FirstName = "F"
        };

        var mockExampleContext = new Mock<IExampleContext>();
        var mockContextFactory = new Mock<IDbContextFactory>();
        var mockRepository = new Mock<IExampleRepository>();
        var mockPassword = new Mock<IPasswordHasherService>();
        mockContextFactory.Setup(x => x.CreateContext()).Returns(mockExampleContext.Object);
        mockContextFactory.Setup(x => x.CreateExampleRepository(mockExampleContext.Object))
            .Returns(mockRepository.Object);
        var mockService = new ExampleService(mockContextFactory.Object, mockPassword.Object);

        //Act
        var exception = await Assert.ThrowsAsync<ValidationException>(() => mockService.CreateAsync(example));

        //Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task CreateInvalidFirstNameEmptyTest()
    {
        //Arrange
        var example = new ExampleContext.Domain.Entities.Example
        {
            Password = "Password@01",
            Email = "test1@test.com",
            Surname = "Surname",
            FirstName = ""
        };

        var mockExampleContext = new Mock<IExampleContext>();
        var mockContextFactory = new Mock<IDbContextFactory>();
        var mockRepository = new Mock<IExampleRepository>();
        var mockPassword = new Mock<IPasswordHasherService>();
        mockContextFactory.Setup(x => x.CreateContext()).Returns(mockExampleContext.Object);
        mockContextFactory.Setup(x => x.CreateExampleRepository(mockExampleContext.Object))
            .Returns(mockRepository.Object);
        var mockService = new ExampleService(mockContextFactory.Object, mockPassword.Object);


        //Act
        var exception = await Assert.ThrowsAsync<ValidationException>(() => mockService.CreateAsync(example));

        //Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task CreateInvalidFirstNameMaxLengthTest()
    {
        //Arrange
        var example = new ExampleContext.Domain.Entities.Example
        {
            Password = "Password@01",
            Email = "test1@test.com",
            Surname = "Surname",
            FirstName =
                "First Name First Name First Name First Name First Name First Name First Name First Name First Name First Name First Name."
        };

        var mockExampleContext = new Mock<IExampleContext>();
        var mockContextFactory = new Mock<IDbContextFactory>();
        var mockRepository = new Mock<IExampleRepository>();
        var mockPassword = new Mock<IPasswordHasherService>();
        mockContextFactory.Setup(x => x.CreateContext()).Returns(mockExampleContext.Object);
        mockContextFactory.Setup(x => x.CreateExampleRepository(mockExampleContext.Object))
            .Returns(mockRepository.Object);
        var mockService = new ExampleService(mockContextFactory.Object, mockPassword.Object);

        //Act
        var exception = await Assert.ThrowsAsync<ValidationException>(() => mockService.CreateAsync(example));

        //Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task CreateInvalidSurnameEmptyTest()
    {
        //Arrange
        var example = new ExampleContext.Domain.Entities.Example
        {
            Password = "Password@01",
            Email = "test1@test.com",
            Surname =
                "Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname",
            FirstName = "First Name"
        };

        var mockExampleContext = new Mock<IExampleContext>();
        var mockContextFactory = new Mock<IDbContextFactory>();
        var mockRepository = new Mock<IExampleRepository>();
        var mockPassword = new Mock<IPasswordHasherService>();
        mockContextFactory.Setup(x => x.CreateContext()).Returns(mockExampleContext.Object);
        mockContextFactory.Setup(x => x.CreateExampleRepository(mockExampleContext.Object))
            .Returns(mockRepository.Object);
        var mockService = new ExampleService(mockContextFactory.Object, mockPassword.Object);

        //Act
        var exception = await Assert.ThrowsAsync<ValidationException>(() => mockService.CreateAsync(example));

        //Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task CreateInvalidSurnameMaxLengthTest()
    {
        //Arrange
        var example = new ExampleContext.Domain.Entities.Example
        {
            Password = "Password@01",
            Email = "test1@test.com",
            Surname =
                "Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname",
            FirstName = "First Name"
        };

        var mockExampleContext = new Mock<IExampleContext>();
        var mockContextFactory = new Mock<IDbContextFactory>();
        var mockRepository = new Mock<IExampleRepository>();
        var mockPassword = new Mock<IPasswordHasherService>();
        mockContextFactory.Setup(x => x.CreateContext()).Returns(mockExampleContext.Object);
        mockContextFactory.Setup(x => x.CreateExampleRepository(mockExampleContext.Object))
            .Returns(mockRepository.Object);
        var mockService = new ExampleService(mockContextFactory.Object, mockPassword.Object);

        //Act
        var exception = await Assert.ThrowsAsync<ValidationException>(() => mockService.CreateAsync(example));

        //Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task CreateInvalidSurnameMinLengthTest()
    {
        //Arrange
        var example = new ExampleContext.Domain.Entities.Example
        {
            Password = "Password@01",
            Email = "test1@test.com",
            Surname = "S",
            FirstName = "First Name"
        };

        var mockExampleContext = new Mock<IExampleContext>();
        var mockContextFactory = new Mock<IDbContextFactory>();
        var mockRepository = new Mock<IExampleRepository>();
        var mockPassword = new Mock<IPasswordHasherService>();
        mockContextFactory.Setup(x => x.CreateContext()).Returns(mockExampleContext.Object);
        mockContextFactory.Setup(x => x.CreateExampleRepository(mockExampleContext.Object))
            .Returns(mockRepository.Object);
        var mockService = new ExampleService(mockContextFactory.Object, mockPassword.Object);

        //Act
        var exception = await Assert.ThrowsAsync<ValidationException>(() => mockService.CreateAsync(example));

        //Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task CreateInvalidEmailTest()
    {
        //Arrange
        var example = new ExampleContext.Domain.Entities.Example
        {
            Password = "Password@01",
            Email = "test1",
            Surname = "Surname",
            FirstName = "First Name"
        };

        var mockExampleContext = new Mock<IExampleContext>();
        var mockContextFactory = new Mock<IDbContextFactory>();
        var mockRepository = new Mock<IExampleRepository>();
        var mockPassword = new Mock<IPasswordHasherService>();

        mockContextFactory.Setup(x => x.CreateContext()).Returns(mockExampleContext.Object);
        mockContextFactory.Setup(x => x.CreateExampleRepository(mockExampleContext.Object))
            .Returns(mockRepository.Object);
        var mockService = new ExampleService(mockContextFactory.Object, mockPassword.Object);

        //Act
        var exception = await Assert.ThrowsAsync<ValidationException>(() => mockService.CreateAsync(example));

        //Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task CreateInvalidEmailEmptyTest()
    {
        //Arrange
        var example = new ExampleContext.Domain.Entities.Example
        {
            Password = "Password@01",
            Email = "",
            Surname = "Surname",
            FirstName = "First Name"
        };

        var mockExampleContext = new Mock<IExampleContext>();
        var mockContextFactory = new Mock<IDbContextFactory>();
        var mockRepository = new Mock<IExampleRepository>();
        var mockPassword = new Mock<IPasswordHasherService>();

        mockContextFactory.Setup(x => x.CreateContext()).Returns(mockExampleContext.Object);
        mockContextFactory.Setup(x => x.CreateExampleRepository(mockExampleContext.Object))
            .Returns(mockRepository.Object);
        var mockService = new ExampleService(mockContextFactory.Object, mockPassword.Object);

        //Act
        var exception = await Assert.ThrowsAsync<ValidationException>(() => mockService.CreateAsync(example));

        //Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task CreateInvalidPasswordEmptyTest()
    {
        //Arrange
        var example = new ExampleContext.Domain.Entities.Example
        {
            Password = "",
            Email = "test1@test.com",
            Surname =
                "Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname",
            FirstName = "First Name"
        };

        var mockExampleContext = new Mock<IExampleContext>();
        var mockContextFactory = new Mock<IDbContextFactory>();
        var mockRepository = new Mock<IExampleRepository>();
        var mockPassword = new Mock<IPasswordHasherService>();

        mockContextFactory.Setup(x => x.CreateContext()).Returns(mockExampleContext.Object);
        mockContextFactory.Setup(x => x.CreateExampleRepository(mockExampleContext.Object))
            .Returns(mockRepository.Object);
        var mockService = new ExampleService(mockContextFactory.Object, mockPassword.Object);

        //Act
        var exception = await Assert.ThrowsAsync<ValidationException>(() => mockService.CreateAsync(example));

        //Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task CreateInvalidPasswordMaxLengthTest()
    {
        //Arrange
        var example = new ExampleContext.Domain.Entities.Example
        {
            Password = "Password@0122222222222222222",
            Email = "test1@test.com",
            Surname = "Surname",
            FirstName = "First Name"
        };

        var mockExampleContext = new Mock<IExampleContext>();
        var mockContextFactory = new Mock<IDbContextFactory>();
        var mockRepository = new Mock<IExampleRepository>();
        var mockPassword = new Mock<IPasswordHasherService>();

        mockContextFactory.Setup(x => x.CreateContext()).Returns(mockExampleContext.Object);
        mockContextFactory.Setup(x => x.CreateExampleRepository(mockExampleContext.Object))
            .Returns(mockRepository.Object);
        var mockService = new ExampleService(mockContextFactory.Object, mockPassword.Object);

        //Act
        var exception = await Assert.ThrowsAsync<ValidationException>(() => mockService.CreateAsync(example));

        //Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task CreateInvalidPasswordMinLengthTest()
    {
        //Arrange
        var example = new ExampleContext.Domain.Entities.Example
        {
            Password = "P@01",
            Email = "test1@test.com",
            Surname = "Surname",
            FirstName = "First Name"
        };

        var mockExampleContext = new Mock<IExampleContext>();
        var mockContextFactory = new Mock<IDbContextFactory>();
        var mockRepository = new Mock<IExampleRepository>();
        var mockPassword = new Mock<IPasswordHasherService>();

        mockContextFactory.Setup(x => x.CreateContext()).Returns(mockExampleContext.Object);
        mockContextFactory.Setup(x => x.CreateExampleRepository(mockExampleContext.Object))
            .Returns(mockRepository.Object);
        var mockService = new ExampleService(mockContextFactory.Object, mockPassword.Object);

        //Act
        var exception = await Assert.ThrowsAsync<ValidationException>(() => mockService.CreateAsync(example));

        //Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task UpdateInvalidIdTest()
    {
        //Arrange
        const long id = 0;
        var example = new ExampleContext.Domain.Entities.Example
        {
            Password = "P@01",
            Email = "test1@test.com",
            Surname = "Surname",
            FirstName = "First Name"
        };

        var mockExampleContext = new Mock<IExampleContext>();
        var mockContextFactory = new Mock<IDbContextFactory>();
        var mockRepository = new Mock<IExampleRepository>();
        var mockPassword = new Mock<IPasswordHasherService>();

        mockContextFactory.Setup(x => x.CreateContext()).Returns(mockExampleContext.Object);
        mockContextFactory.Setup(x => x.CreateExampleRepository(mockExampleContext.Object))
            .Returns(mockRepository.Object);
        var mockService = new ExampleService(mockContextFactory.Object, mockPassword.Object);

        //Act
        var exception = await Assert.ThrowsAsync<ValidationException>(() => mockService.UpdateAsync(id, example));

        //Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task UpdateInvalidExampleTest()
    {
        //Arrange
        const long id = 1;

        var mockExampleContext = new Mock<IExampleContext>();
        var mockContextFactory = new Mock<IDbContextFactory>();
        var mockRepository = new Mock<IExampleRepository>();
        var mockPassword = new Mock<IPasswordHasherService>();

        mockContextFactory.Setup(x => x.CreateContext()).Returns(mockExampleContext.Object);
        mockContextFactory.Setup(x => x.CreateExampleRepository(mockExampleContext.Object))
            .Returns(mockRepository.Object);
        var mockService = new ExampleService(mockContextFactory.Object, mockPassword.Object);

        //Act
        var exception = await Assert.ThrowsAsync<ValidationException>(() => mockService.UpdateAsync(id, null));

        //Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task UpdateInvalidExampleNotFoundTest()
    {
        //Arrange
        const long id = 1;

        var example = new ExampleContext.Domain.Entities.Example
        {
            Id = 1,
            Password = "P@013333343",
            Email = "test1@test.com",
            Surname = "Surname",
            FirstName = "First Name"
        };

        var mockExampleContext = new Mock<IExampleContext>();
        var mockContextFactory = new Mock<IDbContextFactory>();
        var mockRepository = new Mock<IExampleRepository>();
        var mockPassword = new Mock<IPasswordHasherService>();

        mockContextFactory.Setup(x => x.CreateContext()).Returns(mockExampleContext.Object);
        mockContextFactory.Setup(x => x.CreateExampleRepository(mockExampleContext.Object))
            .Returns(mockRepository.Object);
        var mockService = new ExampleService(mockContextFactory.Object, mockPassword.Object);

        //Act
        var exception =
            await Assert.ThrowsAsync<EntityNotFoundException>(() => mockService.UpdateAsync(id, example));

        //Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task DeleteInvalidIdTest()
    {
        //Arrange
        const long id = 0;

        var mockExampleContext = new Mock<IExampleContext>();
        var mockContextFactory = new Mock<IDbContextFactory>();
        var mockRepository = new Mock<IExampleRepository>();
        var mockPassword = new Mock<IPasswordHasherService>();

        mockContextFactory.Setup(x => x.CreateContext()).Returns(mockExampleContext.Object);
        mockContextFactory.Setup(x => x.CreateExampleRepository(mockExampleContext.Object))
            .Returns(mockRepository.Object);
        var mockService = new ExampleService(mockContextFactory.Object, mockPassword.Object);

        //Act
        var exception = await Assert.ThrowsAsync<ValidationException>(() => mockService.DeleteAsync(id));

        //Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task DeleteInvalidNotFoundTest()
    {
        //Arrange
        const long id = 1;

        var mockExampleContext = new Mock<IExampleContext>();
        var mockContextFactory = new Mock<IDbContextFactory>();
        var mockRepository = new Mock<IExampleRepository>();
        var mockPassword = new Mock<IPasswordHasherService>();

        mockContextFactory.Setup(x => x.CreateContext()).Returns(mockExampleContext.Object);
        mockContextFactory.Setup(x => x.CreateExampleRepository(mockExampleContext.Object))
            .Returns(mockRepository.Object);
        var mockService = new ExampleService(mockContextFactory.Object, mockPassword.Object);

        //Act
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => mockService.DeleteAsync(id));

        //Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task IsAvailableEmailTest()
    {
        //Arrange
        var example = new ExampleContext.Domain.Entities.Example
        {
            Id = 1,
            Password = "P@013333343",
            Email = "test1@test.com",
            Surname = "Surname",
            FirstName = "First Name"
        };

        var mockExampleContext = new Mock<IExampleContext>();
        var mockContextFactory = new Mock<IDbContextFactory>();
        var mockRepository = new Mock<IExampleRepository>();
        var mockPassword = new Mock<IPasswordHasherService>();

        mockContextFactory.Setup(x => x.CreateContext()).Returns(mockExampleContext.Object);
        mockContextFactory.Setup(x => x.CreateExampleRepository(mockExampleContext.Object))
            .Returns(mockRepository.Object);

        var filter = new ExampleFilter { Email = example.Email };
        mockRepository.Setup(x => x.GetByFilterAsync(filter)).ReturnsAsync(example);
        var mockService = new ExampleService(mockContextFactory.Object, mockPassword.Object);

        //Act
        var existingEmail = await mockService.IsAvailableEmail(example.Email);

        //Assert
        Assert.True(existingEmail);
    }
}