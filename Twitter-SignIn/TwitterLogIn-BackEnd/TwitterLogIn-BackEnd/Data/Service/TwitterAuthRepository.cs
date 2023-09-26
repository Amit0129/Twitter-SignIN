using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OAuth;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TwitterLogIn_BackEnd.Data.Interface;
using TwitterLogIn_BackEnd.Model;

namespace TwitterLogIn_BackEnd.Data.Service
{
    public class TwitterAuthRepository : ITwitterAuthRepository
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IOptions<TwitterSettings> _twitterConfig;
        //private readonly DataContext _context;

        public TwitterAuthRepository(IConfiguration config, IHttpClientFactory clientFactory, IOptions<TwitterSettings> twitterConfig)//, DataContext context
        {
            //_context = context;
            _twitterConfig = twitterConfig;
            _clientFactory = clientFactory;
            _config = config;

        }
        public async Task<RequestTokenResponse> GetRequestToken()
        {
            var requestTokenResponse = new RequestTokenResponse();

            var client = _clientFactory.CreateClient("twitter");
            var consumerKey = _twitterConfig.Value.AppId;
            var consumerSecret = _twitterConfig.Value.AppSecret;
            var callbackUrl = "http://localhost:4200/login";

            client.DefaultRequestHeaders.Accept.Clear();
            var oauthClient = new OAuthRequest()
            {
                Method = "POST",
                Type = OAuthRequestType.RequestToken,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                RequestUrl = "https://api.twitter.com/oauth/request_token",
                Version = "1.0a",
                Realm = "twitter.com",
                CallbackUrl = callbackUrl
            };
            string auth = oauthClient.GetAuthorizationHeader();
            client.DefaultRequestHeaders.Add("Authorization", auth);

            try
            {
                var content = new StringContent("", Encoding.UTF8, "application/json");

                using (var response = await client.PostAsync(oauthClient.RequestUrl, content))
                {
                    response.EnsureSuccessStatusCode();
                    var responseString = response.Content.ReadAsStringAsync().Result.Split("&");

                    requestTokenResponse = new RequestTokenResponse
                    {
                        oauth_token = responseString[0],
                        oauth_token_secret = responseString[1],
                        oauth_callback_confirmed = responseString[2]
                    };
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
            return requestTokenResponse;
        }
    }
}
