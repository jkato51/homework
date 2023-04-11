using AutoMapper;
using Moq;
using Standard.ExampleContext.Application.Dtos;
using Standard.ExampleContext.Application.Facades;
using Standard.ExampleContext.Domain.Models;
using Standard.ExampleContext.Domain.Services.Interfaces;
using Xunit;
using Xunit.Sdk;

namespace Standard.ExampleContext.UnitTest.Example.Facade;

public class ExampleFacadeTest
{
    [Fact]
    public async Task CreateTest()
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

        var exampleRequestDto = new ExampleRequestDto
        {
            Email = "test1@test.com",
            Surname = "Surname",
            FirstName = "First Name",
            Password = "P@013333343",
            ConfirmPassword = "P@013333343"
        };

        const long id = 1;
        var mockService = new Mock<IExampleService>();
        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(x => x.Map<ExampleContext.Domain.Entities.Example>(exampleRequestDto)).Returns(example);
        mockService.Setup(x => x.CreateAsync(example)).ReturnsAsync(id);
        var mockFacade = new ExampleFacade(mockService.Object, mockMapper.Object);

        //Act
        var result = await mockFacade.CreateAsync(exampleRequestDto);

        //Assert
        Assert.Equal(id, result);
    }

    [Fact]
    public async Task GetTest()
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

        var exampleResponseDto = new ExampleResponseDto
        {
            Id = 1,
            Email = "test1@test.com",
            Surname = "Surname",
            FirstName = "First Name",
            FullName = "First Name Surname"
        };

        var filterDto = new ExampleFilterDto { Id = 1 };
        var filter = new ExampleFilter { Id = 1 };

        //Act
        var mockService = new Mock<IExampleService>();
        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(x => x.Map<ExampleResponseDto>(example)).Returns(exampleResponseDto);
        mockMapper.Setup(x => x.Map<ExampleFilter>(filterDto)).Returns(filter);
        mockService.Setup(x => x.GetByFilterAsync(filter)).ReturnsAsync(example);
        var mockFacade = new ExampleFacade(mockService.Object, mockMapper.Object);

        //Act
        var result = await mockFacade.GetByFilterAsync(filterDto);

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
            Password = "sdfdsfdsfds",
            Email = "test@test.com",
            Surname = "Surname",
            FirstName = "Ronaldo",
            Updated = DateTime.Now,
            Created = DateTime.Now
        };

        var exampleTwo = new ExampleContext.Domain.Entities.Example
        {
            Id = 2,
            Password = "sdfdsfdsfds",
            Email = "test@test.com",
            Surname = "Surname",
            FirstName = "Ronaldinho",
            Updated = DateTime.Now,
            Created = DateTime.Now
        };

        var examples = new List<ExampleContext.Domain.Entities.Example>
        {
            exampleOne,
            exampleTwo
        };

        var pagination = new Pagination<ExampleContext.Domain.Entities.Example>
        {
            PageSize = 10,
            CurrentPage = 1,
            Count = 2,
            Result = examples
        };

        var exampleOneDto = new ExampleResponseDto
        {
            Id = 1,
            Email = "test@test.com",
            Surname = "Surname",
            FirstName = "Ronaldo"
        };

        var exampleTwoDto = new ExampleResponseDto
        {
            Id = 2,
            Email = "test@test.com",
            Surname = "Surname",
            FirstName = "Ronaldinho"
        };

        var examplesDto = new List<ExampleResponseDto>
        {
            exampleOneDto,
            exampleTwoDto
        };

        var paginationDto = new PaginationDto<ExampleResponseDto>
        {
            PageSize = 10,
            CurrentPage = 1,
            Count = 2,
            TotalPages = 1,
            Result = examplesDto
        };

        var filterDto = new ExampleFilterDto { Id = 1 };
        var filter = new ExampleFilter { Id = 1 };

        //Act
        var mockService = new Mock<IExampleService>();
        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(x => x.Map<ExampleFilter>(filterDto)).Returns(filter);
        mockMapper.Setup(x => x.Map<PaginationDto<ExampleResponseDto>>(pagination)).Returns(paginationDto);
        mockService.Setup(x => x.GetListByFilterAsync(filter)).ReturnsAsync(pagination);
        var mockFacade = new ExampleFacade(mockService.Object, mockMapper.Object);

        //Act
        var result = await mockFacade.GetListByFilterAsync(filterDto);

        //Assert
        Assert.Equal(paginationDto.Count, result.Count);
    }

    [Fact]
    public async Task UpdateTest()
    {
        //Arrange
        var exampleRequestDto = new ExampleRequestDto
        {
            Password = "Passrrr@1",
            ConfirmPassword = "Passrrr@1",
            Email = "test@test.com",
            Surname = "Surname",
            FirstName = "First Name"
        };

        var example = new ExampleContext.Domain.Entities.Example
        {
            Password = "Passrrr@1",
            Email = "test@test.com",
            Surname = "Surname",
            FirstName = "First Name"
        };

        const long id = 1;
        var mockService = new Mock<IExampleService>();
        var mockMapper = new Mock<IMapper>();

        mockMapper.Setup(x => x.Map<ExampleContext.Domain.Entities.Example>(exampleRequestDto)).Returns(example);
        var mockFacade = new ExampleFacade(mockService.Object, mockMapper.Object);

        //Act
        //Assert
        try
        {
            await Assert.ThrowsAsync<Exception>(() => mockFacade.UpdateAsync(id, exampleRequestDto));
        }
        catch (AssertActualExpectedException exception)
        {
            Assert.Equal("(No exception was thrown)", exception.Actual);
        }
    }

    [Fact]
    public async Task DeleteTest()
    {
        //Arrange
        const long id = 1;

        var mockService = new Mock<IExampleService>();
        var mockMapper = new Mock<IMapper>();

        var mockFacade = new ExampleFacade(mockService.Object, mockMapper.Object);

        //Act
        //Assert
        try
        {
            await Assert.ThrowsAsync<Exception>(() => mockFacade.DeleteAsync(id));
        }
        catch (AssertActualExpectedException exception)
        {
            Assert.Equal("(No exception was thrown)", exception.Actual);
        }
    }
}