namespace WMS.Presentation.DTOs.Auth
{
    public sealed record class RefreshRequest
    {
        //public string RefreshToken { get; set; }
        public string Username { get; set; }
    }
}