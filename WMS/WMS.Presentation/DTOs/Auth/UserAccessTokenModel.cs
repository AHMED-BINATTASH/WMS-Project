namespace WMS.Presentation.DTOs.Auth
{
    public sealed record class UserAccessTokenModel
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }

        public UserAccessTokenModel(int userID, string username, string role)
        {
            UserID = userID;
            Username = username;
            Role = role;
        }

    }
}
