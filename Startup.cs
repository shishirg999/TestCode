using Auth0.ManagementApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnboardingApp.Model;
using OnboardingApp.Services;
using Pomelo.EntityFrameworkCore.MySql;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingApp
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var jwtConfiguration = Configuration.GetSection("Jwt").Get<JwtConfiguration>();
            services.Configure<JwtConfiguration>(instance => Configuration.Bind("Jwt", instance));
            services.AddScoped(provider => provider.GetRequiredService<IOptionsSnapshot<JwtConfiguration>>().Value);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = jwtConfiguration.Issuer,
                        ValidAudience = jwtConfiguration.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Key))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = ValidateToken,
                    };
                });

            // Configure Entityframecore with SQL SErver
            services.AddDbContextPool<AppDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(Configuration.GetConnectionString("DefaultConnection")));
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<ITokenValidator, TokenValidator>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ILoginContext, LoginContext>();
            services.AddScoped<IProviderContext, ProviderContext>();
            services.AddScoped<IProviderRepository, ProviderRepository>();
            services.AddScoped<IConversationContext, ConversationContext>();
            services.AddScoped<IConversationRepository, ConversationRepository>();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OnboardingApp", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }

        public static string GetTokenFromHeader(IHeaderDictionary requestHeaders)
        {
            if (!requestHeaders.TryGetValue("Authorization", out var authorizationHeader))
                throw new InvalidOperationException("Authorization token does not exists");

            var authorization = authorizationHeader.FirstOrDefault()!.Split(" ");

            var type = authorization[0];

            if (type != "Bearer") throw new InvalidOperationException("You should provide a Bearer token");

            var value = authorization[1] ?? throw new InvalidOperationException("Authorization token does not exists");
            return value;
        }

        public static Task ValidateToken(MessageReceivedContext context)
        {
            try
            {
                context.Token = GetTokenFromHeader(context.Request.Headers);

                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(context.Token, context.Options.TokenValidationParameters, out var validatedToken);

                var jwtSecurityToken = validatedToken as JwtSecurityToken;

                context.Principal = new ClaimsPrincipal();

                Debug.Assert(jwtSecurityToken != null, nameof(jwtSecurityToken) + " != null");

                var claimsIdentity = new ClaimsIdentity(jwtSecurityToken.Claims.ToList(), "JwtBearerToken",
                    ClaimTypes.NameIdentifier, ClaimTypes.Role);

                context.Principal.AddIdentity(claimsIdentity);

                context.Success();

                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                context.Fail(e);
            }

            return Task.CompletedTask;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(options =>
                {
                    options.PreSerializeFilters.Add((swagger, httpReq) =>
                    {
                        //Clear servers -element in swagger.json because it got the wrong port when hosted behind reverse proxy
                        swagger.Servers.Clear();
                    });
                });

                app.UseSwaggerUI(c =>
                {
                    string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                    c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "OnboardingApp v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
