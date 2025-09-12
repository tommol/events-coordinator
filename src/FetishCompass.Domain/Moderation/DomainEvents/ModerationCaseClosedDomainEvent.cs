using FetishCompass.Domain.Moderation.Model;
using FetishCompass.Shared.Domain;

namespace FetishCompass.Domain.Moderation.DomainEvents;

public sealed record ModerationCaseClosedDomainEvent(ModerationCaseId ModerationCaseId, OccasionSubmissionId Submission, DateTimeOffset ClosedAt, bool PositiveResolution, ReviewNotes? Notes) : DomainEvent;