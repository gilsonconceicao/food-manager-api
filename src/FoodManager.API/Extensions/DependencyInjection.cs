using FluentValidation;
using FoodManager.API.Firebase;
using FoodManager.API.Services;
using FoodManager.Application.Foods.Commands;
using FoodManager.Application.Foods.Queries.FoodGetListPaginationQuery;
using FoodManager.Application.Foods.Queries.GetFoodByIdQuery;
using FoodManager.Application.Orders.Commands;
using FoodManager.Application.Orders.Commands.Validatons;
using FoodManager.Application.Orders.Queries;
using FoodManager.Application.Users.Commands;
using FoodManager.Application.Users.Queries;
using FoodManager.Domain.Enums;
using FoodManager.Domain.Extensions;
using FoodManager.Domain.Models;
using MediatR;

namespace FoodManager.API.Extensions;
public static class MyConfigServiceCollectionExtensions
{
    public static IServiceCollection AddDependencyInjections(
        this IServiceCollection services
    )
    {
        // firebase 
        var firebaseService = new FirebaseService();
        services.AddSingleton<FirebaseAuthService>();
        services.AddScoped<ITokenService, TokenService>();

        // #region FluentValidations
        services.AddValidatorsFromAssemblyContaining<FoodCreateValidations>();
        services.AddValidatorsFromAssemblyContaining<OrderCreateValidations>();

        // #region Commands
        services.AddTransient<IRequestHandler<FoodCreateCommand, bool>, FoodCreateHandler>();
        services.AddTransient<IRequestHandler<FoodDeleteCommand, bool>, FoodDeleteHandler>();
        services.AddTransient<IRequestHandler<FoodUpdateCommand, bool>, FoodUpdateHandler>();
        services.AddTransient<IRequestHandler<OrderCreateCommand, bool>, OrderCreateHandler>();
        services.AddTransient<IRequestHandler<OrderDeleteCommand, bool>, OrderDeleteHandler>();
        services.AddTransient<IRequestHandler<UserCreateCommand, User>, UserCreateCommandHandler>();
        services.AddTransient<IRequestHandler<UserUpdateCommand, bool>, UserUpdateCommandHandler>();
        services.AddTransient<IRequestHandler<ExecuteTriggerCommand, OrderStatus>, ExecuteTriggerCommandHandler>();

        // #region Queries
        services.AddTransient<IRequestHandler<FoodGetListPaginationQuery, ListDataResponse<List<Food>>>, FoodGetListPaginationQueryHandler>();
        services.AddTransient<IRequestHandler<GetFoodByIdQuery, Food>, GetFoodByIdHandler>();
        services.AddTransient<IRequestHandler<OrderPaginationListQuery, ListDataResponse<List<Order>>>, OrderPaginationListHandler>();
        services.AddTransient<IRequestHandler<OrderGetByIdQuery, Order>, OrderGetByIdHandler>();
        services.AddTransient<IRequestHandler<UserPaginationListQuery, ListDataResponse<List<User>>>, UserPaginationListQueryHandler>();
        services.AddTransient<IRequestHandler<UserGetByIdQuery, User>, UserGetByIdQueryHandler>();
        services.AddTransient<IRequestHandler<UserGetByRegistrationNumberQuery, User>, UserGetByRegistrationNumberHandler>();
        services.AddTransient<IRequestHandler<VerifyUserIsMasterQuery, bool>, VerifyUserIsMasterQueryHandler>();

        return services;
    }
}
