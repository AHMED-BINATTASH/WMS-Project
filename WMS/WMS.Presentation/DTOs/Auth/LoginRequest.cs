   namespace WMS.Presentation.DTOs.Auth
{
    public sealed record class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
