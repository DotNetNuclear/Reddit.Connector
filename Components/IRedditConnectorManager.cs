namespace DotNetNuclear.DNN.Connectors.Reddit.Components
{
    public interface IRedditConnectionManager
    {
        bool HasConfig { get; }
        string AuthorizationEndpoint { get; }
        string AccessTokenEndpoint { get; }
        string MeGraphEndpoint { get; }
        string AccessTokenCookieName { get; }
        string ClientId { get; }
        string ClientSecret { get; }
        string ApiScope { get; }

    }
}
