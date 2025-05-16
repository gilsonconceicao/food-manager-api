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
using Domain.Extensions;
using Domain.Models;
using MediatR;
using Integrations.Settings;
using Domain.Interfaces;
using Integrations.MercadoPago;
using Application.Payment.Commands;
using Application.Carts.Dtos;
using Integrations.SMTP;
using Api.Workflows.JobSchedulerService;
using Application.Workflows.Workflows;
using Application.Workflows.Activities;
using Application.Foods.Queries.FoodGetByIdQuery;

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
        services.AddTransient<IRequestHandler<OrderCreateCommand, Guid>, OrderCreateHandler>();
        services.AddTransient<IRequestHandler<OrderDeleteCommand, bool>, OrderDeleteHandler>();
        services.AddTransient<IRequestHandler<UserCreateCommand, User>, UserCreateCommandHandler>();
        services.AddTransient<IRequestHandler<UserUpdateCommand, bool>, UserUpdateCommandHandler>();
        services.AddTransient<IRequestHandler<CartCreateCommand, bool>, CartCreateCommandHandler>();
        services.AddTransient<IRequestHandler<CartDeleteCommand, bool>, CartDeleteCommandHandler>();
        services.AddTransient<IRequestHandler<MergeUsersFirebaseCommand, bool>, MergeUsersFirebaseCommandHandler>();
        services.AddTransient<IRequestHandler<CreatePaymentCommand, string>, CreatePaymentCommandHandler>();
        services.AddTransient<IRequestHandler<ProcessMerchantOrderWebhookCommand, Unit>, ProcessMerchantOrderWebhookCommandHandler>();
        services.AddTransient<IRequestHandler<TryHangFireCommand, bool>, TryHangFireCommandHandler>();

        // #endregion

        // #region Queries
        services.AddTransient<IRequestHandler<FoodGetListPaginationQuery, ListDataResponse<List<Food>>>, FoodGetListPaginationQueryHandler>();
        services.AddTransient<IRequestHandler<OrderPaginationListQuery, ListDataResponse<List<Order>>>, OrderPaginationListHandler>();
        services.AddTransient<IRequestHandler<OrderGetByIdQuery, Order>, OrderGetByIdHandler>();
        services.AddTransient<IRequestHandler<UserPaginationListQuery, ListDataResponse<List<User>>>, UserPaginationListQueryHandler>();
        services.AddTransient<IRequestHandler<UserGetByIdQuery, User>, UserGetByIdQueryHandler>();
        services.AddTransient<IRequestHandler<VerifyUserIsMasterQuery, bool>, VerifyUserIsMasterQueryHandler>();
        services.AddTransient<IRequestHandler<CartGetListQuery, CartListDto>, CartGetListQueryHandler>();
        services.AddTransient<IRequestHandler<FoodGetByIdQuery, Food>, FoodGetByIdQueryHandler>();
        // #endregion

        services.AddScoped<ISmtpService, SmtpServices>();

        // #region Workflows, Jobs and Activities
        services.AddScoped<IJobSchedulerService, JobSchedulerService>();

        services.AddScoped<UpdateOrderStatusWorkflow>();
        services.AddScoped<OrderExpirationWorkflow>();
        
        services.AddScoped<ITryHangFireWorkflow, TryHangFireWorkflow>();
        services.AddScoped<IMergeUsersWorkflow, MergeUsersWorkflow>();
    
        services.AddScoped<IProcessTryHangFireActivity, ProcessTryHangFireActivity>();
        services.AddScoped<IProcessOrderExpirationActivity, ProcessOrderExpirationActivity>();
        services.AddScoped<IProcessMergeUsersFirebaseActivity, ProcessMergeUsersFirebaseActivity>();
        services.AddScoped<IUpdateOrderStatusActivity, UpdateOrderStatusActivity>();
        // #endregion

        // #region Factories
        services.AddScoped<ICartFactory, CartFactory>();
        // #endregion

        return services;
    }
}
