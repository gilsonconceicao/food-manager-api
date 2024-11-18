using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
namespace Api.Extensions;

public class FirebaseService
{
    public FirebaseService()
    {
        if (FirebaseApp.DefaultInstance == null)
        {
            try
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(@"firebasesettings.json")
                });
                Console.WriteLine("Sucesso ao ler arquivo.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
