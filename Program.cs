
using Ecommerce_Webapi.Data;
using Ecommerce_Webapi.Mapping;
using Ecommerce_Webapi.Services.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Ecommerce_Webapi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            
            
            builder.Services.AddControllers();
            builder.Services.AddLogging();
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
            builder.Services.AddScoped<IUserService,UserServices>();
            builder.Services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
            });
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(op =>
            {
                op.IncludeErrorDetails = true;
                op.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true



                };
                op.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("Token validated");
                        return Task.CompletedTask;
                    },
                    OnMessageReceived = msg =>
                    {
                        var token = msg?.Request.Headers.Authorization.ToString();
                        string path = msg?.Request.Path ?? "";
                        if (!string.IsNullOrEmpty(token))

                        {
                            Console.WriteLine("Access token");
                            Console.WriteLine($"URL: {path}");
                            Console.WriteLine($"Token: {token}\r\n");
                        }
                        else
                        {
                            Console.WriteLine("Access token");
                            Console.WriteLine("URL: " + path);
                            Console.WriteLine("Token: No access token provided\r\n");
                        }
                        return Task.CompletedTask;
                    }
                }; 


            });
            builder.Services.AddAuthorization();    

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseAuthentication();


            app.MapControllers();

            app.Run();
        }
    }
}
