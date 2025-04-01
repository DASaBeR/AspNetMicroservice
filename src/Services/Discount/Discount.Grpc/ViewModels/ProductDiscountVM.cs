namespace Discount.Grpc.ViewModels
{
	public class ProductDiscountVM
	{
		public string Id { get; set; }
		public string ProductId { get; set; }
		public string Description { get; set; }
		public double Percent { get; set; }
		public string CreateTimeStamp { get; set; }
		public bool IsExpired { get; set; }
	}
}
