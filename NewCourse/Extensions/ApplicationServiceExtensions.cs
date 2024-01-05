using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NewCourse.Data;
using NewCourse.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace NewCourse.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            services.AddScoped<AccountService>();
            services.AddScoped<AccountUtility>();
            services.AddEndpointsApiExplorer();
            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache();
            services.AddSession();

            return services;
        }
    }
}
