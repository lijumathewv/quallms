namespace QualvationLibrary
{
    public class JwtTokens
    {
        public string Token { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;
    }

    public class UserRefreshToken
    {
        public string UserName { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;
    }
}
