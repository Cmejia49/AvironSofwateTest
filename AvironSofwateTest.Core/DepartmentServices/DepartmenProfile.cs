using AutoMapper;
using AvironSofwateTest.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.Core.DepartmentServices
{
    public class DepartmenProfile : Profile
    {
        public DepartmenProfile()
        {
            CreateMap<Department, ReadDepartmentDto>()
             .ForMember(d => d.Id, m => m.MapFrom(s => s.Id))
             .ForMember(d => d.Name, m => m.MapFrom(x => x.Name));

            CreateMap<CreateDepartmentDto, Department>()
           .ForMember(d => d.Name, m => m.MapFrom(x => x.Name));

            CreateMap<UpdateDepartmentDto, Department>()
           .ForMember(d => d.Id, m => m.MapFrom(s => s.Id))
           .ForMember(d => d.Name, m => m.MapFrom(x => x.Name));


            CreateMap<Department, UpdateDepartmentDto>()
         .ForMember(d => d.Id, m => m.MapFrom(s => s.Id))
         .ForMember(d => d.Name, m => m.MapFrom(x => x.Name));
        }
    }
}
