using System.Net.Http.Headers;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Standard.ExampleContext.Application.Dtos;
using Standard.Api;
using Standard.ExampleContext.IntegrationTest;
using Xunit;

namespace Standard.ExampleContext.IntegrationTest.Example;

public class ExampleIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    private const string ExampleUrl = "api/v1/example";
    private readonly CustomWebApplicationFactory _factory;

    public ExampleIntegrationTest(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateExampleOkTest()
    {
        var dto = new ExampleRequestDto
        {
            FirstName = "Test Name",
            Surname = "Test Surname",
            Email = CreateValidEmail(),
            Password = "Password1@",
            ConfirmPassword = "Password1@"
        };

        var content = await CreateStringContent(dto);
        var client = _factory.CreateClient();
        var response = await client.PostAsync(ExampleUrl, content);
        response.EnsureSuccessStatusCode();

        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CreateExampleInvalidEmailTest()
    {
        var dto = new ExampleRequestDto
        {
            FirstName = "Test Name",
            Surname = "Test Surname",
            Email = CreateInvalidEmail(),
            Password = "Password1@"
        };

        var content = await CreateStringContent(dto);
        var client = _factory.CreateClient();
        var response = await client.PostAsync(ExampleUrl, content);

        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CreateExampleExistingEmailTest()
    {
        var email = CreateValidEmail();

        var dto = new ExampleRequestDto
        {
            FirstName = "Test Name",
            Surname = "Test Surname",
            Email = email,
            Password = "Password1@",
            ConfirmPassword = "Password1@"
        };

        var content = await CreateStringContent(dto);
        var client = _factory.CreateClient();
        var response = await client.PostAsync(ExampleUrl, content);
        Assert.True(response.IsSuccessStatusCode);

        var clientNotOk = _factory.CreateClient();
        var responseNotOk = await clientNotOk.PostAsync(ExampleUrl, content);
        Assert.False(responseNotOk.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CreateExampleRequiredEmailTest()
    {
        var dto = new ExampleRequestDto
        {
            FirstName = "Test Name",
            Surname = "Test Surname",
            Email = "",
            Password = "Password1@",
            ConfirmPassword = "Password1@"
        };

        var content = await CreateStringContent(dto);
        var client = _factory.CreateClient();
        var response = await client.PostAsync(ExampleUrl, content);

        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CreateExampleRequiredFirstNameTest()
    {
        var dto = new ExampleRequestDto
        {
            FirstName = "",
            Surname = "Test Surname",
            Email = CreateValidEmail(),
            Password = "Password1@",
            ConfirmPassword = "Password1@"
        };

        var content = await CreateStringContent(dto);
        var client = _factory.CreateClient();
        var response = await client.PostAsync(ExampleUrl, content);

        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CreateExampleRequiredSurnameTest()
    {
        var dto = new ExampleRequestDto
        {
            FirstName = "Test First Name",
            Surname = "",
            Email = CreateValidEmail(),
            Password = "Password1@",
            ConfirmPassword = "Password1@"
        };

        var content = await CreateStringContent(dto);
        var client = _factory.CreateClient();
        var response = await client.PostAsync(ExampleUrl, content);

        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CreateExampleMaxLengthFirstNameTest()
    {
        var dto = new ExampleRequestDto
        {
            FirstName =
                "Test First Name Test First Name Test First Name Test First Name Test First Name Test First Name Test First Name",
            Surname = "Test Surname",
            Email = CreateValidEmail(),
            Password = "Password1@",
            ConfirmPassword = "Password1@"
        };

        var content = await CreateStringContent(dto);
        var client = _factory.CreateClient();
        var response = await client.PostAsync(ExampleUrl, content);

        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CreateExampleMinLengthFirstNameTest()
    {
        var dto = new ExampleRequestDto
        {
            FirstName = "T",
            Surname = "Test Surname",
            Email = CreateValidEmail(),
            Password = "Password1@",
            ConfirmPassword = "Password1@"
        };

        var content = await CreateStringContent(dto);
        var client = _factory.CreateClient();
        var response = await client.PostAsync(ExampleUrl, content);

        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CreateExampleMaxLengthSurnameTest()
    {
        var dto = new ExampleRequestDto
        {
            FirstName = "Test First",
            Surname =
                "Test Surname Test Surname Test Surname Test Surname Test Surname Test Surname Test Surname Test Surname Test Surname",
            Email = CreateValidEmail(),
            Password = "Password1@",
            ConfirmPassword = "Password1@"
        };

        var content = await CreateStringContent(dto);
        var client = _factory.CreateClient();
        var response = await client.PostAsync(ExampleUrl, content);

        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CreateExampleMinLengthSurnameTest()
    {
        var dto = new ExampleRequestDto
        {
            FirstName = "Test First",
            Surname = "T",
            Email = CreateValidEmail(),
            Password = "Password1@",
            ConfirmPassword = "Password1@"
        };

        var content = await CreateStringContent(dto);
        var client = _factory.CreateClient();
        var response = await client.PostAsync(ExampleUrl, content);

        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CreateRequiredPasswordTest()
    {
        var dto = new ExampleRequestDto
        {
            FirstName = "Test First",
            Surname = "Test Surname",
            Email = CreateValidEmail(),
            Password = ""
        };

        var content = await CreateStringContent(dto);
        var client = _factory.CreateClient();
        var response = await client.PostAsync(ExampleUrl, content);

        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CreatePasswordsDoesNotMatchTest()
    {
        var dto = new ExampleRequestDto
        {
            FirstName = "Test First",
            Surname = "Test Surname",
            Email = CreateValidEmail(),
            Password = "Password1@",
            ConfirmPassword = "Password3!"
        };

        var content = await CreateStringContent(dto);
        var client = _factory.CreateClient();
        var response = await client.PostAsync(ExampleUrl, content);

        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CreateMinLengthPasswordTest()
    {
        var dto = new ExampleRequestDto
        {
            FirstName = "Test First",
            Surname = "Test Surname",
            Email = CreateValidEmail(),
            Password = "@123RF",
            ConfirmPassword = "@123RF"
        };

        var content = await CreateStringContent(dto);
        var client = _factory.CreateClient();
        var response = await client.PostAsync(ExampleUrl, content);

        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CreateMaxLengthPasswordTest()
    {
        var dto = new ExampleRequestDto
        {
            FirstName = "Test First",
            Surname = "Test Surname",
            Email = CreateValidEmail(),
            Password = "01234567901234567901234@Df",
            ConfirmPassword = "01234567901234567901234@Df"
        };

        var content = await CreateStringContent(dto);
        var client = _factory.CreateClient();
        var response = await client.PostAsync(ExampleUrl, content);

        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CreateInvalidPasswordTest()
    {
        var dto = new ExampleRequestDto
        {
            FirstName = "Test First",
            Surname = "Test Surname",
            Email = CreateValidEmail(),
            Password = "01234567901234",
            ConfirmPassword = "01234567901234"
        };

        var content = await CreateStringContent(dto);
        var client = _factory.CreateClient();
        var response = await client.PostAsync(ExampleUrl, content);

        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task UpdateExampleOkTest()
    {
        var email = CreateValidEmail();

        var dto = new ExampleRequestDto
        {
            FirstName = "Test Name",
            Surname = "Test Surname",
            Email = email,
            Password = "Password1@",
            ConfirmPassword = "Password1@"
        };

        var content = await CreateStringContent(dto);
        var client = _factory.CreateClient();
        var createResponse = await client.PostAsync(ExampleUrl, content);
        Assert.True(createResponse.IsSuccessStatusCode);

        client = _factory.CreateClient();
        var getResponse = await client.GetAsync(createResponse.Headers.Location.ToString());
        var example =
            JsonConvert.DeserializeObject<ExampleResponseDto>(await getResponse.Content.ReadAsStringAsync());

        dto.FirstName = "New Name";
        var contentUpdate = await CreateStringContent(dto);
        var putResponse = await client.PutAsync($"{ExampleUrl}/{example.Id}", contentUpdate);
        Assert.True(putResponse.IsSuccessStatusCode);
    }

    [Fact]
    public async Task UpdateExampleExistingEmailTest()
    {
        var exampleOneEmail = CreateValidEmail();

        var dto = new ExampleRequestDto
        {
            FirstName = "Test Name",
            Surname = "Test Surname",
            Email = exampleOneEmail,
            Password = "Password1@",
            ConfirmPassword = "Password1@"
        };

        var contentExampleOne = await CreateStringContent(dto);
        var client = _factory.CreateClient();
        var createExampleOneResponse = await client.PostAsync(ExampleUrl, contentExampleOne);
        Assert.True(createExampleOneResponse.IsSuccessStatusCode);

        dto.Email = CreateValidEmail();

        var contentExampleTwo = await CreateStringContent(dto);
        client = _factory.CreateClient();
        var createExampleTwoResponse = await client.PostAsync(ExampleUrl, contentExampleTwo);
        Assert.True(createExampleTwoResponse.IsSuccessStatusCode);

        var parameters = new Dictionary<string, string> { { "email", dto.Email } };
        var requestUri = QueryHelpers.AddQueryString(ExampleUrl, parameters);
        client = _factory.CreateClient();
        var getResponse = await client.GetAsync(requestUri);
        Assert.True(getResponse.IsSuccessStatusCode);
        var example =
            JsonConvert.DeserializeObject<ExampleResponseDto>(await getResponse.Content.ReadAsStringAsync());

        dto.Email = exampleOneEmail;
        var content = await CreateStringContent(dto);
        client = _factory.CreateClient();
        var response = await client.PutAsync($"{ExampleUrl}/{example.Id}", content);
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task GetExampleTest()
    {
        var exampleOneEmail = CreateValidEmail();

        var dto = new ExampleRequestDto
        {
            FirstName = "Test Name",
            Surname = "Test Surname",
            Email = exampleOneEmail,
            Password = "Password1@",
            ConfirmPassword = "Password1@"
        };

        var content = await CreateStringContent(dto);
        var client = _factory.CreateClient();
        var response = await client.PostAsync(ExampleUrl, content);
        Assert.True(response.IsSuccessStatusCode);

        client = _factory.CreateClient();
        var getResponse = await client.GetAsync(response.Headers.Location.ToString());
        Assert.True(getResponse.IsSuccessStatusCode);
    }

    [Fact]
    public async Task GetExampleListTest()
    {
        var dto = new ExampleRequestDto
        {
            FirstName = "Test Name Get",
            Surname = "Test Surname",
            Email = CreateValidEmail(),
            Password = "Password1@",
            ConfirmPassword = "Password1@"
        };

        var content = await CreateStringContent(dto);
        var client = _factory.CreateClient();
        var response = await client.PostAsync(ExampleUrl, content);
        Assert.True(response.IsSuccessStatusCode);

        dto.Email = CreateValidEmail();
        var contentTwo = await CreateStringContent(dto);
        client = _factory.CreateClient();
        var responseTwo = await client.PostAsync(ExampleUrl, contentTwo);
        Assert.True(responseTwo.IsSuccessStatusCode);

        var parameters = new Dictionary<string, string>
        {
            { "currentPage", "1" },
            { "pageSize", "1" },
            { "orderBy", dto.FirstName },
            { "sortBy", "asc" }
        };

        var requestUri = QueryHelpers.AddQueryString(ExampleUrl, parameters);

        client = _factory.CreateClient();
        var getResponse = await client.GetAsync(requestUri);
        Assert.True(getResponse.IsSuccessStatusCode);
        var examples =
            JsonConvert.DeserializeObject<PaginationDto<ExampleResponseDto>>(
                await getResponse.Content.ReadAsStringAsync());
        Assert.True(examples.Count > 1);
    }

    [Fact]
    public async Task DeleteExampleOkTest()
    {
        var email = CreateValidEmail();

        var dto = new ExampleRequestDto
        {
            FirstName = "Test Name",
            Surname = "Test Surname",
            Email = email,
            Password = "Password1@",
            ConfirmPassword = "Password1@"
        };

        var content = await CreateStringContent(dto);

        var client = _factory.CreateClient();
        var response = await client.PostAsync(ExampleUrl, content);
        Assert.True(response.IsSuccessStatusCode);

        client = _factory.CreateClient();
        var getResponse = await client.GetAsync(response.Headers.Location.ToString());
        Assert.True(getResponse.IsSuccessStatusCode);
        var example =
            JsonConvert.DeserializeObject<ExampleResponseDto>(await getResponse.Content.ReadAsStringAsync());

        client = _factory.CreateClient();
        var deleteResponse = await client.DeleteAsync($"{ExampleUrl}/{example.Id}");
        Assert.True(deleteResponse.IsSuccessStatusCode);
    }

    private static async Task<StringContent> CreateStringContent(ExampleRequestDto dto)
    {
        var content = new StringContent(await Task.Factory.StartNew(() => JsonConvert.SerializeObject(dto)));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        return content;
    }

    private static string CreateValidEmail()
    {
        return $"{DateTime.Now:yyyyMMdd_hhmmssfff}@test.com";
    }

    private static string CreateInvalidEmail()
    {
        return $"{DateTime.Now:yyyyMMdd_hhmmssfff}attest.com";
    }
}