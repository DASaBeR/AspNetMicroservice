using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grps.Entities
{
	public class Coupon
	{
		public Guid Id { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }
		public double Percent { get; set; }
		public string CreateTimeStamp { get; set; }
		public bool IsExpired { get; set; }

		public Coupon()
		{
			Id = Guid.NewGuid();
			CreateTimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
		}
	}
}
