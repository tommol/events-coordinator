using System.Diagnostics.CodeAnalysis;
using FetishCompass.Domain.IAM.DomainEvents;
using FetishCompass.Shared.Domain;

namespace FetishCompass.Domain.IAM.Model;

public class OrganizerAccount : AggregateRoot<OrganizerAccountId>
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string? AuthId { get; private set; }
    public OrganizerAccountType AccountType { get; private set; }
    public OrganizerType OrganizerType { get; private set; }

    [ExcludeFromCodeCoverage]
    private OrganizerAccount()
    {
        
    }
    
    private OrganizerAccount(
        OrganizerAccountId id,
        string name,
        string email,
        OrganizerAccountType accountType,
        OrganizerType organizerType) : base(id)
    {
        Name = name;
        Email = email;
        AccountType = accountType;
        OrganizerType = organizerType;
    }
    
    public static OrganizerAccount CreatePerson(
        OrganizerAccountId id,
        string name,
        string email)
    {
        // Add any necessary validation here
        var account = new OrganizerAccount(id, name, email, OrganizerAccountType.Anonymous, OrganizerType.Person);
        account = PublishCreation(account);
        return account;
    }

    public static OrganizerAccount CreatePersonWithLogin(
        OrganizerAccountId id,
        string name,
        string email)
    {
        var account = new OrganizerAccount(id, name, email, OrganizerAccountType.Registered, OrganizerType.Person);
        account = PublishCreation(account);
        return account;
    }
    
    public static OrganizerAccount CreateNonProfit(
        OrganizerAccountId id,
        string name,
        string email)
    {
        // Add any necessary validation here
        var account = new OrganizerAccount(id, name, email, OrganizerAccountType.Anonymous, OrganizerType.NonProfit);
        account = PublishCreation(account);
        return account;
    }
    
    public static OrganizerAccount CreateNonProfitWithLogin(
        OrganizerAccountId id,
        string name,
        string email)
    {
        // Add any necessary validation here
        var account = new OrganizerAccount(id, name, email, OrganizerAccountType.Registered, OrganizerType.NonProfit);
        account = PublishCreation(account);
        return account;
    }
    
    public static OrganizerAccount CreateCompany(
        OrganizerAccountId id,
        string name,
        string email)
    {
        // Add any necessary validation here
        var account = new OrganizerAccount(id, name, email, OrganizerAccountType.Anonymous, OrganizerType.Business);
        account = PublishCreation(account);
        return account;
    }
    public static OrganizerAccount CreateCompanyWithLogin(
        OrganizerAccountId id,
        string name,
        string email)
    {
        // Add any necessary validation here
        var account = new OrganizerAccount(id, name, email, OrganizerAccountType.Registered, OrganizerType.Business);
        account = PublishCreation(account);
        return account;
    }

    public void LinkAuthAccount(string auth)
    {
        if (AccountType == OrganizerAccountType.Anonymous)
        {
            throw new InvalidOperationException("Cannot link auth account to an anonymous organizer account.");
        }
        ApplyChange(new OrganizerAccountLinkedWithAuthDomainEvent(Id,auth));
        MarkAsModified();
    }

    private static OrganizerAccount PublishCreation(OrganizerAccount account)
    {
        account.ApplyChange(new OrganizerAccountCreatedDomainEvent(
            account.Id,
            account.Name,
            account.Email,
            account.AccountType,
            account.OrganizerType));
        account.MarkAsModified();
        return account;
    }

    private void Apply(OrganizerAccountCreatedDomainEvent domainEvent)
    {
        this.Id = domainEvent.AccountId;
        this.Name = domainEvent.Name;
        this.Email = domainEvent.Email;
        this.AccountType = domainEvent.AccountType;
        this.OrganizerType = domainEvent.OrganizerType;
    }

    private void Apply(OrganizerAccountLinkedWithAuthDomainEvent domainEvent)
    {
        this.AuthId = domainEvent.AuthId;
    }
}