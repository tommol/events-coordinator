namespace FetishCompass.Domain.Moderation.Model;

public record ReviewNotes(string Notes, DateTimeOffset CreatedAt, bool Automated);