using OktaOpeIDConnectDemo.Models;

namespace OktaOpeIDConnectDemo.Services
{
    public interface ITokenService
    {
        Task<OktaResponse> GetToken(string username, string password);
    }
}
