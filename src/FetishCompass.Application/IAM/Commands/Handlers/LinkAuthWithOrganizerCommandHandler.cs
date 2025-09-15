using FetishCompass.Domain.IAM;
using FetishCompass.Domain.IAM.Model;
using FetishCompass.Shared.Application.Commands;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;

namespace FetishCompass.Application.IAM.Commands.Handlers;

public sealed class LinkAuthWithOrganizerCommandHandler(
    IAggregateRepository<OrganizerAccount, OrganizerAccountId> repository,
    ILogger<LinkAuthWithOrganizerCommandHandler> logger) : ICommandHandler<LinkAuthWithOrganizerCommand>
{
    public async Task Handle(LinkAuthWithOrganizerCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var account = await repository.GetByIdAsync((OrganizerAccountId)command.OrganizerId, cancellationToken);
            if (account == null)
            {
                throw new InvalidOperationException($"Organizer account with id {command.OrganizerId} not found");
            }
            account.LinkAuthAccount(command.AuthId);
            await repository.SaveAsync(account, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }
}