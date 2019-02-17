using System;
using System.Collections.Generic;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Connections;

namespace DotNetNuclear.DNN.Connectors.Reddit.Components
{
    public class RedditConnector : IConnector
    {

        #region Properties

        public string Name
        {
            get { return "Reddit"; }
        }

        public string DisplayName
        {
            get { return "Reddit"; }
        }

        public string IconUrl
        {
            get { return "~/DesktopModules/Connectors/Reddit/Images/Reddit.png"; }
        }

        public string PluginFolder
        {
            get { return "~/DesktopModules/Connectors/Reddit/"; }
        }

        #endregion

        private const string AuthenticationCacheKey = "Authentication.Reddit_";

        #region Public Methods
        public bool IsEngageConnector
        {
            get { return false; }
        }

        public string Id { get; set; }

        public ConnectorCategories Type => ConnectorCategories.Other;

        public bool SupportsMultiple => false;

        string IConnector.DisplayName
        {
            get
            {
                return (DisplayName);
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool HasConfig(int portalId)
        {
            var configs = GetConfig(portalId);
            string errorMessage;
            return IsValidated(configs, out errorMessage);
        }

        public IDictionary<string, string> GetConfig(int portalId)
        {
            var configs = new Dictionary<string, string>();

            var settings = PortalController.Instance.GetPortalSettings(portalId);
            GetValue(settings, configs, Constants.RedditClientID);
            GetValue(settings, configs, Constants.RedditClientSecret);
            GetValue(settings, configs, Constants.RedditApiScope, true, Constants.RedditApiScope_Default);

            return configs;
        }

        public bool SaveConfig(int portalId, IDictionary<string, string> values, ref bool validated, out string customErrorMessage)
        {
            validated = true;
            try
            {
                SetSetting(portalId, values, Constants.RedditClientID);
                SetSetting(portalId, values, Constants.RedditClientSecret);
                SetSetting(portalId, values, Constants.RedditApiScope);

                DataCache.RemoveCache(AuthenticationCacheKey + portalId);

                if (!IsValidated(values, out customErrorMessage))
                {
                    validated = false;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                customErrorMessage = ex.Message;
                return false;
            }
        }

        #endregion

        #region validation 

        private bool IsValidated(IDictionary<string, string> values, out string customErrorMessage)
        {
            customErrorMessage = "";
            var ClientId = values[Constants.RedditClientID];
            var ClientSecret = values[Constants.RedditClientSecret];
            var ApiScope = values[Constants.RedditApiScope];

            if (string.IsNullOrEmpty(ClientId) || string.IsNullOrEmpty(ClientSecret) || string.IsNullOrEmpty(ApiScope))
            {
                customErrorMessage = Constants.GetLocalizedSharedString(Constants.RedditConnector_BasicValidationError);
                return false;
            }

            return true;
        }

        #endregion

        #region Private Methods

        private void GetValue(Dictionary<string, string> settings, Dictionary<string, string> configs, string settingName, bool applyDecryption = true, string defaultValue = "")
        {
            string settingValue;
            settings.TryGetValue(settingName, out settingValue);
            if (!string.IsNullOrEmpty(settingValue))
            {
                settingValue = applyDecryption ? new PortalSecurity().Decrypt(Host.GUID, settingValue) : settingValue;
            }
            else
            {
                settingValue = defaultValue ?? string.Empty;
            }
            configs.Add(settingName, settingValue);
        }

        private void SetSetting(int portalId, IDictionary<string, string> values, string settingName, bool applyEncryption = true)
        {
            string settingValue;
            values.TryGetValue(settingName, out settingValue);
            if (!string.IsNullOrEmpty(settingValue))
            {
                settingValue = applyEncryption ? new PortalSecurity().Encrypt(Host.GUID, settingValue) : settingValue;
            }

            PortalController.UpdatePortalSetting(portalId, settingName, settingValue ?? String.Empty, true);
        }

        public IEnumerable<IConnector> GetConnectors(int portalId)
        {
            return new List<IConnector> { this };
        }

        public void DeleteConnector(int portalId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
