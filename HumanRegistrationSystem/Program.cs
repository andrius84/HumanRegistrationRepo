using HumanRegistrationSystem.Database;
using HumanRegistrationSystem.Repositories;
using HumanRegistrationSystem.Services;
using HumanRegistrationSystem.AdditionalServices;
using HumanRegistrationSystem.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HumanRegistrationSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<PictureProcessor>();

            builder.Services.AddTransient<IJwtService, JwtService>();
            
            builder.Services.AddTransient<IAccountMapper, AccountMapper>();
            builder.Services.AddTransient<IAccountService, AccountService>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();

            builder.Services.AddTransient<IPersonMapper, PersonMapper>();
            builder.Services.AddTransient<IPersonService, PersonService>();
            builder.Services.AddScoped<IPersonRepository, PersonRepository>();

            builder.Services.AddTransient<IAddressMapper, AddressMapper>();
            builder.Services.AddTransient<IAddressService, AddressService>();
            builder.Services.AddScoped<IAddressRepository, AddressRepository>();

            builder.Services.AddTransient<IRoleService, RoleService>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) //This method call configures the application to use authentication middleware, specifically setting it up to use JWT bearer tokens as the default scheme. This tells ASP.NET Core that when a request comes in, it should expect the user's identity to be represented in a JWT format.
              .AddJwtBearer(options => //This extension method adds and configures JWT bearer token-based authentication to the project. Within the lambda (options => { ... }), you're specifying how incoming tokens are validated.
              {
                  var secretKey = builder.Configuration.GetSection("Jwt:Key").Value!; //retrieves the secret key used to sign the tokens from the application's configuration, such as an appsettings.json file. This key is crucial for the security of the JWTs, as it's used to validate the signature of incoming tokens.
                  var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey)); //creates a new instance of SymmetricSecurityKey using the secret key. This key is used to validate the signature of the token, ensuring it was issued by a trusted party and has not been tampered with.
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = false, //ensures that the issuer of the token matches the expected issuer, a security measure to prevent tokens issued by an unauthorized server from being accepted.
                      ValidateAudience = false, //ensures that the token's audience matches the expected audience value, verifying that the token is intended to be used by the application.
                      ValidateLifetime = true, //checks that the token has not expired.
                      ValidateIssuerSigningKey = true, //confirms that the token is signed with the expected key, helping to prevent forgery.
                      ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value, //specifies the expected issuer of the token, usually a URL or an identifier for the authentication server.
                      ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value, //specifies the intended recipient of the token, typically the identifier of the ASP.NET Core application.
                      IssuerSigningKey = key //sets the key used to validate the token's signature.
                  };
              });

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Human registration system",
                    Version = "v1"
                });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header is using Bearer scheme.",
                    Name = "Autorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { "Bearer" }
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //app.UseCors(builder => //This method is called to add the CORS middleware to the application's request processing pipeline. 
            //{
            //    builder
            //    //.AllowAnyOrigin() //This method call configures the CORS policy to allow requests from any origin. In a production environment, it's generally recommended to be more specific about which origins are allowed to ensure the security of your web application.
            //    .WithOrigins("http://localhost:5500")
            //    .AllowAnyMethod() //This allows the CORS policy to accept requests made with any HTTP method (such as GET, POST, PUT, DELETE, etc.). This is useful for a RESTful API that needs to support a wide range of actions on resources.
            //    .AllowAnyHeader() //his configures the CORS policy to allow any headers in the requests. Headers are often used in requests to carry information about the content type, authentication, etc. Allowing any header supports a wide range of requests that might include custom or standard headers.
            //    .AllowCredentials();
            //}); //This configuration is very permissive, allowing any web application to make requests to your ASP.NET Core Web API regardless of the origin, HTTP method, or headers used in the request. While this setup is useful for development or when you need to allow wide access to your API, it's important to tighten the CORS policy for production environments to minimize security risks. You would typically do this by specifying allowed origins, methods, and headers that match the requirements of your specific client applications.


            app.UseCors(builder =>
            {
                builder
                    .WithOrigins("http://localhost:5500", "http://127.0.0.1:5500") // Specify allowed origins
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

