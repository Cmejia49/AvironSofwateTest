using AutoMapper;
using AvironSofwateTest.Core.DepartmentServices;
using AvironSofwateTest.Core.EmployeeService.Dtos;
using AvironSofwateTest.Core.ReviewService.Dtos;
using AvironSofwateTest.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.Core.ReviewService
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReadReviewDto>();

            CreateMap<CreateReviewDto, Review>();

            CreateMap<UpdateReviewDto, Review>();

            CreateMap<Review, UpdateReviewDto>();
        }
    }
}
