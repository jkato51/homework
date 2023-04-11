using Microsoft.EntityFrameworkCore;
using Standard.ExampleContext.Domain.Entities;
using Standard.ExampleContext.Domain.Models;
using Standard.ExampleContext.Domain.Repositories;

namespace Standard.ExampleContext.Infrastructure.Repositories;

public class ExampleRepository : RepositoryBase<Example>, IExampleRepository
{
    public ExampleRepository(DbContext.ExampleContext context) : base(context)
    {
    }

    public async Task<int> CountByFilterAsync(ExampleFilter filter)
    {
        var query = DbContext.Examples.AsQueryable();

        query = ApplyFilter(filter, query);

        return await query.CountAsync();
    }

    public async Task<Example> GetByFilterAsync(ExampleFilter filter)
    {
        var query = DbContext.Examples.AsQueryable();

        query = ApplyFilter(filter, query);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<List<Example>> GetListByFilterAsync(ExampleFilter filter)
    {
        var query = DbContext.Examples.AsQueryable();

        query = ApplyFilter(filter, query);

        query = ApplySorting(filter, query);

        if (filter.CurrentPage > 0)
            query = query.Skip((filter.CurrentPage - 1) * filter.PageSize).Take(filter.PageSize);

        return await query.ToListAsync();
    }

    private static IQueryable<Example> ApplySorting(ExampleFilter filter, IQueryable<Example> query)
    {
        if (filter?.OrderBy.ToLower() == "firstname")
            query = filter.SortBy.ToLower() == "asc"
                ? query.OrderBy(x => x.FirstName)
                : query.OrderByDescending(x => x.FirstName);

        if (filter?.OrderBy.ToLower() == "surname")
            query = filter.SortBy.ToLower() == "asc"
                ? query.OrderBy(x => x.Surname)
                : query.OrderByDescending(x => x.Surname);

        if (filter?.OrderBy.ToLower() == "email")
            query = filter.SortBy.ToLower() == "asc"
                ? query.OrderBy(x => x.Email)
                : query.OrderByDescending(x => x.Email);

        return query;
    }

    private static IQueryable<Example> ApplyFilter(ExampleFilter filter, IQueryable<Example> query)
    {
        if (filter.Id > 0)
            query = query.Where(x => x.Id == filter.Id);

        if (!string.IsNullOrWhiteSpace(filter.FirstName))
            query = query.Where(x => x.FirstName.Contains(filter.FirstName));

        if (!string.IsNullOrWhiteSpace(filter.Surname))
            query = query.Where(x => x.Surname.Contains(filter.Surname));

        if (!string.IsNullOrWhiteSpace(filter.Email))
            query = query.Where(x => x.Email.Contains(filter.Email));

        return query;
    }
}