
using FluentValidation;
using LSC.MentorByKarthik.Application;
using LSC.MentorByKarthik.Application.DTOValidations;
using LSC.MentorByKarthik.Application.Interfaces;
using LSC.MentorByKarthik.Application.Services;
using LSC.MentorByKarthik.Infrastructure;
using LSC.SmartCertify.API.Filters;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using System.Net;
using System.Text.Json.Serialization;
using TodoApp.WebAPI.Filters;

namespace LSC.MentorByKarthik.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //use this for real database on your sql server
            builder.Services.AddDbContext<MentorByKarthikContext>(options =>
            {
                options.UseSqlServer(
                builder.Configuration.GetConnectionString("DbContext"),
                providerOptions => providerOptions.EnableRetryOnFailure()
                ).EnableSensitiveDataLogging().EnableDetailedErrors();
            }
              );

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>(); // Add your custom validation filter
                options.Filters.Add<GlobalExceptionFilter>();
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true; // Disable automatic validation
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddOpenApi();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IMentoringSlotService, MentoringSlotService>();
            builder.Services.AddScoped<IMentoringSlotRepository, MentoringSlotRepository>();
            builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            builder.Services.AddScoped<IUserProfileService, UserProfileService>();
            builder.Services.AddScoped<IUserClaims, UserClaims>();

            // Add FluentValidation
            builder.Services.AddValidatorsFromAssemblyContaining<CreateSlotValidator>();
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // In production, modify this with the actual domains you want to allow
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseCors("default");

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature?.Error;

                    Log.Error(exception, "Unhandled exception occurred. {ExceptionDetails}", exception?.ToString());
                    Console.WriteLine(exception?.ToString());
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
                });
            });

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options.WithTitle("My API");
                    options.WithTheme(ScalarTheme.BluePlanet);
                    options.WithSidebar(true);
                });

                app.UseSwaggerUi(options =>
                {
                    options.DocumentPath = "openapi/v1.json";
                });

            }

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}
