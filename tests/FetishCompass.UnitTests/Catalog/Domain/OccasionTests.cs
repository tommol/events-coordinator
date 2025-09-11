using AutoFixture;
using FetishCompass.Domain;
using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Domain.IAM;

namespace FetishCompass.UnitTests.Catalog.Domain;

public class OccasionTests
{
    private IFixture _fixture = new Fixture();
    private OccasionId NewOccasionId() => (OccasionId)Activator.CreateInstance(typeof(OccasionId), true)!;
    private OrganizerAccountId NewOrganizerId() => OrganizerAccountId.New();
    private VenueId NewVenueId() => VenueId.New();
    private OccasionSchedule NewSchedule() => OccasionSchedule.Create(
         LocalDateTime.Create(DateOnly.MinValue, TimeOnly.MaxValue, "Europe/Warsaw"),
         LocalDateTime.Create(DateOnly.MaxValue, TimeOnly.MinValue, "Europe/Warsaw"));

    [Fact]
    public void Create_ValidData_Succeeds()
    {
        var id = NewOccasionId();
        var organizer = NewOrganizerId();
        var venue = NewVenueId();
        var schedule = NewSchedule();
        var occasion = Occasion.Create(id, "Tytuł", "Opis", schedule, OccasionStatus.Draft, organizer, venue);
        Assert.Equal("Tytuł", occasion.Title);
        Assert.Equal("Opis", occasion.Description);
        Assert.Equal(organizer, occasion.Organizer);
        Assert.Equal(venue, occasion.Venue);
        Assert.Equal(schedule.Start, occasion.Schedule.Start);
        Assert.Equal(schedule.End, occasion.Schedule.End);
        Assert.Equal(OccasionStatus.Draft, occasion.Status);
    }

    [Fact]
    public void Create_EmptyTitle_Throws()
    {
        var id = NewOccasionId();
        var organizer = NewOrganizerId();
        var venue = NewVenueId();
        var schedule = NewSchedule();
        Assert.Throws<ArgumentException>(() =>
            Occasion.Create(id, "", "Opis", schedule, OccasionStatus.Draft, organizer, venue));
    }

    [Fact]
    public void Publish_ValidState_ChangesStatus()
    {
        var id = NewOccasionId();
        var organizer = NewOrganizerId();
        var venue = NewVenueId();
        var schedule = NewSchedule();
        var occasion = Occasion.Create(id, "Tytuł", "Opis", schedule, OccasionStatus.Draft, organizer, venue);
        occasion.Publish();
        Assert.Equal(OccasionStatus.Published, occasion.Status);
    }

    [Fact]
    public void Cancel_ValidState_ChangesStatus()
    {
        var id = NewOccasionId();
        var organizer = NewOrganizerId();
        var venue = NewVenueId();
        var schedule = NewSchedule();
        var occasion = Occasion.Create(id, "Tytuł", "Opis", schedule, OccasionStatus.Draft, organizer, venue);
        occasion.Publish();
        occasion.Cancel();
        Assert.Equal(OccasionStatus.Cancelled, occasion.Status);
    }

    [Fact]
    public void UpdateDetails_Cancelled_Throws()
    {
        var id = NewOccasionId();
        var organizer = NewOrganizerId();
        var venue = NewVenueId();
        var schedule = NewSchedule();
        var occasion = Occasion.Create(id, "Tytuł", "Opis", schedule, OccasionStatus.Draft, organizer, venue);
        occasion.Publish();
        occasion.Cancel();
        Assert.Throws<InvalidOperationException>(() => occasion.UpdateDetails("Nowy", "Nowy opis"));
    }

    [Fact]
    public void UpdateDetails_ValidData_UpdatesTitleAndDescription()
    {
        var id = NewOccasionId();
        var organizer = NewOrganizerId();
        var venue = NewVenueId();
        var schedule = NewSchedule();
        var occasion = Occasion.Create(id, "Tytuł", "Opis", schedule, OccasionStatus.Draft, organizer, venue);
        occasion.UpdateDetails("Nowy tytuł", "Nowy opis");
        Assert.Equal("Nowy tytuł", occasion.Title);
        Assert.Equal("Nowy opis", occasion.Description);
    }

    [Fact]
    public void UpdateDetails_EmptyTitle_Throws()
    {
        var id = NewOccasionId();
        var organizer = NewOrganizerId();
        var venue = NewVenueId();
        var schedule = NewSchedule();
        var occasion = Occasion.Create(id, "Tytuł", "Opis", schedule, OccasionStatus.Draft, organizer, venue);
        Assert.Throws<ArgumentException>(() => occasion.UpdateDetails("", "Opis"));
    }

