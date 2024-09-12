using FluentValidation;
using FoodManager.Application.Foods.Commands;
using FoodManager.Application.Foods.Queries.FoodGetListPaginationQuery;
using FoodManager.Application.Foods.Queries.GetFoodByIdQuery;
using FoodManager.Application.Orders.Commands;
using FoodManager.Application.Orders.Commands.Validatons;
using FoodManager.Application.Orders.Queries;
using FoodManager.Application.Users.Commands;
using FoodManager.Application.Users.Queries;
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
        // #region FluentValidations
        services.AddValidatorsFromAssemblyContaining<FoodCreateValidations>();
        services.AddValidatorsFromAssemblyContaining<OrderCreateValidations>();

        // #region Commands
        services.AddTransient<IRequestHandler<FoodCreateCommand, bool>, FoodCreateHandler>();
        services.AddTransient<IRequestHandler<FoodDeleteCommand, bool>, FoodDeleteHandler>();
        services.AddTransient<IRequestHandler<FoodUpdateCommand, bool>, FoodUpdateHandler>();
        services.AddTransient<IRequestHandler<OrderCreateCommand, bool>, OrderCreateHandler>();
        services.AddTransient<IRequestHandler<OrderDeleteCommand, bool>, OrderDeleteHandler>();
        services.AddTransient<IRequestHandler<UserCreateCommand, Guid>, UserCreateCommandHandler>();

        // #region Queries
        services.AddTransient<IRequestHandler<FoodGetListPaginationQuery, ListDataResponse<List<Food>>>, FoodGetListPaginationQueryHandler>();
        services.AddTransient<IRequestHandler<GetFoodByIdQuery, Food>, GetFoodByIdHandler>();
        services.AddTransient<IRequestHandler<OrderPaginationListQuery, ListDataResponse<List<Order>>>, OrderPaginationListHandler>();
        services.AddTransient<IRequestHandler<OrderGetByIdQuery, Order>, OrderGetByIdHandler>();
        services.AddTransient<IRequestHandler<UserPaginationListQuery, ListDataResponse<List<User>>>, UserPaginationListQueryHandler>();
        services.AddTransient<IRequestHandler<UserGetByIdQuery, User>, UserGetByIdQueryHandler>();

        return services;
    }
}
