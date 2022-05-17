using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoAppApi.EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PhotoAppApi.Services.Users;
using PhotoAppApi.Services.Mail;
using PhotoAppApi.Services.Posts;
using PhotoAppApi.Services.Photos;
using PhotoAppApi.Services.Comments;
using PhotoAppApi.Services.BackgroudServices;
using PhotoAppApi.DAL.Generic;
using PhotoAppApi.Services.Users.JWT;

namespace PhotoAppApi
{
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        JWTOptions jwtOptions = new();
                        Configuration.GetSection(JWTOptions.Position).Bind(jwtOptions);


                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            
                            ValidateIssuer = true,
                            ValidIssuer = jwtOptions.ISSUER,

                            ValidateAudience = true,
                            ValidAudience = jwtOptions.AUDIENCE,

                            ValidateLifetime = true,

                            IssuerSigningKey = JWTConstructor.GetSymmetricSecurityKey(jwtOptions.KEY),
                            ValidateIssuerSigningKey = true,
                        };
                    });

            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowAll",
                                  builder =>
                                  {
                                      var webAppUrl = Configuration["WebAppUrl"];
                                      builder.WithOrigins(webAppUrl)
                                             .AllowAnyHeader()
                                             .AllowAnyMethod()
                                             .AllowAnyHeader()
                                             .AllowCredentials();
                                  });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PhotoAppApi", Version = "v1" });
            });

            services.Configure<JWTOptions>(Configuration.GetSection(
                                        JWTOptions.Position));

            services.AddHttpContextAccessor();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });

            services.AddHostedService<BackgroundServiceDeleteUncomfirmedUsers>();

            services.AddScoped<UserService>();
            services.AddScoped<PostService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(PhotoService<>));
            services.AddScoped<PostCommentService>();
            services.AddSingleton<EmailService>();
            services.AddSingleton<JWTConstructor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PhotoAppApi v1"));
            }

            app.UseHttpsRedirection();


            app.UseCors("AllowAll");

            app.UseRouting();


            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
