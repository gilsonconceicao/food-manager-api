using FoodManager.API.Firebase;

#nullable disable

namespace FoodManager.API.Services
{
    public interface ITokenService
    {
        Task<UserInfoResponse> VerifyTokenFromHeaderAsync(HttpRequest request);
    }

    public class TokenService : ITokenService
    {
        private readonly FirebaseAuthService _firebaseAuthService;

        public TokenService(FirebaseAuthService firebaseAuthService)
        {
            _firebaseAuthService = firebaseAuthService;
        }
        public async Task<UserInfoResponse> VerifyTokenFromHeaderAsync(HttpRequest request)
        {
            var authorizationHeader = request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                throw new UnauthorizedAccessException("Cabeçalho Authorization não fornecido ou inválido.");
            }

            var idToken = authorizationHeader.Substring("Bearer ".Length).Trim();

            var decodedToken = await _firebaseAuthService.VerifyTokenAsync(idToken);

            if (decodedToken == null)
            {
                throw new UnauthorizedAccessException("Token inválido.");
            }

            try
            {
                var emailClaim = decodedToken.Claims.FirstOrDefault(x => x.Key == "email").Value?.ToString();
                var nameClaim = decodedToken.Claims.FirstOrDefault(x => x.Key == "name").Value?.ToString();

                var userInfoResponse = new UserInfoResponse
                {
                    UserId = decodedToken.Uid,
                    CurrentToken = idToken,
                    Name = nameClaim ?? "", 
                    Email = emailClaim ?? ""
                };
                return userInfoResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar o token: {ex.Message}");
                throw new UnauthorizedAccessException("Erro ao processar o token.", ex); // Lança a exceção para ser tratada no ponto de chamada
            }
        }

    }
    public class UserInfoResponse
    {
        public string UserId { get; set; }
        public string CurrentToken { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
