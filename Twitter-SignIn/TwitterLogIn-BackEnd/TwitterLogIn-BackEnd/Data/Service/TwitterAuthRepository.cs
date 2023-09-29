using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TwitterLogIn_BackEnd.Context;
using TwitterLogIn_BackEnd.Data.Interface;
using TwitterLogIn_BackEnd.Model;

namespace TwitterLogIn_BackEnd.Data.Service
{
    public class TwitterAuthRepository : ITwitterAuthRepository
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IOptions<TwitterSettings> _twitterConfig;
        private readonly DataContext _context;

        public TwitterAuthRepository(IConfiguration config, IHttpClientFactory clientFactory, IOptions<TwitterSettings> twitterConfig, DataContext context)//
        {
            _context = context;
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
            var callbackUrl = "http://localhost:4200/twitter-login";

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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return requestTokenResponse;
        }


        //Get Access Token
        public async Task<UserModelDto> GetAccessToken(string token, string oauthVerifier)
        {
            var client = _clientFactory.CreateClient("twitter");
            var consumerKey = _twitterConfig.Value.AppId;
            var consumerSecret = _twitterConfig.Value.AppSecret;

            var accessTokenResponse = new UserModelDto();

            client.DefaultRequestHeaders.Accept.Clear();

            var oauthClient = new OAuthRequest
            {
                Method = "POST",
                Type = OAuthRequestType.AccessToken,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                RequestUrl = "https://api.twitter.com/oauth/access_token",
                Token = token,
                Version = "1.0a",
                Realm = "twitter.com"
            };

            string auth = oauthClient.GetAuthorizationHeader();

            client.DefaultRequestHeaders.Add("Authorization", auth);


            try
            {
                var content = new FormUrlEncodedContent(new[]{
                new KeyValuePair<string, string>("oauth_verifier", oauthVerifier)
            });

                using (var response = await client.PostAsync(oauthClient.RequestUrl, content))
                {
                    response.EnsureSuccessStatusCode();

                    //twiiter responds with a string concatenated by &
                    var responseString = response.Content.ReadAsStringAsync()
                                               .Result.Split("&");

                    //split by = to get actual values
                    var length = responseString.Length;
                    accessTokenResponse = new UserModelDto
                    {
                        Token = responseString[0].Split("=")[1],
                        TokenSecret = responseString[1].Split("=")[1],
                        UserId = responseString[2].Split("=")[1],
                        Username = responseString[3].Split("=")[1]
                    };
                    var userAllInfo = new UserInfoModel
                    {
                        Token = responseString[0].Split("=")[1],
                        TokenSecret = responseString[1].Split("=")[1],
                        UserId = responseString[2].Split("=")[1],
                        Username = responseString[3].Split("=")[1],

                    };
                    var userInfo = _context.UserAllInfo.FirstOrDefault(x => x.UserId == userAllInfo.UserId);
                    if (userInfo == null)
                    {
                        _context.UserAllInfo.Add(userAllInfo);
                        _context.SaveChanges();
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return accessTokenResponse;
        }

    }
}
