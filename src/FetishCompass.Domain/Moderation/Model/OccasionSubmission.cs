using System.Diagnostics.CodeAnalysis;
using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Domain.IAM;
using FetishCompass.Domain.IAM.Model;
using FetishCompass.Domain.Moderation.DomainEvents;
using FetishCompass.Shared.Domain;

namespace FetishCompass.Domain.Moderation.Model;

public class OccasionSubmission : AggregateRoot<OccasionSubmissionId>
{
    public ProposedOccasion ProposedOccasion { get; private set; }
    public SubmissionStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ReviewedAt { get; private set; }
    
    public OrganizerAccountId Organizer { get; private set; }
    
    public OccasionId LinkedOccasion { get; private set; }
    
    [ExcludeFromCodeCoverage]
    private OccasionSubmission()
    {
    }
    
    private OccasionSubmission(
        OccasionSubmissionId id,
        ProposedOccasion proposedOccasion,
        SubmissionStatus status,
        DateTimeOffset createdAt,
        OrganizerAccountId organizer)
        : base(id)
    {
        this.ProposedOccasion = proposedOccasion;
        this.Status = status;
        this.CreatedAt = createdAt;
        this.Organizer = organizer;
    }
    
    public static OccasionSubmission Create(
        OccasionSubmissionId id,
        string title, 
        string description, 
        LocalDateTime startDate, 
        LocalDateTime endDate, 
        OrganizerAccountId organizer,
        OccasionId linkedOccasion,
        VenueId venue)
    {
        var proposedOccasion = new ProposedOccasion(
            Guid.NewGuid(),
            title,
            description,
            startDate,
            endDate,
            organizer,
            venue);
        
        var occasionSubmission = new OccasionSubmission(
            id,
            proposedOccasion,
            SubmissionStatus.Received,
            DateTimeOffset.UtcNow,
            organizer);
        occasionSubmission.IncrementVersion();
        
        occasionSubmission.ApplyChange(new OccasionSubmittedDomainEvent(
            occasionSubmission.Id, 
            proposedOccasion, 
            organizer, 
            occasionSubmission.CreatedAt));
        
        return occasionSubmission;
    }

    public void SendToReview()
    {
        if (this.Status != SubmissionStatus.Received)
            throw new InvalidOperationException("Only submissions with status 'Received' can be sent to review.");
        MarkAsModified();
        ApplyChange(new OccasionSubmissionSentToReviewDomainEvent(Id, this.LinkedOccasion));
    }
    
    
    public void Accept(DateTimeOffset reviewedAt)
    {
        if (this.Status != SubmissionStatus.UnderReview)
        {
            throw new InvalidOperationException("Only submissions with status 'UnderReview' can be accepted.");
        }
    
        MarkAsModified();
        ApplyChange(new OccasionSubmissionReviewedDomainEvent(this.Id,  this.LinkedOccasion,SubmissionStatus.Accepted, reviewedAt));
        
    }
    public void Reject(DateTimeOffset reviewedAt)
    {
        if (this.Status != SubmissionStatus.UnderReview)
        {
            throw new InvalidOperationException("Only submissions with status 'UnderReview' can be accepted.");
        }
    
        MarkAsModified();
        ApplyChange(new OccasionSubmissionReviewedDomainEvent(this.Id,this.LinkedOccasion, SubmissionStatus.Rejected, reviewedAt));

    }
    private void Apply(OccasionSubmittedDomainEvent domainEvent)
    {
        this.Id = domainEvent.OccasionSubmissionId;
        this.ProposedOccasion = domainEvent.Ocassion;
        this.Status = SubmissionStatus.Received;
        this.CreatedAt = domainEvent.CreatedAt;
        this.Organizer = domainEvent.Organizer;
    }
    
    private void Apply(OccasionSubmissionSentToReviewDomainEvent domainEvent)
    {
        if (this.Id != @domainEvent.OccasionSubmissionId)
            throw new InvalidOperationException("Event OccasionSubmissionId does not match aggregate Id.");

        this.Status = SubmissionStatus.UnderReview;
    }

    private void Apply(OccasionSubmissionReviewedDomainEvent domainEvent)
    {
        if (this.Id != domainEvent.OccasionSubmissionId)
            throw new InvalidOperationException("Event OccasionSubmissionId does not match aggregate Id.");

        this.Status = domainEvent.NewStatus;
        this.ReviewedAt = domainEvent.ReviewedAt;
    }
}