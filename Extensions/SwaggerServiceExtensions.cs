using Microsoft.OpenApi.Models;

namespace APIProject.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwagerDocumention( this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "TalabatDemo", Version = "v1" });
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "Jwt Auth Bearer Schema",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Id="Bearer",
                        Type= ReferenceType.SecurityScheme
                    }
                };

                opt.AddSecurityDefinition("Bearer", securitySchema);
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };

                opt.AddSecurityRequirement(securityRequirement);
            });

            return services;
        }
    }
}
