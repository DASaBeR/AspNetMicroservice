﻿using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
	public class BasketRepository : IBasketRepository
	{
		private readonly IDistributedCache _redisCache;

		public BasketRepository(IDistributedCache redisCache)
		{
			_redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
		}


		public async Task DeleteBasket(string username)
		{
			await _redisCache.RemoveAsync(username);
		}

		public async Task<ShoppingCart> GetBasket(string username)
		{
			var basket = await _redisCache.GetStringAsync(username);
			if (String.IsNullOrEmpty(basket)) 
				return null;

			var ShoppingCart = JsonConvert.DeserializeObject<ShoppingCart>(basket);
			return ShoppingCart;

		}

		public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
		{
			await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));

			return await GetBasket(basket.UserName);

		}
	}
}
