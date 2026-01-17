using Microsoft.EntityFrameworkCore;
using UniObs.Application.Contracts.Repositories;
using UniObs.Domain.Entities;

namespace UniObs.Infrastructure.Persistence.Repositories
{
    public class YoneticiRepository : IYoneticiRepository
    {
        private readonly ObsDbContext _context;

        public YoneticiRepository(ObsDbContext context)
        {
            _context = context;
        }

        public async Task<Yonetici> GetByEmailAsync(string email)
        {
            return await _context.Yoneticiler.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task UpdateAsync(Yonetici entity)
        {
            _context.Yoneticiler.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
