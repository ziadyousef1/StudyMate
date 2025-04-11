using System.Text;
using EmailService;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Parameters;
using Serilog;
using StudyMate.Data;
using StudyMate.Models;
using StudyMate.Repositories.Implementaions;
using StudyMate.Repositories.Implementaions.Authentication;
using StudyMate.Repositories.Interfaces;
using StudyMate.Services.Implementaions;
using StudyMate.Services.Interfaces;
using StudyMate.Services.Interfaces.Authentication;
using StudyMate.Settings;
using StudyMate.Validations;
using StudyMate.Validations.Authentication;

namespace StudyMate.DependancyInjection;


    public static class  ServiceContainer
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ProductionConnection")));
            services.AddDefaultIdentity<AppUser>(opt =>
                {
                    opt.Password.RequireDigit = false;
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequireUppercase = false;
                } )
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAutoMapper(typeof(ServiceContainer));
            services.AddSingleton(Log.Logger);
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.JwtSetting));
            services.Configure<CloudStorageSettings>(configuration.GetSection(CloudStorageSettings.AzureStorage));
            services.Configure<AiSettings>(configuration.GetSection(AiSettings.AzureAi));
            
            services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();

            services.AddScoped(typeof(IAppLogger<>), typeof(SerilogLogger<>));
            services.AddScoped<IValldationService, ValidationService>();
          
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddScoped<ITokenRepository,TokenRepository>();
            services.AddScoped<IRoleRepository,RoleRepository>();
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<IImageService,ImageService>();
            services.AddScoped<ICloudStorageService, CloudStorageService>();
            services.AddScoped<ISummarizeService,SummarizeService>();
            var emailConfiguration = configuration.GetSection("EmailConfiguration").Get<EmailConfigration>();
            
            services.AddSingleton(emailConfiguration);  
            services.AddScoped<IEmailSender, EmailSender>();
            var serviceProvider = services.BuildServiceProvider();
            var jwtSettings = serviceProvider.GetRequiredService<IOptions<JwtSettings>>().Value;
            services.AddScoped<IVerificationCodeRepository, VerificationCodeRepository>();
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime =true,
                        RequireExpirationTime= true,
                        ValidIssuer= jwtSettings.Issuer,
                        ValidAudience= jwtSettings.Audience,
                        ClockSkew= TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    


                    };

                }
            );


            return services;
        }
        public static IApplicationBuilder UseApplicationServices(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            return app;
        }
    }
