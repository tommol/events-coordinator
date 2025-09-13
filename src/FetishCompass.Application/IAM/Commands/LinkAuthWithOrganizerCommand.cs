using FetishCompass.Shared.Application.Commands;

namespace FetishCompass.Application.IAM.Commands;

public sealed record LinkAuthWithOrganizerCommand(Guid OrganizerId, string AuthId):ICommand;