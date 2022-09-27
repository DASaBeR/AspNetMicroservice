using AutoMapper;
using Discount.Grpc.Protos;
using Discount.Grpc.ViewModels;
using Discount.Grps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Mapper
{
	public class CouponProfile : Profile
	{
		public CouponProfile()
		{
			CreateMap<Coupon, CouponModel>().ReverseMap();
			CreateMap<CouponVM, CouponModel>().ReverseMap();
		}
	}
}
