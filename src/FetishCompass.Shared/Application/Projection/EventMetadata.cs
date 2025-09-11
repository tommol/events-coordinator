namespace FetishCompass.Shared.Application.Projection;

public sealed record EventMetadata(
    string StreamId,         
    ulong StreamPosition,     
    ulong GlobalPosition,    
    DateTimeOffset CreatedUtc,
    string EventType,         
    int SchemaVersion         
);