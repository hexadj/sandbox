namespace Backend.Models;

public sealed class CounterState
{
    public int Id { get; set; }
    public int Value { get; set; }
    public DateTime UpdatedAt { get; set; }
}
