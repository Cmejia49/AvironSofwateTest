using AutoMapper;
using AvironSofwateTest.Core.DepartmentServices;
using AvironSofwateTest.Core.EmployeeService.Dtos;
using AvironSofwateTest.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.Core.EmployeeService
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, ReadEmployeeDto>()
                 .ForMember(x => x.Id, m => m.MapFrom(s => s.Id))
                .ForMember(x => x.DepartmentName, m => m.MapFrom(s => s.Department.Name))
                .ForMember(x => x.Name, m => m.MapFrom(s => s.Name));

            CreateMap<CreateEmployeeDto, Employee>()
                .ForMember(x => x.Name , m => m.MapFrom(s => s.Name));

            CreateMap<UpdateEmployeeDto, Employee>()
                    .ForMember(x => x.Id, m => m.MapFrom(s => s.Id))
                .ForMember(x => x.Name, m => m.MapFrom(s => s.Name));

            CreateMap<Employee, UpdateEmployeeDto>()
                    .ForMember(x => x.Id, m => m.MapFrom(s => s.Id))
                .ForMember(x => x.Name, m => m.MapFrom(s => s.Name));
        }
    }
}
