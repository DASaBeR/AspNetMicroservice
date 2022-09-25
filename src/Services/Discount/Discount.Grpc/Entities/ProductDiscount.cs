using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Entities
{
	public class ProductDiscount
	{
		public Guid Id { get; set; }
		public string ProductId { get; set; }
		public string Description { get; set; }
		public double Percent { get; set; }
		public string CreateTimeStamp { get; set; }
		public bool IsExpired { get; set; }

		public ProductDiscount()
		{
			Id = Guid.NewGuid();
			CreateTimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
		}
	}
}
