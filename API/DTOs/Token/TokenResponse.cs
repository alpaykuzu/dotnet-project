namespace API.DTOs.Token
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string AccessTokenExpTime { get; set; }
        public string RefreshTokenExpTime { get; set; }
    }
}
