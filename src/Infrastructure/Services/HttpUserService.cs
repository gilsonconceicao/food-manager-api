using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Firebase;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;

#nullable disable

namespace Api.Services
{
    public interface ICurrentUser
    {
        Task<List<ExportedUserRecord>> GetExportedUserRecords();
        Task<UserInfoResponse> GetAuthenticatedUser();
        Task<UserRecord> GetUserByUserIdAsync(string userId);
    }

    public class HttpCurrentUser : ICurrentUser
    {

        private readonly FirebaseAuthService _firebaseAuthService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpCurrentUser(FirebaseAuthService firebaseAuthService, IHttpContextAccessor httpContextAccessor)
        {
            _firebaseAuthService = firebaseAuthService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserInfoResponse> GetAuthenticatedUser()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

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

                return new UserInfoResponse
                {
                    UserId = decodedToken.Uid,
                    CurrentToken = idToken,
                    Name = nameClaim ?? "",
                    Email = emailClaim ?? ""
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar o token: {ex.Message}");
                throw new UnauthorizedAccessException("Erro ao processar o token.", ex);
            }
        }

        public async Task<List<ExportedUserRecord>> GetExportedUserRecords()
        {
            var enumerator = FirebaseAuth.DefaultInstance.ListUsersAsync(null).GetAsyncEnumerator();
            var currentUsers = new List<ExportedUserRecord>();
      
            while (await enumerator.MoveNextAsync())
            {
                ExportedUserRecord user = enumerator.Current;
                currentUsers.Add(user); 
            }

            return currentUsers; 
        }

        public async Task<UserRecord> GetUserByUserIdAsync(string userId)
        {
            UserRecord userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(userId);
            return userRecord;
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
