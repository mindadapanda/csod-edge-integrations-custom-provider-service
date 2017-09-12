using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;

namespace CsodATSCoreLib
{
    class CsodSTSRequestHandler
    {
        IRestClient _client;
        string _username;
        string _apiKey;
        string _apiSecret;
        CsodSTSSignature _sigProvider;
        public CsodSTSRequestHandler(IRestClient client, string username, string apiKey, string apiSecret, CsodSTSSignature sigProvider)
        {
            _client = client;
            _username = username;
            _apiKey = apiKey;
            _apiSecret = apiSecret;
            _sigProvider = sigProvider;
        }

        static bool _initialized;
        static string _sessionToken;
        static string _sessionTokenSecret;

        private bool Initialize()
        {
            if (!_initialized)
            {
                string alias = Guid.NewGuid().ToString();

                string stsUrl = string.Format(@"/services/api/sts/session?userName={0}&alias={1}", _username, alias);

                IRestRequest request = new RestRequest(stsUrl, Method.POST);

                NameValueCollection headers = new NameValueCollection();
                headers.Add("x-csod-date", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.000"));
                headers.Add("x-csod-api-key", _apiKey);

                Uri absoluteUrl = new Uri(string.Format("{0}{1}", _client.BaseUrl, request.Resource));

                string signature = _sigProvider.SignRequest(_apiSecret, request.Method.ToString(), headers, absoluteUrl.AbsolutePath);

                headers.Add("x-csod-signature", signature);


                foreach (var i in headers.AllKeys)
                {
                    request.AddHeader(i, headers[i]);
                }

                var tcs = new TaskCompletionSource<bool>();
                _client.ExecuteAsync(request, x =>
                {
                    if (x.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = JsonConvert.DeserializeObject<dynamic>(x.Content);
                        _sessionToken = result.data[0].Token;
                        _sessionTokenSecret = result.data[0].Secret;

                        tcs.SetResult(true);
                    }
                    else
                    {
                        tcs.SetResult(false);
                    }
                });

                _initialized = tcs.Task.Result;
            }

            return _initialized;
        }

        public dynamic ExecuteRequestJson(Method method, string url)
        {
            return JsonConvert.DeserializeObject<dynamic>(ExecuteRequest(method, url));
        }
        public string ExecuteRequest(Method method, string url)
        {
            if (Initialize())
            {
                IRestRequest request = new RestRequest(url, method);

                NameValueCollection headers = new NameValueCollection();
                headers.Add("x-csod-date", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.000"));
                headers.Add("x-csod-session-token", SessionToken);

                Uri absoluteUrl = new Uri(string.Format("{0}{1}", _client.BaseUrl, request.Resource));

                string signature = _sigProvider.SignRequest(SessionTokenSecret, request.Method.ToString(), headers, absoluteUrl.AbsolutePath);
                headers.Add("x-csod-signature", signature);

                headers.Add("content-type", "application/json");

                foreach (var i in headers.AllKeys)
                {
                    request.AddHeader(i, headers[i]);
                }

                var tcs = new TaskCompletionSource<string>();
                _client.ExecuteAsync(request, x =>
                {
                    if (x.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        tcs.SetResult(x.Content);
                    }
                    else
                    {
                        tcs.SetException(new Exception(string.Format("Something went wrong, please contact a developer. url:{0}", url)));
                    }
                });

                return tcs.Task.Result;
            }

            throw new InvalidOperationException();
        }

        public string SessionToken
        {
            get { return _sessionToken; }
        }

        public string SessionTokenSecret
        {
            get { return _sessionTokenSecret; }
        }
    }
}
