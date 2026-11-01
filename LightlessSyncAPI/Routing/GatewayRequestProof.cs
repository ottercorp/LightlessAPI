using System.Security.Cryptography;
using System.Text;

namespace LightlessSync.API.Routing
{
    public static class GatewayRequestProof
    {
        public const string Issuer = "LightlessSync.Gateway";
        public const string Audience = "LightlessSync.GatewayRequest.v1";
        public const string GatewayIdClaim = "gateway_id";
        public const string PublicOriginClaim = "gateway_origin";
        public const string RequestClaim = "gateway_request";
        public const string ClientIpClaim = "gateway_client_ip";

        public static string GetRequestHash(string method, string pathAndQuery)
        {
            return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(
                method.ToUpperInvariant() + "\n" + pathAndQuery)));
        }
    }
}
