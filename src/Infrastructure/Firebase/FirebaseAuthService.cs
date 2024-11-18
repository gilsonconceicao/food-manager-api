using FirebaseAdmin.Auth;
namespace Api.Firebase;
public class FirebaseAuthService
{
    public async Task<FirebaseToken> VerifyTokenAsync(string idToken)
    {
        try
        {
            var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
            return decodedToken;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao verificar o token: {ex.Message}");
            return null;
        }
    }
}
