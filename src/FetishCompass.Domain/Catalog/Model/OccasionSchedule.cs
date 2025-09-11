namespace FetishCompass.Domain.Catalog.Model;

public sealed class OccasionSchedule
{
    public LocalDateTime Start { get; }
    public LocalDateTime End { get; }

    private OccasionSchedule(LocalDateTime start, LocalDateTime end)
    {
        this.Start = start;
        this.End = end;

        // Walidacja: Start < End w sensie absolutnym (UTC)
        var startUtc = this.Start.ToUtcInstant();
        var endUtc = this.End.ToUtcInstant();
        if (startUtc >= endUtc)
            throw new InvalidOperationException("Event end must be after start.");
    }

    public static OccasionSchedule Create(LocalDateTime start, LocalDateTime end)
        => new(start, end);

    public (DateTimeOffset StartUtc, DateTimeOffset EndUtc) ToUtcRange()
        => (this.Start.ToUtcInstant(), this.End.ToUtcInstant());
}