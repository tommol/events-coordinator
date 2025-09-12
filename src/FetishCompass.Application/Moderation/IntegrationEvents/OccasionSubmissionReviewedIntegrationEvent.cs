using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Domain.Moderation.Model;
using FetishCompass.Shared.Application.IntegrationEvents;

namespace FetishCompass.Application.Moderation.IntegrationEvents;

public sealed record OccasionSubmissionReviewedIntegrationEvent(OccasionSubmissionId SubmissionId, OccasionId OccasionId, bool Approved, DateTimeOffset ReviewedAt) : IntegrationEvent;