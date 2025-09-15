using AutoFixture;
using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Domain.IAM.Model;
using FetishCompass.Domain.Moderation.Model;

namespace FetishCompass.UnitTests.Moderation.Domain;

public class OccasionSubmissionTests
{
    private IFixture _fixture = new Fixture();
    private OccasionSubmissionId NewSubmissionId() => (OccasionSubmissionId)Activator.CreateInstance(typeof(OccasionSubmissionId), true)!;
    private OrganizerAccountId NewOrganizerId() => OrganizerAccountId.New();
    private VenueId NewVenueId() => VenueId.New();
    private OccasionId NewOccasionId() => (OccasionId)Activator.CreateInstance(typeof(OccasionId), true)!;
    private FetishCompass.Domain.LocalDateTime NewLocalDateTime() => FetishCompass.Domain.LocalDateTime.Create(DateOnly.FromDateTime(DateTime.UtcNow), TimeOnly.FromDateTime(DateTime.UtcNow), "Europe/Warsaw");

    [Fact]
    public void Create_ValidData_Succeeds()
    {
        var id = NewSubmissionId();
        var organizer = NewOrganizerId();
        var venue = NewVenueId();
        var occasionId = NewOccasionId();
        var start = NewLocalDateTime();
        var end = NewLocalDateTime();
        var submission = OccasionSubmission.Create(id, "Tytuł", "Opis", start, end, organizer, occasionId, venue);
        Assert.Equal("Tytuł", submission.ProposedOccasion.Title);
        Assert.Equal("Opis", submission.ProposedOccasion.Description);
        Assert.Equal(organizer, submission.Organizer);
        Assert.Equal(SubmissionStatus.Received, submission.Status);
        Assert.NotEqual(default, submission.CreatedAt);
    }

    [Fact]
    public void SendToReview_ValidState_ChangesStatus()
    {
        var id = NewSubmissionId();
        var organizer = NewOrganizerId();
        var venue = NewVenueId();
        var occasionId = NewOccasionId();
        var start = NewLocalDateTime();
        var end = NewLocalDateTime();
        var submission = OccasionSubmission.Create(id, "Tytuł", "Opis", start, end, organizer, occasionId, venue);
        submission.SendToReview();
        Assert.Equal(SubmissionStatus.UnderReview, submission.Status);
    }

    [Fact]
    public void SendToReview_InvalidState_Throws()
    {
        var id = NewSubmissionId();
        var organizer = NewOrganizerId();
        var venue = NewVenueId();
        var occasionId = NewOccasionId();
        var start = NewLocalDateTime();
        var end = NewLocalDateTime();
        var submission = OccasionSubmission.Create(id, "Tytuł", "Opis", start, end, organizer, occasionId, venue);
        submission.SendToReview();
        Assert.Throws<InvalidOperationException>(() => submission.SendToReview());
    }

    [Fact]
    public void Accept_ValidState_ChangesStatus()
    {
        var id = NewSubmissionId();
        var organizer = NewOrganizerId();
        var venue = NewVenueId();
        var occasionId = NewOccasionId();
        var start = NewLocalDateTime();
        var end = NewLocalDateTime();
        var submission = OccasionSubmission.Create(id, "Tytuł", "Opis", start, end, organizer, occasionId, venue);
        submission.SendToReview();
        var reviewedAt = DateTimeOffset.UtcNow;
        submission.Accept(reviewedAt);
        Assert.Equal(SubmissionStatus.Accepted, submission.Status);
        Assert.Equal(reviewedAt, submission.ReviewedAt);
    }

    [Fact]
    public void Accept_InvalidState_Throws()
    {
        var id = NewSubmissionId();
        var organizer = NewOrganizerId();
        var venue = NewVenueId();
        var occasionId = NewOccasionId();
        var start = NewLocalDateTime();
        var end = NewLocalDateTime();
        var submission = OccasionSubmission.Create(id, "Tytuł", "Opis", start, end, organizer, occasionId, venue);
        Assert.Throws<InvalidOperationException>(() => submission.Accept(DateTimeOffset.UtcNow));
    }

    [Fact]
    public void Reject_ValidState_ChangesStatus()
    {
        var id = NewSubmissionId();
        var organizer = NewOrganizerId();
        var venue = NewVenueId();
        var occasionId = NewOccasionId();
        var start = NewLocalDateTime();
        var end = NewLocalDateTime();
        var submission = OccasionSubmission.Create(id, "Tytuł", "Opis", start, end, organizer, occasionId, venue);
        submission.SendToReview();
        var reviewedAt = DateTimeOffset.UtcNow;
        submission.Reject(reviewedAt);
        Assert.Equal(SubmissionStatus.Rejected, submission.Status);
        Assert.Equal(reviewedAt, submission.ReviewedAt);
    }

    [Fact]
    public void Reject_InvalidState_Throws()
    {
        var id = NewSubmissionId();
        var organizer = NewOrganizerId();
        var venue = NewVenueId();
        var occasionId = NewOccasionId();
        var start = NewLocalDateTime();
        var end = NewLocalDateTime();
        var submission = OccasionSubmission.Create(id, "Tytuł", "Opis", start, end, organizer, occasionId, venue);
        Assert.Throws<InvalidOperationException>(() => submission.Reject(DateTimeOffset.UtcNow));
    }

    [Fact]
    public void Create_WithEmptyTitle_ThrowsArgumentException()
    {
        var id = NewSubmissionId();
        var organizer = NewOrganizerId();
        var venue = NewVenueId();
        var occasionId = NewOccasionId();
        var start = NewLocalDateTime();
        var end = NewLocalDateTime();
        Assert.Throws<ArgumentException>(() =>
            OccasionSubmission.Create(id, "", "Opis", start, end, organizer, occasionId, venue));
        Assert.Throws<ArgumentException>(() =>
            OccasionSubmission.Create(id, "   ", "Opis", start, end, organizer, occasionId, venue));
        Assert.Throws<ArgumentException>(() =>
            OccasionSubmission.Create(id, null!, "Opis", start, end, organizer, occasionId, venue));
    }

    [Fact]
    public void Create_WithEmptyDescription_ThrowsArgumentException()
    {
        var id = NewSubmissionId();
        var organizer = NewOrganizerId();
        var venue = NewVenueId();
        var occasionId = NewOccasionId();
        var start = NewLocalDateTime();
        var end = NewLocalDateTime();
        Assert.Throws<ArgumentException>(() =>
            OccasionSubmission.Create(id, "Tytuł", "", start, end, organizer, occasionId, venue));
        Assert.Throws<ArgumentException>(() =>
            OccasionSubmission.Create(id, "Tytuł", "   ", start, end, organizer, occasionId, venue));
        Assert.Throws<ArgumentException>(() =>
            OccasionSubmission.Create(id, "Tytuł", null!, start, end, organizer, occasionId, venue));
    }
}
