namespace WriteLens.Auth.Settings;

public class JwtSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Secret { get; set; }
    public int ExpirationInMinutes { get; set; }

    public override string ToString()
    {
        return $"{nameof(JwtSettings)}: (Issuer: {Issuer}; Audience: {Audience}; Secret: {Secret}; ExpirationInMinutes: {ExpirationInMinutes})";
    }
}