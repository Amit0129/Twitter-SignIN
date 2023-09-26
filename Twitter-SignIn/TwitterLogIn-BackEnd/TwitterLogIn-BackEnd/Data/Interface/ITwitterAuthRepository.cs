using System.Threading.Tasks;
using TwitterLogIn_BackEnd.Model;

namespace TwitterLogIn_BackEnd.Data.Interface
{
    public interface ITwitterAuthRepository
    {
        Task<RequestTokenResponse> GetRequestToken();
    }
}
