using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Shared.Application.Commands;

namespace FetishCompass.Application.Catalog.Commands;

public sealed record MarkOccasionAsReadyCommand(OccasionId OccasionId) : ICommand;