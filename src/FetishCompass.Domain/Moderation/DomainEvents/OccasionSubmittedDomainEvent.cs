using FetishCompass.Domain.IAM;
using FetishCompass.Domain.IAM.Model;
using FetishCompass.Domain.Moderation.Model;
using FetishCompass.Shared.Domain;

namespace FetishCompass.Domain.Moderation.DomainEvents;

public sealed record OccasionSubmittedDomainEvent(OccasionSubmissionId OccasionSubmissionId, ProposedOccasion Ocassion, OrganizerAccountId Organizer, DateTimeOffset CreatedAt) : DomainEvent;