using FetishCompass.Domain;
using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Domain.IAM;
using FetishCompass.Domain.Moderation.Model;
using FetishCompass.Shared.Application.Commands;

namespace FetishCompass.Application.Moderation.Commands;

public sealed record CreateSubmissionCommand(
    OccasionSubmissionId SubmissionId,
    OccasionId OccasionId,
    string Title,
    string Description,
    LocalDateTime Start,
    LocalDateTime End,
    OrganizerAccountId Organizer,
    VenueId Venue) : ICommand;