namespace WMS.Presentation.DTOs.Auth
{
    public sealed record class LogoutRequest
    {
        public string Username { get; set; }
        public string RefreshToken { get; set; }
    }
}