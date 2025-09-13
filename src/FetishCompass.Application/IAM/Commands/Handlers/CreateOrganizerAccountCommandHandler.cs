using FetishCompass.Domain.IAM;
using FetishCompass.Shared.Application.Commands;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;

namespace FetishCompass.Application.IAM.Commands.Handlers;

public sealed class CreateOrganizerAccountCommandHandler(
    IAggregateRepository<OrganizerAccount, OrganizerAccountId> repository,
    ILogger<CreateOrganizerAccountCommandHandler> logger)
    : ICommandHandler<CreateOrganizerAccountCommand, Guid>
{
    public async  Task<Guid> Handle(CreateOrganizerAccountCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            OrganizerAccount account;
            OrganizerAccountId id = OrganizerAccountId.New();
            if (command.IsPerson)
            {
                account = command.NeedAccount ? OrganizerAccount.CreatePersonWithLogin(id, command.Name, command.Email) : OrganizerAccount.CreatePerson(id, command.Name, command.Email);
            }else if (command.IsNonProfit)
            {
                account = command.NeedAccount ? OrganizerAccount.CreateNonProfitWithLogin(id, command.Name, command.Email) : OrganizerAccount.CreateNonProfit(id, command.Name, command.Email);  
            }
            else
            {
                account = command.NeedAccount ? OrganizerAccount.CreateCompanyWithLogin(id, command.Name, command.Email) : OrganizerAccount.CreateCompany(id, command.Name, command.Email);
            }

            await repository.SaveAsync(account, cancellationToken);
            return account.Id;
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }
}