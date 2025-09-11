namespace FetishCompass.Shared.Domain;

public interface IVersioned
{
    long Version { get; }
    void IncrementVersion();
}
