using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Runtime.Serialization;
using System.Text;
using DotNetNuke.Common.Utilities;

namespace DotNetNuclear.DNN.Connectors.Reddit.Components
{
    public class RedditApiClient
    {
        private string _accessToken;

        public string AccessToken
        {
            get
            {
                if (String.IsNullOrEmpty(_accessToken))
                {
                    if (System.Web.HttpContext.Current.Session[Constants.RedditApiTokenCookieName] != null)
                    {
                        // If the cookie is encrypted, here is where we would decrypt
                        _accessToken = System.Web.HttpContext.Current.Session[Constants.RedditApiTokenCookieName].ToString();
                    }
                }
                return _accessToken;
            }
        }

        public RedditApiClient(string accessToken)
        {
            _accessToken = accessToken;
        }

        public AccountData MyAccount()
        {
            try
            {
                return ApiRequest<AccountData>("GET", "me", null);
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
                return null;
            }
        }

        private T ApiRequest<T>(string verb, string path, NameValueCollection request) where T : new()
        {
            var result = new T();
            try
            {
                var wr = WebRequest.CreateDefault(GenerateRequestUri(path, ""));
                var myHttpWebRequest = (HttpWebRequest)wr;
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.UserAgent = "DnnConnector/0.1 by DotNetNuclear";
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + AccessToken);
                myHttpWebRequest.Accept = "application/json";

                if (verb == "POST")
                {
                    var postData = Encoding.ASCII.GetBytes(request.ToString());

                    myHttpWebRequest.Method = "POST";
                    myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                    myHttpWebRequest.ContentLength = postData.Length;
                    using (var stream = myHttpWebRequest.GetRequestStream())
                    {
                        stream.Write(postData, 0, postData.Length);
                    }
                }
                using (WebResponse myWebResponse = myHttpWebRequest.GetResponse())
                {
                    using (Stream responseStream = myWebResponse.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (var responseReader = new StreamReader(responseStream))
                            {
                                var userdata = responseReader.ReadToEnd();

                                result = Json.Deserialize<T>(userdata);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
            }
            return result;
        }

        private Uri GenerateRequestUri(string apiPath, string parameters)
        {
            if (string.IsNullOrEmpty(parameters))
            {
                return new Uri($"{Constants.RedditOAuthApiBaseUrl}/{apiPath}");
            }
            return new Uri($"{Constants.RedditOAuthApiBaseUrl}/{apiPath}?{parameters}");
        }
    }

    [DataContract]
    public class AccountData
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "inbox_count")]
        public int InboxCount { get; set; }

        [DataMember(Name = "link_karma")]
        public int LinkKarma { get; set; }

        [DataMember(Name = "over_18")]
        public bool IsOver18 { get; set; }

        [DataMember(Name = "is_gold")]
        public bool IsGoldMember { get; set; }

        [DataMember(Name = "is_mod")]
        public bool IsModerator { get; set; }

        [DataMember(Name = "verified")]
        public bool IsVerified { get; set; }

        [DataMember(Name = "created")]
        public double Created { get; set; }
    }

}
