using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.Core.ReviewService.Dtos
{
    public record UpdateReviewDto
    {
        public Guid Id { get; set; }
        public int Rate { get; set; }
        public Guid EmployeeId { get; set; }
    }
}
