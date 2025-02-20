using Superhero.Data;
using Superhero.Entities;

namespace Superhero.Repositories
{
    public class UnhandledExceptionRepository : IUnhandledExceptionRepository
    {
        private readonly DataContext _context;

        public UnhandledExceptionRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<int> RecordAsync(UnhandledException exception)
        {
            _context.UnhandledExceptions.Add(exception);
            await _context.SaveChangesAsync();
            return exception.Id;
        }
    }
}
