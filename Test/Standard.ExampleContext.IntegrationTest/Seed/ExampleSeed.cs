namespace Standard.ExampleContext.IntegrationTest.Seed;

public class ExampleSeed
{
    public static async Task Populate(Infrastructure.DbContext.ExampleContext dbContext)
    {
        await dbContext.Examples.AddAsync(new Domain.Entities.Example
        {
            Email = "seed.record@test.com",
            Password = "Rgrtgr#$543gfregeg",
            FirstName = "Seed",
            Surname = "Seed",
            Created = DateTime.Now,
            Updated = DateTime.Now
        });

        await dbContext.SaveChangesAsync();
    }
}