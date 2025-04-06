using System.Net.Mime;
using System.Reflection;
using Application.Common.Exceptions;
using Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Api.Extensions;
using Microsoft.AspNetCore.Authentication;
using Api.Firebase;
using Hangfire;
using Hangfire.PostgreSql;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public void ConfigureServices(IServiceCollection services)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection")!;
        services.AddEndpointsApiExplorer();
        // mediatR to CQRS of application
        services.AddMediatR(Assembly.GetExecutingAssembly());
        // mappers
        services.AddMappersConfigs();
        // database
        services.AddDbContext<DataBaseContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddDependencyInjections(_configuration);

        string connectionStringHangfire = _configuration.GetConnectionString("HangfireConnection")!;

        // #region Hangfire
        services.AddHangfire(configuration => configuration
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseColouredConsoleLogProvider()
            .UsePostgreSqlStorage(connectionStringHangfire, new PostgreSqlStorageOptions
            {
                SchemaName = "hangfire"
            }));
        // #endregion



        // Add controllers with NewtonsoftJson for handling JSON serialization
        services.AddControllers(options =>
            options.Filters.Add(new HttpResponseExceptionFilter())
        )
        .ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var result = new BadRequestObjectResult(context.ModelState);

                result.ContentTypes.Add(MediaTypeNames.Application.Json);
                result.ContentTypes.Add(MediaTypeNames.Application.Xml);
                options.SuppressConsumesConstraintForFormFileParameters = true;
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressMapClientErrors = true;
                options.ClientErrorMapping[StatusCodes.Status404NotFound].Link = "https://httpstatuses.com/404";
                options.DisableImplicitFromServicesParameters = true;
                return result;
            };
        })
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Food-Manager-API",
                Description = "Sisteme de gerencimaneto de comida",
            });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });
            options.SchemaFilter<SchemeFilterSwashbuckle>();
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        });

        services.AddAuthentication("Bearer")
            .AddScheme<AuthenticationSchemeOptions, FirebaseAuthHandler>("Bearer", options => { });

        services.AddHttpContextAccessor();
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Auth", policy => policy
                .RequireAuthenticatedUser()
            );
        });

        services.AddHangfireServer();
        services.AddApplicationInsightsTelemetry();


        var postgreSql = GetPostgreSql(services);
        var logger = GetLogger(services);

        try
        {
            postgreSql.MigrateAsync().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Error on migration");
            Console.WriteLine("Não foi possível concluir a migração do DB." + ex.ToString());
            throw;
        }
    }

    public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env
        )
    {

        app.UseSwagger(o =>
        {
            o.RouteTemplate = "docs/{documentName}/docs.json";
        });
        app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = "docs";
            c.SwaggerEndpoint("/docs/v1/docs.json", "FoodAPI");

            c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        });

#if DEBUG
        app.UseDeveloperExceptionPage();
#endif

        app.UseRouting();
        app.UseCors(policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHangfireDashboard();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });


    }

    private static DataBaseContext GetPostgreSql(IServiceCollection services)
    {
        return (DataBaseContext)services.BuildServiceProvider().GetService(typeof(DataBaseContext))!;
    }

    private static ILogger<Startup> GetLogger(IServiceCollection services)
    {
        return (ILogger<Startup>)services.BuildServiceProvider().GetService(typeof(ILogger<Startup>))!;
    }

}
