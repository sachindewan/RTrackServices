using Basket.Api.Data.Interfaces;
using Basket.Api.Repositories.Interfaces;
using Basket.API.Entities;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace Basket.Api.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IBasketContext _basketContext;

        public BasketRepository(IBasketContext basketContext)
        {
            _basketContext = basketContext;
        }
        public async Task<bool> DeleteBasket(string userName)
        {
           return  await _basketContext
                                .Redis
                                .KeyDeleteAsync(userName);
        }

        public async Task<BasketCart> GetBasket(string userName)
        {
            var basket = await _basketContext
                            .Redis
                            .StringGetAsync(userName);
            if (basket.IsNullOrEmpty)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<BasketCart>(basket);
        }

        public async Task<BasketCart> UpdateBasket(BasketCart basket)
        {
            var updated = await _basketContext
                             .Redis
                             .StringSetAsync(basket.UserName, JsonConvert.SerializeObject(basket));
            if (!updated)
            {
                return null;
            }
            return await GetBasket(basket.UserName);
        }
    }
}
