using Backend.Data;
using Backend.Services;
using Microsoft.EntityFrameworkCore;

namespace Backend.Tests;

public sealed class CounterServiceTests
{
    [Fact]
    public async Task IncrementAsync_IncrementsAndPersistsCounter()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var dbContext = new AppDbContext(options);
        await CounterService.EnsureSeedAsync(dbContext);
        var service = new CounterService(dbContext);

        var initial = await service.GetValueAsync(CancellationToken.None);
        var incremented = await service.IncrementAsync(CancellationToken.None);
        var reloaded = await service.GetValueAsync(CancellationToken.None);

        Assert.Equal(0, initial);
        Assert.Equal(1, incremented);
        Assert.Equal(1, reloaded);
    }
}
