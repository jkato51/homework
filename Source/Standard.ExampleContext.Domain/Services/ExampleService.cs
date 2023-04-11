using Standard.ExampleContext.Domain.DbContext;
using Standard.ExampleContext.Domain.Entities;
using Standard.ExampleContext.Domain.Exceptions;
using Standard.ExampleContext.Domain.Models;
using Standard.ExampleContext.Domain.Services.Interfaces;

namespace Standard.ExampleContext.Domain.Services;

public class ExampleService : IExampleService
{
    private readonly IDbContextFactory _dbContextFactory;
    private readonly IPasswordHasherService _passwordHasher;

    public ExampleService(IDbContextFactory dbContextFactory, IPasswordHasherService passwordHasher)
    {
        _passwordHasher = passwordHasher;
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Pagination<Example>> GetListByFilterAsync(ExampleFilter filter)
    {
        using var context = _dbContextFactory.CreateContext();
        var exampleRepository = _dbContextFactory.CreateExampleRepository(context);

        if (filter == null)
            throw new ValidationException("Filter is null.");

        if (filter.PageSize > 100)
            throw new ValidationException("Maximum allowed page size is 100.");

        if (filter.CurrentPage <= 0) filter.PageSize = 1;

        var total = await exampleRepository.CountByFilterAsync(filter);

        if (total == 0) return new Pagination<Example>();

        var paginateResult = await exampleRepository.GetListByFilterAsync(filter);

        var result = new Pagination<Example>
        {
            Count = total,
            CurrentPage = filter.CurrentPage,
            PageSize = filter.PageSize,
            Result = paginateResult.ToList()
        };

        return result;
    }

    public async Task<Example> GetByFilterAsync(ExampleFilter filter)
    {
        if (filter == null)
            throw new ValidationException("Filter is null.");

        using var context = _dbContextFactory.CreateContext();
        var exampleRepository = _dbContextFactory.CreateExampleRepository(context);

        return await exampleRepository.GetByFilterAsync(filter);
    }

    public async Task UpdateAsync(long id, Example example)
    {
        if (id <= 0) throw new ValidationException("Id is invalid.");

        if (example == null)
            throw new ValidationException("Example is null.");

        using var context = _dbContextFactory.CreateContext();
        var exampleRepository = _dbContextFactory.CreateExampleRepository(context);

        var entity = await exampleRepository.GetByIdAsync(id);

        if (entity == null)
            throw new EntityNotFoundException(id);

        Validate(example);

        if (entity.Email != example.Email && !await IsAvailableEmail(example.Email))
            throw new ValidationException("Email is not available.");

        entity.Email = example.Email;
        entity.FirstName = example.FirstName;
        entity.Surname = example.Surname;

        var password = _passwordHasher.Check(entity.Password, example.Password);
        if (!password.Verified) entity.Password = _passwordHasher.Hash(example.Password);

        entity.SetUpdatedDate();
        await context.SaveChangesAsync(CancellationToken.None);
    }

    public async Task<long> CreateAsync(Example example)
    {
        if (example == null)
            throw new ValidationException("Example is null.");

        Validate(example);

        using var context = _dbContextFactory.CreateContext();
        var exampleRepository = _dbContextFactory.CreateExampleRepository(context);

        var isAvailableEmail = await IsAvailableEmail(example.Email);
        if (!isAvailableEmail) throw new ValidationException("Email is not available.");

        example.Password = _passwordHasher.Hash(example.Password);
        example.SetCreatedDate();

        exampleRepository.Add(example);
        await context.SaveChangesAsync(CancellationToken.None);

        return example.Id;
    }

    public async Task DeleteAsync(long id)
    {
        if (id <= 0) throw new ValidationException("Id is invalid.");

        using var context = _dbContextFactory.CreateContext();
        var exampleRepository = _dbContextFactory.CreateExampleRepository(context);

        var entity = await exampleRepository.GetByIdAsync(id);

        if (entity == null) throw new EntityNotFoundException(id);

        exampleRepository.Remove(entity);

        await context.SaveChangesAsync(CancellationToken.None);
    }

    public async Task<bool> IsAvailableEmail(string email)
    {
        using var context = _dbContextFactory.CreateContext();
        var exampleRepository = _dbContextFactory.CreateExampleRepository(context);
        var existingEmail = await exampleRepository.GetByFilterAsync(new ExampleFilter { Email = email });
        return existingEmail == null;
    }

    private static void Validate(Example example)
    {
        example.ValidateFistName();

        example.ValidateSurname();

        example.ValidateEmail();

        example.ValidatePassword();
    }
}