﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.API.ViewModels
{
	public class ProductDiscountVM
	{
		public Guid Id { get; set; }
		public string ProductId { get; set; }
		public string Description { get; set; }
		public double Percent { get; set; }
		public string CreateTimeStamp { get; set; }
		public bool IsExpired { get; set; }
	}
}
