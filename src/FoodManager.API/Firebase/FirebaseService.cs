using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
namespace FoodManager.API.Extensions;

public class FirebaseService
{
    public FirebaseService()
    {
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(@"firebasesettings.json")
            });
        }
    }
}
