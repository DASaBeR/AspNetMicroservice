﻿using Basket.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
	public interface IBasketRepository
	{
		Task<ShoppingCart> GetBasket(string username);

		//We Will Useing UpdateBasket As CreateBasket and AddItem & ...
		Task<ShoppingCart> UpdateBasket(ShoppingCart basket);
		Task DeleteBasket(string username);
	}
}
