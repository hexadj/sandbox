using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public sealed class CounterService(AppDbContext dbContext)
{
    public async Task<int> GetValueAsync(CancellationToken cancellationToken)
    {
        var state = await GetCounterStateAsync(cancellationToken);
        return state.Value;
    }

    public async Task<int> IncrementAsync(CancellationToken cancellationToken)
    {
        var state = await GetCounterStateAsync(cancellationToken);
        state.Value += 1;
        state.UpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        return state.Value;
    }

    public static async Task EnsureSeedAsync(AppDbContext dbContext)
    {
        var state = await dbContext.CounterStates.FirstOrDefaultAsync(x => x.Id == 1);
        if (state is not null)
        {
            return;
        }

        dbContext.CounterStates.Add(new CounterState
        {
            Id = 1,
            Value = 0,
            UpdatedAt = DateTime.UtcNow
        });
        await dbContext.SaveChangesAsync();
    }

    private async Task<CounterState> GetCounterStateAsync(CancellationToken cancellationToken)
    {
        var state = await dbContext.CounterStates.FirstOrDefaultAsync(x => x.Id == 1, cancellationToken);
        if (state is not null)
        {
            return state;
        }

        state = new CounterState
        {
            Id = 1,
            Value = 0,
            UpdatedAt = DateTime.UtcNow
        };

        dbContext.CounterStates.Add(state);
        await dbContext.SaveChangesAsync(cancellationToken);
        return state;
    }
}
