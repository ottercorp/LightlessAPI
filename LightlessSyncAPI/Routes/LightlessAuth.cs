namespace LightlessSync.API.Routes;

public class LightlessAuth
{
    public const string OAuth = "/oauth";
    public const string Auth = "/auth";
    public const string XIVAuth = "/xivauth";
    public const string Auth_CreateIdent = "createWithIdent";
    public const string Auth_RenewToken = "renewToken";
    public const string OAuth_GetUIDsBasedOnSecretKeys = "getUIDsViaSecretKey";
    public const string OAuth_CreateOAuth = "createWithOAuth";
    public const string OAuth_RenewOAuthToken = "renewToken";
    public const string OAuth_GetDiscordOAuthEndpoint = "getDiscordOAuthEndpoint";
    public const string OAuth_GetUIDs = "getUIDs";
    public const string OAuth_GetDiscordOAuthToken = "getDiscordOAuthToken";
    public const string XIVAuth_GetEndpoint = "plugin/endpoint";
    public const string XIVAuth_GetToken = "plugin/token";
    public const string User = "/user";
    public const string Group = "/group";
    public const string User_Unban_Discord = "unbanDiscord";
    public const string User_Unban_Uid = "unbanUID";
    public const string Ban_Uid = "ban";
    public const string Disable_Profile = "disableProfile";
    public const string Enable_Profile = "enableProfile";
    public static Uri AuthFullPath(Uri baseUri) => new Uri(baseUri, Auth + "/" + Auth_CreateIdent);
    public static Uri AuthWithOauthFullPath(Uri baseUri) => new Uri(baseUri, OAuth + "/" + OAuth_CreateOAuth);
    public static Uri RenewTokenFullPath(Uri baseUri) => new Uri(baseUri, Auth + "/" + Auth_RenewToken);
    public static Uri RenewOAuthTokenFullPath(Uri baseUri) => new Uri(baseUri, OAuth + "/" + OAuth_RenewOAuthToken);
    public static Uri GetUIDsBasedOnSecretKeyFullPath(Uri baseUri) => new Uri(baseUri, OAuth + "/" + OAuth_GetUIDsBasedOnSecretKeys);
    public static Uri GetDiscordOAuthEndpointFullPath(Uri baseUri) => new Uri(baseUri, OAuth + "/" + OAuth_GetDiscordOAuthEndpoint);
    public static Uri GetDiscordOAuthTokenFullPath(Uri baseUri, string sessionId) => new Uri(baseUri, OAuth + "/" + OAuth_GetDiscordOAuthToken + "?sessionId=" + sessionId);
    public static Uri GetUIDsFullPath(Uri baseUri) => new Uri(baseUri, OAuth + "/" + OAuth_GetUIDs);
    public static Uri GetXIVAuthEndpointFullPath(Uri baseUri) => new Uri(baseUri, XIVAuth + "/" + XIVAuth_GetEndpoint);
    public static Uri GetXIVAuthTokenFullPath(Uri baseUri, string sessionId) => new Uri(baseUri, XIVAuth + "/" + XIVAuth_GetToken + "?sessionId=" + sessionId);
}