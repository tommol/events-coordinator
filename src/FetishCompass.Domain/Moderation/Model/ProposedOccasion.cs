using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Domain.IAM;
using FetishCompass.Shared.Domain;

namespace FetishCompass.Domain.Moderation.Model;

public sealed class ProposedOccasion : Entity<Guid>
{
    public string Title { get; init; }
    public string Description { get; init; }
    public LocalDateTime StartDate { get; init; }
    public LocalDateTime EndDate { get; init; }
    public OrganizerAccountId Organizer { get; init; }
    public VenueId Venue { get; init; }

    internal ProposedOccasion(Guid id, string title, string description, LocalDateTime startDate, LocalDateTime endDate, OrganizerAccountId organizer, VenueId venue) : base(id)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be empty.", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Description cannot be empty.", nameof(description));
        }

        Title = title;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        Organizer = organizer;
        Venue = venue;
    }
}