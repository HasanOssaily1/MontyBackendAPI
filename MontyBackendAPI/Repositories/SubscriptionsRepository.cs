using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MontyBackendAPI.Models;

namespace MontyBackendAPI.Repositories
{
    public class SubscriptionsRepository : ISubscriptionsRepository
    {
        private readonly MyContext _context;

        public SubscriptionsRepository(MyContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(int id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                // subscription not found, handle accordingly
                return false;
            }

            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Subscriptions>> GetAllSubscriptions()
        {
            return await _context.Subscriptions.ToListAsync();
        }

        public async Task<IEnumerable<Subscriptions>> GetSubscriptionsByUserId(int id, bool active)
        {
            DateTime currentDate = DateTime.UtcNow;

            IQueryable<Subscriptions> query = _context.Subscriptions
                                                .Where(x => x.userid == id);

            if (active)
            {
                query = query.Where(x => x.startdate <= currentDate && (x.enddate == null || x.enddate >= currentDate));
            }
            else
            {
                query = query.Where(x => x.startdate > currentDate || x.enddate < currentDate);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Subscriptions>> GetSubscriptionsByUserId(int id)
        {
            DateTime currentDate = DateTime.Now.Date;

            IQueryable<Subscriptions> query = _context.Subscriptions
                                                .Where(x => x.userid == id);

            return await query.ToListAsync();
        }

        public async Task<Subscriptions> GetSubscriptionById(int id)
        {

            return await _context.Subscriptions.FindAsync(id);
        }

        public async Task Create(Subscriptions subscription)
        {
                _context.Add(subscription);
                await _context.SaveChangesAsync(); // Save changes to the database
        }

        public async Task Update(Subscriptions subscription)
        {
            _context.Entry(subscription).State = EntityState.Modified;
            await _context.SaveChangesAsync(); // Save changes to the database
        }

    }

}
