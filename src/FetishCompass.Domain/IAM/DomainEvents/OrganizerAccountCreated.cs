using FetishCompass.Shared.Domain;

namespace FetishCompass.Domain.IAM.DomainEvents;

public sealed record OrganizerAccountCreatedDomainEvent(OrganizerAccountId AccountId, string Name, string Email, OrganizerAccountType AccountType, OrganizerType OrganizerType) :DomainEvent;
public sealed record OrganizerAccountLinkedWithAuthDomainEvent(OrganizerAccountId AccountId, string AuthId) :DomainEvent;