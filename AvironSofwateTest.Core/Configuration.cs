using AvironSofwateTest.Core.DepartmentServices;
using AvironSofwateTest.Core.EmployeeService;
using AvironSofwateTest.Core.ReviewService;
using AvironSofwateTest.Core.ReviewServoce;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection;
public static class Configuration
{
    public static IServiceCollection AddCoreService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IReviewService, ReviewService>();

        return services;
    }
}

