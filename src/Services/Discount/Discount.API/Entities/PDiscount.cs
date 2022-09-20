using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.API.Entities
{
	public class PDiscount
	{
		public Guid Id { get; set; }
		public string ProductId { get; set; }
		public string Description { get; set; }
		public double Percent { get; set; }
		public string CreateTimeStamp { get; set; }
		public bool IsExpired { get; set; }

		public PDiscount()
		{
			var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
			CreateTimeStamp = timestamp;
		}
	}
}
