using Discount.Grpc.Protos;
using Discount.Grpc.ViewModels;
using Mapster;

namespace Discount.Grpc.Mapper
{
	public class CouponMappingConfig : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.NewConfig<CouponVM, CouponModel>()
				.Map(dest => dest.Id, src => src.Id ?? string.Empty)
				.Map(dest => dest.CreateTimeStamp, src => src.CreateTimeStamp ?? string.Empty)
				.Map(dest => dest.Description, src => src.Description ?? string.Empty);
		}
	}
}
