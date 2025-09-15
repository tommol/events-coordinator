using AutoFixture;
using FetishCompass.Domain.IAM.Model;

namespace FetishCompass.UnitTests.IAM.Domain;

public sealed class OrganizerAccountTests
{
    private IFixture _fixture = new Fixture();
    private OrganizerAccountId NewOrganizerAccountId() => FetishCompass.Domain.IAM.Model.OrganizerAccountId.New();

    [Fact]
    public void CreatePersonWithoutLogin_ValidData_Succeeds()
    {
        var id = NewOrganizerAccountId();
        var name = _fixture.Create<string>();
        var email = _fixture.Create<string>();
        var account = OrganizerAccount.CreatePerson(id, name, email);
        
        Assert.Equal(id, account.Id);
        Assert.Equal(name, account.Name);
        Assert.Equal(email, account.Email);
        Assert.Null(account.AuthId);
        Assert.Equal(OrganizerAccountType.Anonymous, account.AccountType);
        Assert.Equal(OrganizerType.Person, account.OrganizerType);
    }
    
    [Fact]
    public void CreatePersonWithLogin_ValidData_Succeeds()
    {
        var id = NewOrganizerAccountId();
        var name = _fixture.Create<string>();
        var email = _fixture.Create<string>();
        var account = OrganizerAccount.CreatePersonWithLogin(id, name, email);
        
        Assert.Equal(id, account.Id);
        Assert.Equal(name, account.Name);
        Assert.Equal(email, account.Email);
        Assert.Null(account.AuthId);
        Assert.Equal(OrganizerAccountType.Registered, account.AccountType);
        Assert.Equal(OrganizerType.Person, account.OrganizerType);
    }
    
    [Fact]
    public void CreateNonProfitWithoutLogin_ValidData_Succeeds()
    {
        var id = NewOrganizerAccountId();
        var name = _fixture.Create<string>();
        var email = _fixture.Create<string>();
        var account = OrganizerAccount.CreateNonProfit(id, name, email);
        
        Assert.Equal(id, account.Id);
        Assert.Equal(name, account.Name);
        Assert.Equal(email, account.Email);
        Assert.Null(account.AuthId);
        Assert.Equal(OrganizerAccountType.Anonymous, account.AccountType);
        Assert.Equal(OrganizerType.NonProfit, account.OrganizerType);
    }
    
    [Fact]
    public void CreateNonProfitWithLogin_ValidData_Succeeds()
    {
        var id = NewOrganizerAccountId();
        var name = _fixture.Create<string>();
        var email = _fixture.Create<string>();
        var account = OrganizerAccount.CreateNonProfitWithLogin(id, name, email);
        
        Assert.Equal(id, account.Id);
        Assert.Equal(name, account.Name);
        Assert.Equal(email, account.Email);
        Assert.Null(account.AuthId);
        Assert.Equal(OrganizerAccountType.Registered, account.AccountType);
        Assert.Equal(OrganizerType.NonProfit, account.OrganizerType);
    }
    
    [Fact]
    public void CreateCompanyWithoutLogin_ValidData_Succeeds()
    {
        var id = NewOrganizerAccountId();
        var name = _fixture.Create<string>();
        var email = _fixture.Create<string>();
        var account = OrganizerAccount.CreateCompany(id, name, email);
        
        Assert.Equal(id, account.Id);
        Assert.Equal(name, account.Name);
        Assert.Equal(email, account.Email);
        Assert.Null(account.AuthId);
        Assert.Equal(OrganizerAccountType.Anonymous, account.AccountType);
        Assert.Equal(OrganizerType.Business, account.OrganizerType);
    }
    
    [Fact]
    public void CreateCompanyWithLogin_ValidData_Succeeds()
    {
        var id = NewOrganizerAccountId();
        var name = _fixture.Create<string>();
        var email = _fixture.Create<string>();
        var account = OrganizerAccount.CreateCompanyWithLogin(id, name, email);
        
        Assert.Equal(id, account.Id);
        Assert.Equal(name, account.Name);
        Assert.Equal(email, account.Email);
        Assert.Null(account.AuthId);
        Assert.Equal(OrganizerAccountType.Registered, account.AccountType);
        Assert.Equal(OrganizerType.Business, account.OrganizerType);
    }
    
    [Theory]
    [InlineData(OrganizerType.Person)]
    [InlineData(OrganizerType.NonProfit)]
    [InlineData(OrganizerType.Business)]
    public void LinkAuth_ValidState_SetsAuthId(OrganizerType accountType)
    {
        var id = NewOrganizerAccountId();
        var name = _fixture.Create<string>();
        var email = _fixture.Create<string>();
        OrganizerAccount account = accountType switch
        {
            OrganizerType.Person => OrganizerAccount.CreatePersonWithLogin(id, name, email),
            OrganizerType.NonProfit => OrganizerAccount.CreateNonProfitWithLogin(id, name, email),
            OrganizerType.Business => OrganizerAccount.CreateCompanyWithLogin(id, name, email),
            _ => throw new ArgumentOutOfRangeException(nameof(accountType), accountType, null)
        };
        
        var authId = _fixture.Create<string>();
        account.LinkAuthAccount(authId);
        
        Assert.Equal(authId, account.AuthId);
        Assert.Equal(accountType, account.OrganizerType);
    }
    
    [Theory]
    [InlineData(OrganizerType.Person)]
    [InlineData(OrganizerType.NonProfit)]
    [InlineData(OrganizerType.Business)]
    public void LinkAuth_InvalidState_Throws(OrganizerType accountType)
    {
        var id = NewOrganizerAccountId();
        var name = _fixture.Create<string>();
        var email = _fixture.Create<string>();
        var account = accountType switch
        {
            OrganizerType.Person => OrganizerAccount.CreatePerson(id, name, email),
            OrganizerType.NonProfit => OrganizerAccount.CreateNonProfit(id, name, email),
            OrganizerType.Business => OrganizerAccount.CreateCompany(id, name, email),
            _ => throw new ArgumentOutOfRangeException(nameof(accountType), accountType, null)
        };
        
        var authId = _fixture.Create<string>();
        
        Assert.Throws<InvalidOperationException>(()=>account.LinkAuthAccount(authId) );
    }
}