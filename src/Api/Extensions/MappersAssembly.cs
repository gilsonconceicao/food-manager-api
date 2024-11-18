using Application.Mappings;

namespace Api.Extensions;

public static class MapperAssembly
{
    public static IServiceCollection AddMappersConfigs(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(FoodMappers).Assembly);
        services.AddAutoMapper(typeof(OrderMappers).Assembly);
        services.AddAutoMapper(typeof(UserMappers).Assembly);
        services.AddAutoMapper(typeof(OrderItemsMapper).Assembly);
        services.AddAutoMapper(typeof(AddressMappers).Assembly);

        return services;
    }
}