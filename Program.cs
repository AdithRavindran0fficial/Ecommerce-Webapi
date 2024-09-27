
using Ecommerce_Webapi.Data;
using Ecommerce_Webapi.Mapping;
using Ecommerce_Webapi.Services.CartService;
using Ecommerce_Webapi.Services.JWTServices;
using Ecommerce_Webapi.Services.ProductService;
using Ecommerce_Webapi.Services.UserService;
using Ecommerce_Webapi.Services.WhishListService;
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
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IJWTServices, JWTServices>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IWhishList, WhishListService>();
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
                    //ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    //ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true



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
