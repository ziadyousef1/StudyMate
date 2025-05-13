namespace StudyMate.DependancyInjection;

    public static class  ServiceContainer
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("LocalConnection")));
            services.AddDefaultIdentity<AppUser>(opt =>
                {
                    opt.Password.RequireDigit = false;
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequireUppercase = false;
                } )
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.JwtSetting));
            services.Configure<CloudStorageSettings>(configuration.GetSection(CloudStorageSettings.AzureStorage));
            services.Configure<AiSettings>(configuration.GetSection(AiSettings.AzureAi));
            
            services.AddAutoMapper(typeof(ServiceContainer));
            services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();
            services.AddSingleton(Log.Logger);
            services.AddSingleton(typeof(IAppLogger<>), typeof(SerilogLogger<>));
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
            
            services.AddSingleton<IEmailSender, EmailSender>();
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
