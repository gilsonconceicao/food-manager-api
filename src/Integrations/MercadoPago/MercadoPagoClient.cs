using MercadoPago.Config;
namespace Integrations.MercadoPago;

public interface IMercadoPagoClient
{
    void ConfigureAccessToken(string accessToken);
}

public class MercadoPagoClient : IMercadoPagoClient
{
    public void ConfigureAccessToken(string accessToken)
    {
        MercadoPagoConfig.AccessToken = accessToken;
    }
}
