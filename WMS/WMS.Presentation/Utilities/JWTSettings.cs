namespace WMS.Presentation.Utilities
{
    public class JWTSettings
    {
       public string Issuer { get; set; }
       public string Audience { get; set; }
       public int Lifetime { get; set; }
       public string SigntureKey { get; set; }
    }
}
