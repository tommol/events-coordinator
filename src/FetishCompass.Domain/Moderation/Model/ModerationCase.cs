using System.Diagnostics.CodeAnalysis;
using FetishCompass.Domain.Moderation.DomainEvents;
using FetishCompass.Shared.Domain;

namespace FetishCompass.Domain.Moderation.Model;

public class ModerationCase : AggregateRoot<ModerationCaseId>
{
    private List<ReviewNotes> notes = [];
    
    public OccasionSubmissionId Submission{ get; private set;  }
    public ModerationCaseStatus Status { get; private set; }
    public AutomatedReviewFlag? AutomatedReview { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ClosedAt { get; private set; }
    
    public IReadOnlyList<ReviewNotes> Notes => this.notes.AsReadOnly();

    [ExcludeFromCodeCoverage]
    private ModerationCase(){}
    
    private ModerationCase(
        ModerationCaseId id,
        OccasionSubmissionId submissionId,
        ModerationCaseStatus status,
        DateTimeOffset createdAt,
        AutomatedReviewFlag? automatedReview)
        : base(id)
    {
        this.Submission = submissionId;
        this.Status = status;
        this.AutomatedReview = automatedReview;
        this.CreatedAt = createdAt;
    }
    public static ModerationCase Create(ModerationCaseId id, OccasionSubmissionId submissionId)
    {
      
        var moderationCase =  new ModerationCase(
            id,
            submissionId,
            ModerationCaseStatus.Open,
            DateTimeOffset.UtcNow,
            null);
        moderationCase.IncrementVersion();
        moderationCase.ApplyChange(new ModerationCaseCreatedDomainEvent(id, submissionId, moderationCase.CreatedAt));
        return moderationCase;
    }

    public void Accept(ReviewNotes reviewNotes, bool automatedReview, DateTimeOffset acceptedAt)
    {
        if (this.Status == ModerationCaseStatus.Closed)
        {
            throw new InvalidOperationException("Moderation case is already closed.");
        }
        if (this.Status != ModerationCaseStatus.Open && automatedReview)
        {
            throw new InvalidOperationException("Moderation case is already in review by moderator.");
        }

        if (automatedReview)
        {
            MarkAsModified();
            ApplyChange(new ModerationCaseAcceptedDomainEvent(this.Id, reviewNotes, automatedReview, acceptedAt));
        }
        else
        {
            MarkAsModified();
            ApplyChange(new ModerationCaseClosedDomainEvent(this.Id, this.Submission, acceptedAt,true, reviewNotes));
        }
    }
    
    public void Reject(ReviewNotes reviewNotes, bool automatedReview, DateTimeOffset rejectedAt)
    {
        if (this.Status == ModerationCaseStatus.Closed)
        {
            throw new InvalidOperationException("Moderation case is already closed.");
        }
        if (this.Status != ModerationCaseStatus.Open && automatedReview)
        {
            throw new InvalidOperationException("Moderation case is already in review by moderator.");
        }

        if (automatedReview)
        {
            MarkAsModified();
            ApplyChange(new ModerationCaseRejectedDomainEvent(this.Id, reviewNotes, automatedReview, rejectedAt));
        }
        else
        {
            MarkAsModified();
            ApplyChange(new ModerationCaseClosedDomainEvent(this.Id, this.Submission, rejectedAt,false, reviewNotes));
        }
    }

    private void Apply(ModerationCaseCreatedDomainEvent domainEvent)
    {
        this.Id = domainEvent.ModerationCaseId;
        this.Submission = domainEvent.OccasionSubmissionId;
        this.CreatedAt = domainEvent.CreatedAt;
        this.Status = ModerationCaseStatus.Open;
        this.AutomatedReview = null;
    }

    private void Apply(ModerationCaseAcceptedDomainEvent domainEvent)
    {
        if (domainEvent.ModerationCaseId != this.Id)
        {
            throw new InvalidOperationException("Domain event does not belong to this moderation case.");
        }

        if (domainEvent.AutomatedReview)
        {
            this.AutomatedReview = AutomatedReviewFlag.Green;
            this.Status = ModerationCaseStatus.InReview;
            this.notes.Add(domainEvent.ReviewNotes);
            // Automated review does not close the case, it just adds a note and sets the flag
        }
    }

    private void Apply(ModerationCaseClosedDomainEvent domainEvent)
    {
        if (domainEvent.ModerationCaseId != this.Id)
        {
            throw new InvalidOperationException("Domain event does not belong to this moderation case.");
        }
        this.Status = ModerationCaseStatus.Closed;
        if(domainEvent.Notes != null){
            this.notes.Add(domainEvent.Notes);
        }
        this.ClosedAt = domainEvent.ClosedAt;
    }
    
    private void Apply(ModerationCaseRejectedDomainEvent domainEvent)
    {
        if (domainEvent.ModerationCaseId != this.Id)
        {
            throw new InvalidOperationException("Domain event does not belong to this moderation case.");
        }

        if (domainEvent.AutomatedReview)
        {
            this.AutomatedReview = AutomatedReviewFlag.Yellow;
            this.Status = ModerationCaseStatus.InReview;
            this.notes.Add(domainEvent.ReviewNotes);
            // Automated review does not close the case, it just adds a note and sets the flag
        }
    }
    
}