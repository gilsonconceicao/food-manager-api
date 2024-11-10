using FirebaseAdmin.Auth;
using FoodManager.API.Firebase;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace FoodManager.API.Services
{
    public class UserInfoResponse
    {
        public string UserId { get; set; }
        public string CurrentToken { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
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

            // Verifica o token com FirebaseAuthService
            var decodedToken = await _firebaseAuthService.VerifyTokenAsync(idToken);

            if (decodedToken == null)
            {
                throw new UnauthorizedAccessException("Token inválido.");
            }

            // Cria a resposta com as informações do usuário
            var userInfoResponse = new UserInfoResponse
            {
                UserId = decodedToken.Uid,
                CurrentToken = idToken,
                Name = decodedToken.Claims.FirstOrDefault(x =>x.Key == "email").Value.ToString()!,
                Email = decodedToken.Claims.FirstOrDefault(x =>x.Key == "name").Value.ToString()!,
            };

            return userInfoResponse;
        }
    }
}
