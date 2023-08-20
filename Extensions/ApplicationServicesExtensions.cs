using APIProject.Errors;
using APIProject.Helper;
using TalabatBLL.Interfaces;
using TalabatDAL;
using TalabatDAL.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIProject.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
           services.AddScoped<IProductRepository, ProductRepository>();
           services.AddScoped(typeof(IGenericRepositry<>), typeof(GenericRepositry<>));
           services.AddAutoMapper(typeof(MappingProfiles));
           services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAppSession, AppSession>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddSingleton<IResponseCasheService, ResponseCasheService>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ActionContext =>
                {
                    var errors = ActionContext.ModelState.Where(e => e.Value.Errors.Count > 0)
                                                         .SelectMany(x => x.Value.Errors)
                                                         .Select(x => x.ErrorMessage).ToList();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };
                
                    return new  BadRequestObjectResult(errorResponse);
                };

            });

            return services;
        }
    }
}
