using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Domain.Moderation.Model;
using FetishCompass.Shared.Domain;

namespace FetishCompass.Domain.Moderation.DomainEvents;

public sealed record OccasionSubmissionSentToReviewDomainEvent(OccasionSubmissionId OccasionSubmissionId, OccasionId OccasionId) : DomainEvent;