    [Fact]
    public void UpdateDetails_EmptyDescription_Throws()
    {
        var id = NewOccasionId();
        var organizer = NewOrganizerId();
        var venue = NewVenueId();
        var schedule = NewSchedule();
        var occasion = Occasion.Create(id, "Tytuł", "Opis", schedule, OccasionStatus.Draft, organizer, venue);
        Assert.Throws<ArgumentException>(() => occasion.UpdateDetails("Tytuł", ""));
    }

    [Fact]
    public void UpdateSchedule_ValidData_UpdatesSchedule()
    {
        var id = NewOccasionId();
        var organizer = NewOrganizerId();
        var venue = NewVenueId();
        var schedule = NewSchedule();
        var occasion = Occasion.Create(id, "Tytuł", "Opis", schedule, OccasionStatus.Draft, organizer, venue);
        var newStart = LocalDateTime.Create(DateOnly.MinValue.AddDays(1), TimeOnly.MinValue, "Europe/Warsaw");
        var newEnd = LocalDateTime.Create(DateOnly.MaxValue.AddDays(-1), TimeOnly.MaxValue, "Europe/Warsaw");
        occasion.UpdateSchedule(newStart, newEnd);
        Assert.Equal(newStart, occasion.Schedule.Start);
        Assert.Equal(newEnd, occasion.Schedule.End);
    }

    [Fact]
    public void UpdateSchedule_SameSchedule_NoChange()
    {
        var id = NewOccasionId();
        var organizer = NewOrganizerId();
        var venue = NewVenueId();
        var schedule = NewSchedule();
        var occasion = Occasion.Create(id, "Tytuł", "Opis", schedule, OccasionStatus.Draft, organizer, venue);
        occasion.UpdateSchedule(schedule.Start, schedule.End);
        Assert.Equal(schedule.Start, occasion.Schedule.Start);
        Assert.Equal(schedule.End, occasion.Schedule.End);
    }

    [Fact]
    public void UpdateSchedule_Cancelled_Throws()
    {
        var id = NewOccasionId();
        var organizer = NewOrganizerId();
        var venue = NewVenueId();
        var schedule = NewSchedule();
        var occasion = Occasion.Create(id, "Tytuł", "Opis", schedule, OccasionStatus.Draft, organizer, venue);
        occasion.Publish();
        occasion.Cancel();
        var newStart = LocalDateTime.Create(DateOnly.MinValue.AddDays(1), TimeOnly.MinValue, "Europe/Warsaw");
        var newEnd = LocalDateTime.Create(DateOnly.MaxValue.AddDays(-1), TimeOnly.MaxValue, "Europe/Warsaw");
        Assert.Throws<InvalidOperationException>(() => occasion.UpdateSchedule(newStart, newEnd));
    }

    [Fact]
    public void ChangeVenue_ValidData_ChangesVenue()
    {
        var id = NewOccasionId();
        var organizer = NewOrganizerId();
        var venue = NewVenueId();
        var schedule = NewSchedule();
        var occasion = Occasion.Create(id, "Tytuł", "Opis", schedule, OccasionStatus.Draft, organizer, venue);
        var newVenue = NewVenueId();
        occasion.ChangeVenue(newVenue);
        Assert.Equal(newVenue, occasion.Venue);
    }

    [Fact]
    public void ChangeVenue_SameVenue_NoChange()
    {
        var id = NewOccasionId();
        var organizer = NewOrganizerId();
        var venue = NewVenueId();
        var schedule = NewSchedule();
        var occasion = Occasion.Create(id, "Tytuł", "Opis", schedule, OccasionStatus.Draft, organizer, venue);
        occasion.ChangeVenue(occasion.Venue);
        Assert.Equal(venue, occasion.Venue);
    }
    
    [Fact]
    public void Publish_NotDraft_Throws()
    {
        var id = NewOccasionId();
        var organizer = NewOrganizerId();
        var venue = NewVenueId();
        var schedule = NewSchedule();
        var occasion = Occasion.Create(id, "Tytuł", "Opis", schedule, OccasionStatus.Draft, organizer, venue);
        occasion.Publish();
        Assert.Throws<InvalidOperationException>(() => occasion.Publish());
    }

    [Fact]
    public void Cancel_NotPublished_Throws()
    {
        var id = NewOccasionId();
        var organizer = NewOrganizerId();
        var venue = NewVenueId();
        var schedule = NewSchedule();
        var occasion = Occasion.Create(id, "Tytuł", "Opis", schedule, OccasionStatus.Draft, organizer, venue);
        Assert.Throws<InvalidOperationException>(() => occasion.Cancel());
    }
}
