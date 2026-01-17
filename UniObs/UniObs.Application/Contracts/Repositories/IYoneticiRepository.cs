using UniObs.Domain.Entities;

namespace UniObs.Application.Contracts.Repositories
{
    public interface IYoneticiRepository
    {
        Task<Yonetici> GetByEmailAsync(string email);
        Task UpdateAsync(Yonetici entity);
    }
}
