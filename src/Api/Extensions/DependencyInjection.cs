using FluentValidation;
using Api.Firebase;
using Api.Services;
using Application.Carts.Commands;
using Application.Carts.Commands.Factories;
using Application.Carts.Queries;
using Application.Foods.Commands;
using Application.Foods.Queries.FoodGetListPaginationQuery;
using Application.Orders.Commands;
using Application.Orders.Commands.Validatons;
using Application.Orders.Queries;
using Application.Users.Commands;
using Application.Users.Queries;
using Domain.Enums;
using Domain.Extensions;
using Domain.Models;
using MediatR;
using Integrations.Settings;
using Domain.Interfaces;
using Integrations.MercadoPago;
using Application.Payment.Commands;
using Application.Carts.Dtos;
using Api.Workflows.Workflows;
using Integrations.SMTP;
using Api.Workflows.JobSchedulerService;

namespace Api.Extensions;

public static class MyConfigServiceCollectionExtensions
{
    public static IServiceCollection AddDependencyInjections(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<FirebaseService>();


        // configure 
        services.Configure<MercadoPagoSettings>(configuration.GetSection("MercadoPago"));
        services.Configure<SmtpServicesSettings>(configuration.GetSection("SmtpServicesSettings"));

        // region Firebase 
        var firebaseService = new FirebaseService(configuration);
        services.AddSingleton<FirebaseAuthService>();
        services.AddScoped<ICurrentUser, HttpCurrentUser>();
        // #endregion

        // #region FluentValidations
        services.AddValidatorsFromAssemblyContaining<FoodCreateValidations>();
        services.AddValidatorsFromAssemblyContaining<OrderCreateValidations>();
        // #endregion


        // #region Integrations
        services.AddScoped<IPaymentCommunication, PaymentCommunication>();
        services.AddScoped<IMercadoPagoClient, MercadoPagoClient>();
        services.AddHttpClient<IMercadoPagoClient, MercadoPagoClient>();
        // #endregion

        // #region Commands
        services.AddTransient<IRequestHandler<FoodCreateCommand, bool>, FoodCreateHandler>();
        services.AddTransient<IRequestHandler<FoodDeleteCommand, bool>, FoodDeleteHandler>();
        services.AddTransient<IRequestHandler<FoodUpdateCommand, bool>, FoodUpdateHandler>();
        services.AddTransient<IRequestHandler<OrderCreateCommand, bool>, OrderCreateHandler>();
        services.AddTransient<IRequestHandler<OrderDeleteCommand, bool>, OrderDeleteHandler>();
        services.AddTransient<IRequestHandler<UserCreateCommand, User>, UserCreateCommandHandler>();
        services.AddTransient<IRequestHandler<UserUpdateCommand, bool>, UserUpdateCommandHandler>();
        services.AddTransient<IRequestHandler<CartCreateCommand, bool>, CartCreateCommandHandler>();
        services.AddTransient<IRequestHandler<CartDeleteCommand, bool>, CartDeleteCommandHandler>();
        services.AddTransient<IRequestHandler<MergeUsersFirebaseCommand, bool>, MergeUsersFirebaseCommandHandler>();
        services.AddTransient<IRequestHandler<CreatePaymentCommand, string>, CreatePaymentCommandHandler>();

        // #endregion

        // #region Queries
        services.AddTransient<IRequestHandler<FoodGetListPaginationQuery, ListDataResponse<List<Food>>>, FoodGetListPaginationQueryHandler>();
        services.AddTransient<IRequestHandler<OrderPaginationListQuery, ListDataResponse<List<Order>>>, OrderPaginationListHandler>();
        services.AddTransient<IRequestHandler<OrderGetByIdQuery, Order>, OrderGetByIdHandler>();
        services.AddTransient<IRequestHandler<UserPaginationListQuery, ListDataResponse<List<User>>>, UserPaginationListQueryHandler>();
        services.AddTransient<IRequestHandler<UserGetByIdQuery, User>, UserGetByIdQueryHandler>();
        services.AddTransient<IRequestHandler<VerifyUserIsMasterQuery, bool>, VerifyUserIsMasterQueryHandler>();
        services.AddTransient<IRequestHandler<CartGetListQuery, CartListDto>, CartGetListQueryHandler>();
        // #endregion

        services.AddScoped<ISmtpService, SmtpServices>();

        // #region Workflows, Jobs and Activities
        services.AddScoped<IProcessMergeUsersFirebaseActivity, ProcessMergeUsersFirebaseActivity>();
        services.AddScoped<IMergeUsersWorkflow, MergeUsersWorkflow>();
        services.AddScoped<IJobSchedulerService, JobSchedulerService>();
        // #endregion

        // #region Factories
        services.AddScoped<ICartFactory, CartFactory>();
        // #endregion

        return services;
    }
}
