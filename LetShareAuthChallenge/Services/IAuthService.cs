namespace LetShareAuthChallenge.Services
{
    public interface IAuthService
    {
        Task<(string AccessToken, string RefreshToken)> AuthenticateAsync(string username, string password);
    }
}
