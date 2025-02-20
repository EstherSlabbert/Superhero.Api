using Superhero.Entities;

namespace Superhero.Repositories
{
    public interface IUnhandledExceptionRepository
    {
        Task<int> RecordAsync(UnhandledException exception);
    }
}
