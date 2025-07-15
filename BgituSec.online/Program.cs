using BgituSec.Domain.Interfaces;
using BgituSec.Infrastructure.Data;
using BgituSec.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.OpenApi.Models;
using BgituSec.Api.Validators.User;
using BgituSec.Application.Services.Token;
using BgituSec.Application.Services.SSE;
using MediatR;
using BgituSec.Application.Features.Users.Commands;
using BgituSec.Api.Hubs;
using BgituSec.Api;

namespace BgituSec.Online
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IdentityModelEventSource.ShowPII = true;
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSignalR();
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginUserCommand).Assembly));

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();
            builder.Services.AddScoped<IAuditoriumRepository, AuditoriumRepository>();
            builder.Services.AddScoped<IComputerRepository, ComputerRepository>();
            builder.Services.AddScoped<IBreakdownRepository, BreakdownRepository>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<LoginUserRequestValidator>();
            builder.Services.AddControllers()
                            .AddJsonOptions(options =>
                                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            
            builder.Services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            });

            builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection("Jwt"));
            builder.Services.AddTransient<ITokenService, TokenService>();
            builder.Services.AddSingleton<ISSEService, SSEService>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.IncludeErrorDetails = true;

                var jwt = builder.Configuration.GetSection("Jwt").Get<JWTConfig>();
                var key = Encoding.UTF8.GetBytes(jwt!.Key);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwt.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwt.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });
            builder.Services.AddAuthorization();
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }
            app.UseSwagger(options =>
            {
                options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0;

            });
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapHub<BuildingHub>("/hubs/building");
            app.MapHub<AuditoriumHub>("/hubs/auditorium");
            app.MapHub<UserHub>("/hubs/user");
            app.MapHub<ComputerHub>("/hubs/computer");
            app.MapHub<BreakdownHub>("/hubs/breakdown");
            app.Run();
        }
    }
}
