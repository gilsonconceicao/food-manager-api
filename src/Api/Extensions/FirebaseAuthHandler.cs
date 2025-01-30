using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Api.Firebase; 
public class FirebaseAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly FirebaseAuthService _firebaseAuthService;

    public FirebaseAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                               ILoggerFactory logger,
                               UrlEncoder encoder,
                               ISystemClock clock,
                               FirebaseAuthService firebaseAuthService) 
        : base(options, logger, encoder, clock)
    {
        _firebaseAuthService = firebaseAuthService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return AuthenticateResult.NoResult(); 

        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();

        try
        {
            var decodedToken = await _firebaseAuthService.VerifyTokenAsync(token);

            if (decodedToken == null)
            {
                return AuthenticateResult.Fail("Token inválido");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, decodedToken.Uid),
                new Claim(ClaimTypes.Name, decodedToken.Uid) // Exemplo: Use o UID como nome do usuário
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);

            return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
        }
        catch (Exception ex)
        {
            return AuthenticateResult.Fail($"Erro ao autenticar com Firebase: {ex.Message}");
        }
    }
}
