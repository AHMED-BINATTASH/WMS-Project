namespace WMS.Presentation.DTOs.Auth
{
    public sealed record class TokenResponse
    {
        public string AccessToken { get; set; }
        //public string RefreshToken { get; set; }
    }
}