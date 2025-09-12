using AutoFixture;
using FetishCompass.Domain.Moderation.Model;

namespace FetishCompass.UnitTests.Moderation.Model;

public class ModerationCaseTests
{
    private IFixture _fixture = new Fixture();
    private ModerationCaseId NewCaseId() => (ModerationCaseId)Activator.CreateInstance(typeof(ModerationCaseId), true)!;
    private OccasionSubmissionId NewSubmissionId() => (OccasionSubmissionId)Activator.CreateInstance(typeof(OccasionSubmissionId), true)!;
    private ReviewNotes NewReviewNotes() => new ReviewNotes("Test notatka", DateTimeOffset.UtcNow, false);

    [Fact]
    public void Create_ValidData_Succeeds()
    {
        var id = NewCaseId();
        var submissionId = NewSubmissionId();
        var moderationCase = ModerationCase.Create(id, submissionId);
        Assert.Equal(id, moderationCase.Id);
        Assert.Equal(submissionId, moderationCase.Submission);
        Assert.Equal(ModerationCaseStatus.Open, moderationCase.Status);
        Assert.NotEqual(default, moderationCase.CreatedAt);
        Assert.Null(moderationCase.AutomatedReview);
    }

    [Fact]
    public void Accept_AutomatedReview_ValidState_SetsFlagAndStatus()
    {
        var id = NewCaseId();
        var submissionId = NewSubmissionId();
        var moderationCase = ModerationCase.Create(id, submissionId);
        var notes = NewReviewNotes();
        var acceptedAt = DateTimeOffset.UtcNow;
        moderationCase.Accept(notes, true, acceptedAt);
        Assert.Equal(ModerationCaseStatus.InReview, moderationCase.Status);
        Assert.Equal(AutomatedReviewFlag.Green, moderationCase.AutomatedReview);
        Assert.Contains(notes, moderationCase.Notes);
    }

    [Fact]
    public void Accept_ManualReview_ValidState_ClosesCase()
    {
        var id = NewCaseId();
        var submissionId = NewSubmissionId();
        var moderationCase = ModerationCase.Create(id, submissionId);
        var notes = NewReviewNotes();
        var acceptedAt = DateTimeOffset.UtcNow;
        moderationCase.Accept(notes, false, acceptedAt);
        Assert.Equal(ModerationCaseStatus.Closed, moderationCase.Status);
        Assert.Contains(notes, moderationCase.Notes);
        Assert.Equal(acceptedAt, moderationCase.ClosedAt);
    }

    [Fact]
    public void Accept_ClosedCase_Throws()
    {
        var id = NewCaseId();
        var submissionId = NewSubmissionId();
        var moderationCase = ModerationCase.Create(id, submissionId);
        var notes = NewReviewNotes();
        var acceptedAt = DateTimeOffset.UtcNow;
        moderationCase.Accept(notes, false, acceptedAt);
        Assert.Throws<InvalidOperationException>(() => moderationCase.Accept(notes, false, acceptedAt));
    }

    [Fact]
    public void Reject_AutomatedReview_ValidState_SetsFlagAndStatus()
    {
        var id = NewCaseId();
        var submissionId = NewSubmissionId();
        var moderationCase = ModerationCase.Create(id, submissionId);
        var notes = NewReviewNotes();
        var rejectedAt = DateTimeOffset.UtcNow;
        moderationCase.Reject(notes, true, rejectedAt);
        Assert.Equal(ModerationCaseStatus.InReview, moderationCase.Status);
        Assert.Equal(AutomatedReviewFlag.Yellow, moderationCase.AutomatedReview);
        Assert.Contains(notes, moderationCase.Notes);
    }

    [Fact]
    public void Reject_ManualReview_ValidState_ClosesCase()
    {
        var id = NewCaseId();
        var submissionId = NewSubmissionId();
        var moderationCase = ModerationCase.Create(id, submissionId);
        var notes = NewReviewNotes();
        var rejectedAt = DateTimeOffset.UtcNow;
        moderationCase.Reject(notes, false, rejectedAt);
        Assert.Equal(ModerationCaseStatus.Closed, moderationCase.Status);
        Assert.Contains(notes, moderationCase.Notes);
        Assert.Equal(rejectedAt, moderationCase.ClosedAt);
    }

    [Fact]
    public void Reject_ClosedCase_Throws()
    {
        var id = NewCaseId();
        var submissionId = NewSubmissionId();
        var moderationCase = ModerationCase.Create(id, submissionId);
        var notes = NewReviewNotes();
        var rejectedAt = DateTimeOffset.UtcNow;
        moderationCase.Reject(notes, false, rejectedAt);
        Assert.Throws<InvalidOperationException>(() => moderationCase.Reject(notes, false, rejectedAt));
    }
}
