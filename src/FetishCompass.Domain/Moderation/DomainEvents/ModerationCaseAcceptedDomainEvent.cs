using FetishCompass.Domain.Moderation.Model;
using FetishCompass.Shared.Domain;

namespace FetishCompass.Domain.Moderation.DomainEvents;

public sealed record ModerationCaseAcceptedDomainEvent(ModerationCaseId ModerationCaseId, ReviewNotes ReviewNotes, bool AutomatedReview, DateTimeOffset AcceptedAt) : DomainEvent;