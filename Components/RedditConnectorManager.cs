using System.Collections.Generic;

namespace DotNetNuclear.DNN.Connectors.Reddit.Components
{
    public class RedditConnectorManager : IRedditConnectionManager
    {
        private readonly int _portalId;
        private readonly RedditConnector _RedditConnector;
        private readonly IDictionary<string, string> _config;

        #region Properties

        public int PortalId
        {
            get { return _portalId; }
            set { }
        }
        public bool HasConfig
        {
            get
            {
                return _RedditConnector.HasConfig(_portalId);
            }
        }

        public string AuthenticationTypeName
        {
            get
            {
                return _RedditConnector.Name;
            }
        }

        public string AuthorizationEndpoint
        {
            get { return $"{Constants.RedditApiBaseUrl}/authorize"; }
        }
        public string AccessTokenEndpoint
        {
            get { return $"{Constants.RedditApiBaseUrl}/access_token"; }
        }
        public string MeGraphEndpoint
        {
            get { return $"{Constants.RedditApiBaseUrl}/me.json"; }
        }
        public string AccessTokenCookieName
        {
            get { return Constants.RedditApiTokenCookieName; }
        }
        public string ClientId { get; }
        public string ClientSecret { get; }
        public string ApiScope { get; }

        #endregion

        #region constructors

        public RedditConnectorManager(int portalId)
        {
            _portalId = portalId;
            _RedditConnector = new RedditConnector();
            _config = _RedditConnector.GetConfig(portalId);
            ClientId = GetConnectorValue(Constants.RedditClientID);
            ClientSecret = GetConnectorValue(Constants.RedditClientSecret);
            ApiScope = GetConnectorValue(Constants.RedditApiScope);
        }

        #endregion

        #region private

        private string GetConnectorValue(string settingName)
        {
            return _config[settingName];
        }

        private bool GetConnectorValueToBool(string settingName)
        {
            try
            {
                return bool.Parse(_config[settingName]);
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        #endregion
    }
}
