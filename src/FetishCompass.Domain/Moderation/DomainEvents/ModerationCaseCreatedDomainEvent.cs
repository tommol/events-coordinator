using FetishCompass.Domain.Moderation.Model;
using FetishCompass.Shared.Domain;

namespace FetishCompass.Domain.Moderation.DomainEvents;

public sealed record ModerationCaseCreatedDomainEvent(ModerationCaseId ModerationCaseId, OccasionSubmissionId OccasionSubmissionId, DateTimeOffset CreatedAt) : DomainEvent;