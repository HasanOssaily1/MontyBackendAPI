using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MontyBackendAPI.Models;
using MontyBackendAPI.Repositories;

namespace MontyBackendAPI.Controllers
{

    [Route("api/Subscriptions")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionsRepository _subscriptionsRepository;

        public SubscriptionsController(ISubscriptionsRepository subscriptionsRepository)
        {
            _subscriptionsRepository = subscriptionsRepository;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Subscriptions>>> GetAllSubscriptions()
        {
            var subscriptions = await _subscriptionsRepository.GetAllSubscriptions();
            return Ok(subscriptions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Subscriptions>> GetSubscriptionById(int id)
        {

            var subscription = await _subscriptionsRepository.GetSubscriptionById(id);
            if (subscription == null)
            {
                return NotFound();
            }
            return Ok(subscription);
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<Subscriptions>> GetSubscriptionByUserId(int id, [FromQuery] bool? active)
        {
            IEnumerable<Subscriptions>  subscriptions;
            if (!active.HasValue)
            {
                subscriptions = await _subscriptionsRepository.GetSubscriptionsByUserId(id);

            }
            else
            {

                subscriptions = await _subscriptionsRepository.GetSubscriptionsByUserId(id, active.Value);

            }

            if (subscriptions == null || !subscriptions.Any())
            {
                return NotFound();
            }

            return Ok(subscriptions);
        }

        [HttpPost]
        public async Task<ActionResult<Subscriptions>> CreateSubscription(Subscriptions subscription)
        {
            if (subscription.userid == 0)
            {
                return BadRequest();
            }
            subscription.creationdate = DateTime.UtcNow;
            subscription.modificationdate = DateTime.UtcNow;
            await _subscriptionsRepository.Create(subscription);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubscription(int id, Subscriptions subscription)
        {
            if (id != subscription.id)
            {
                return BadRequest();
            }
            subscription.modificationdate = DateTime.UtcNow;
            await _subscriptionsRepository.Update(subscription);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            var deleted = await _subscriptionsRepository.Delete(id);
            if (!deleted)
            {
                return BadRequest();
            }
            return Ok();
        }

    }
}
