using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;

namespace DotNetNuclear.DNN.Connectors.Reddit.Components
{
    public class Constants
    {
        public const string LocalResourceFile =
            "~/DesktopModules/Connectors/Reddit/App_LocalResources/SharedResources.resx";

        public const string RedditAuthorizationEndpoint = "AuthorizationEndpoint";
        public const string RedditClientID = "RedditConnector_ClientID";
        public const string RedditClientSecret = "RedditConnector_ClientSecret";
        public const string RedditApiScope = "RedditConnector_APIScope";

        public const string RedditApiBaseUrl = "https://www.reddit.com/api/v1";
        public const string RedditOAuthApiBaseUrl = "https://oauth.reddit.com/api/v1";
        public const string RedditApiScope_Default = "identity";
        public const string RedditApiTokenCookieName = "RedditApiAccessToken";

        public const string RedditConnector_BasicValidationError = "BasicValidationErrorMessage";

        public static string GetLocalizedSharedString(string key)
        {
            return Localization.GetString(key, LocalResourceFile);
        }

    }
}
