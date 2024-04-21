using MontyBackendAPI.Models;

namespace MontyBackendAPI.Repositories
{
    public interface ISubscriptionsRepository
    {
        Task<IEnumerable<Subscriptions>> GetAllSubscriptions();
        Task<Subscriptions> GetSubscriptionById(int Id);

        Task<IEnumerable<Subscriptions>> GetSubscriptionsByUserId(int Id);
        Task<IEnumerable<Subscriptions>> GetSubscriptionsByUserId(int Id, bool active);
        Task Create(Subscriptions subscription);
        Task Update(Subscriptions subscription);
        Task<bool> Delete(int Id);
    }
}
