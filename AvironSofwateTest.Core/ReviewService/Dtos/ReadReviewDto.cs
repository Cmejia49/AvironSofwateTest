using AvironSofwateTest.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.Core.ReviewService.Dtos
{
    public record ReadReviewDto
    {
        public int Rate { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
    }
}
