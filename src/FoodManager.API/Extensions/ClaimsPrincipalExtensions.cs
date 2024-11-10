using System.Security.Claims;
namespace FoodManager.API.Extensions; 

public static class ClaimsPrincipalExtensions
{
    public static FirebaseUserInfo GetFirebaseUserInfo(this ClaimsPrincipal user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = user.FindFirstValue(ClaimTypes.Name);

        // Verifique se o usuário está autenticado e tem um UID
        if (string.IsNullOrEmpty(userId))
        {
            return null;
        }

        return new FirebaseUserInfo
        {
            UserId = userId,
            UserName = userName
        };
    }
}

public class FirebaseUserInfo
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    // Adicione outros campos conforme necessário
}
