using FluentValidation;
using Api.Firebase;
using Api.Services;
using Application.Carts.Commands;
using Application.Carts.Commands.Factories;
using Application.Carts.Queries;
using Application.Foods.Commands;
using Application.Foods.Queries.FoodGetListPaginationQuery;
using Application.Foods.Queries.GetFoodByIdQuery;
using Application.Orders.Commands;
using Application.Orders.Commands.Validatons;
using Application.Orders.Queries;
using Application.Users.Commands;
using Application.Users.Queries;
using Domain.Enums;
using Domain.Extensions;
using Domain.Models;
using MediatR;

namespace Api.Extensions;
public static class MyConfigServiceCollectionExtensions
{
    public static IServiceCollection AddDependencyInjections(
        this IServiceCollection services
    )
    {
        // region Firebase 
        var firebaseService = new FirebaseService();
        services.AddSingleton<FirebaseAuthService>();
        services.AddScoped<IHttpUserService, HttpUserService>();
        // #endregion

        // #region FluentValidations
        services.AddValidatorsFromAssemblyContaining<FoodCreateValidations>();
        services.AddValidatorsFromAssemblyContaining<OrderCreateValidations>();
        // #endregion

        // #region Commands
        services.AddTransient<IRequestHandler<FoodCreateCommand, bool>, FoodCreateHandler>();
        services.AddTransient<IRequestHandler<FoodDeleteCommand, bool>, FoodDeleteHandler>();
        services.AddTransient<IRequestHandler<FoodUpdateCommand, bool>, FoodUpdateHandler>();
        services.AddTransient<IRequestHandler<OrderCreateCommand, bool>, OrderCreateHandler>();
        services.AddTransient<IRequestHandler<OrderDeleteCommand, bool>, OrderDeleteHandler>();
        services.AddTransient<IRequestHandler<UserCreateCommand, User>, UserCreateCommandHandler>();
        services.AddTransient<IRequestHandler<UserUpdateCommand, bool>, UserUpdateCommandHandler>();
        services.AddTransient<IRequestHandler<ExecuteTriggerCommand, OrderStatus>, ExecuteTriggerCommandHandler>();
        services.AddTransient<IRequestHandler<CartCreateCommand, bool>, CartCreateCommandHandler>();
        services.AddTransient<IRequestHandler<CreateUpdateCommand, bool>, CreateUpdateCommandHandler>();
        services.AddTransient<IRequestHandler<CartDeleteCommand, bool>, CartDeleteCommandHandler>();
        // #endregion

        // #region Queries
        services.AddTransient<IRequestHandler<FoodGetListPaginationQuery, ListDataResponse<List<Food>>>, FoodGetListPaginationQueryHandler>();
        services.AddTransient<IRequestHandler<GetFoodByIdQuery, Food>, GetFoodByIdHandler>();
        services.AddTransient<IRequestHandler<OrderPaginationListQuery, ListDataResponse<List<Order>>>, OrderPaginationListHandler>();
        services.AddTransient<IRequestHandler<OrderGetByIdQuery, Order>, OrderGetByIdHandler>();
        services.AddTransient<IRequestHandler<UserPaginationListQuery, ListDataResponse<List<User>>>, UserPaginationListQueryHandler>();
        services.AddTransient<IRequestHandler<UserGetByIdQuery, User>, UserGetByIdQueryHandler>();
        services.AddTransient<IRequestHandler<UserGetByRegistrationNumberQuery, User>, UserGetByRegistrationNumberHandler>();
        services.AddTransient<IRequestHandler<VerifyUserIsMasterQuery, bool>, VerifyUserIsMasterQueryHandler>();
        services.AddTransient<IRequestHandler<CartGetListQuery, List<Cart>>, CartGetListQueryHandler>();
        // #endregion

        // #region Factories
        services.AddScoped<ICartFactory, CartFactory>();
        // #endregion
        return services;
    }
}
