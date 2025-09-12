using System.Diagnostics.CodeAnalysis;
using FetishCompass.Domain.Catalog.DomainEvents;
using FetishCompass.Domain.IAM;
using FetishCompass.Shared.Domain;

namespace FetishCompass.Domain.Catalog.Model;

public class Occasion : AggregateRoot<OccasionId>
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public OccasionSchedule Schedule { get; private set; }
    public OccasionStatus Status { get; private set; }
    public OrganizerAccountId Organizer { get; private set; }
    public VenueId Venue { get; private set; }

    [ExcludeFromCodeCoverage]
    private Occasion()
    {
    }
    
    private Occasion(
        OccasionId id,
        string title,
        string description,
        OccasionSchedule schedule,
        OccasionStatus status,
        OrganizerAccountId organizer,
        VenueId venue)
        : base(id)
    {
        this.Title = title;
        this.Description = description;
        this.Schedule = schedule;
        this.Status = status;
        this.Organizer = organizer;
        this.Venue = venue;
        this.Status = OccasionStatus.Draft;
    }
    public static Occasion Create(
        OccasionId id,
        string title,
        string description,
        OccasionSchedule schedule,
        OccasionStatus status,
        OrganizerAccountId organizer,
        VenueId venue)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.", nameof(title));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty.", nameof(description));

        if (organizer == null)
        {
            throw new ArgumentNullException(nameof(organizer), "Organizer cannot be null.");
        }

        if (venue == null)
        {
            throw new ArgumentNullException(nameof(venue), "Venue cannot be null.");
        }

        var occasion = new Occasion(id, title, description, schedule, status, organizer, venue);

        occasion.ApplyChange(new OccasionCreatedDomainEvent(
            id,
            title,
            description,
            organizer,
            venue,
            schedule.Start,
            schedule.End));
        
        
        occasion.IncrementVersion();

        return occasion;
    }
    
    public void Publish()
    {
        if (Status != OccasionStatus.Draft)
            throw new InvalidOperationException("Only draft occasions can be published.");

        ApplyChange(new OccasionPublishedDomainEvent(this.Id));
        MarkAsModified();
    }
    
    public void Cancel()
    {
        if (Status != OccasionStatus.Published)
            throw new InvalidOperationException("Only published occasions can be cancelled.");

        ApplyChange(new OccasionCancelledDomainEvent(this.Id));
        MarkAsModified();
    }
    
    public void UpdateDetails(string title, string description)
    {
        if (Status == OccasionStatus.Cancelled)
            throw new InvalidOperationException("Cancelled occasions cannot be updated.");
        
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.", nameof(title));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty.", nameof(description));
       
        ApplyChange(new OccasionDetailsUpdatedDomainEvent(this.Id, title, description));
        MarkAsModified();
    }
    
    public void UpdateSchedule(LocalDateTime start, LocalDateTime end)
    {
        var newSchedule = OccasionSchedule.Create(start, end);

        if (newSchedule.Start.Equals(this.Schedule.Start) && newSchedule.End.Equals(this.Schedule.End))
        {
            return;
        } 
        if (Status == OccasionStatus.Cancelled)
            throw new InvalidOperationException("Cancelled occasions cannot be updated.");

        ApplyChange(new OccasionScheduleUpdatedDomainEvent(this.Id, newSchedule));
        MarkAsModified();
    }
    
    public void ChangeVenue(VenueId newVenue)
    {
        if (this.Venue.Equals(newVenue))
            return;

        ApplyChange(new OccasionVenueChangedDomainEvent(this.Id, newVenue));
        MarkAsModified();
    }
    
    private void Apply(OccasionCreatedDomainEvent @event)
    {
        this.Id = @event.OccasionId;
        this.Title = @event.Title;
        this.Description = @event.Description;
        this.Organizer = @event.Organizer;
        this.Venue = @event.Venue;
        this.Schedule = OccasionSchedule.Create(@event.OccasionStart, @event.OccasionEnd);
        this.Status = OccasionStatus.Draft;
    }
    
    private void Apply(OccasionPublishedDomainEvent @event)
    {
        if (this.Id != @event.OccasionId)
            throw new InvalidOperationException("Event OccasionId does not match aggregate Id.");

        this.Status = OccasionStatus.Published;
    }
    
    private void Apply(OccasionCancelledDomainEvent @event)
    {
        if (this.Id != @event.OccasionId)
            throw new InvalidOperationException("Event OccasionId does not match aggregate Id.");

        this.Status = OccasionStatus.Cancelled;
    }
    
    private void Apply(OccasionDetailsUpdatedDomainEvent @event)
    {
        if (this.Id != @event.OccasionId)
            throw new InvalidOperationException("Event OccasionId does not match aggregate Id.");

        this.Title = @event.Title;
        this.Description = @event.Description;
    }
    
    private void Apply(OccasionScheduleUpdatedDomainEvent @event)
    {
        if (this.Id != @event.OccasionId)
            throw new InvalidOperationException("Event OccasionId does not match aggregate Id.");

        this.Schedule = @event.Schedule;
    }
    
    private void Apply(OccasionVenueChangedDomainEvent @event)
    {
        if (this.Id != @event.OccasionId)
            throw new InvalidOperationException("Event OccasionId does not match aggregate Id.");

        this.Venue = @event.Venue;
    }
    
}