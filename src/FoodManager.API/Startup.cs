using System.Net.Mime;
using System.Reflection;
using FluentValidation;
using FoodManager.Application.Common.Exceptions;
using FoodManager.Application.Orders.Commands;
using FoodManager.Application.Foods.Commands;
using FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery;
using FoodManager.Application.Foods.Queries.GetFoodByIdQuery;
using FoodManager.Domain.Extensions;
using FoodManager.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using FoodManager.Application.Orders.Queries;
using FoodManager.Domain.Models;
using FoodManager.Application.Orders.Commands.Validatons;
using FoodManager.Application.Users.Queries;
using FoodManager.API.Extensions;
using FoodManager.Application.Mappings;

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

        services.AddDependencyInjections();


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
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

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
