using FetishCompass.Domain.Moderation.Model;
using FetishCompass.Shared.Application.Commands;

namespace FetishCompass.Application.Moderation.Commands;

public sealed record AcceptSubmissionCommand(OccasionSubmissionId SubmissionId, DateTimeOffset ReviewedAt) : ICommand;