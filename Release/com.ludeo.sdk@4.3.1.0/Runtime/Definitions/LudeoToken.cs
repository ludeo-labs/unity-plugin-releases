/// @cond INTERNAL
namespace LudeoSDK.Internal
{
    public struct LudeoTokens
    {
        internal const string AppTokenPrefix = "appToken_";
        internal const string AuthTokenPrefix = "authToken_";

        internal string AppToken { get; set; }
        internal string AuthToken { get; set; }

        internal LudeoTokens(string authToken = "", string appToken = "") : this()
        {
            this.AuthToken = authToken;
            this.AppToken = appToken;
        }

        public override string ToString()
        {
            return $"AuthToken: {AuthToken}, AppToken: {AppToken}";
        }
    }
}
/// @endcond INTERNAL
