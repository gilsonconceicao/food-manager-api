using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Api.Extensions
{
    public class FirebaseService
    {
        public FirebaseService(IConfiguration configuration)
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                try
                {
                    var firebaseConfig = configuration.GetSection("FirebaseSettings").Get<FirebaseSettings>();

                    var jsonConfig = JsonSerializer.Serialize(new
                    {
                        type = firebaseConfig.Type,
                        project_id = firebaseConfig.ProjectId,
                        private_key_id = firebaseConfig.PrivateKeyId,
                        private_key = firebaseConfig.PrivateKey.Replace("\\n", "\n"),
                        client_email = firebaseConfig.ClientEmail,
                        client_id = firebaseConfig.ClientId,
                        auth_uri = firebaseConfig.AuthUri,
                        token_uri = firebaseConfig.TokenUri,
                        auth_provider_x509_cert_url = firebaseConfig.AuthProviderX509CertUrl,
                        client_x509_cert_url = firebaseConfig.ClientX509CertUrl,
                        universe_domain = firebaseConfig.UniverseDomain
                    });


                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromJson(jsonConfig)
                    });

                    
                    Console.WriteLine("Firebase inicializado com sucesso." + new
                    {
                        variables = configuration.GetSection("FirebaseSettings"), 
                        firebaseConfig = firebaseConfig, 
                        jsonConfig = jsonConfig
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao inicializar o Firebase: {ex}");
                }
            }
        }
    }
}
