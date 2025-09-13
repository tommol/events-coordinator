using FetishCompass.Shared.Application.Commands;

namespace FetishCompass.Application.IAM.Commands;

public sealed record CreateOrganizerAccountCommand(
    string Name, 
    string Email,
    bool NeedAccount,
    bool IsPerson,
    bool IsNonProfit
):ICommand<Guid>;