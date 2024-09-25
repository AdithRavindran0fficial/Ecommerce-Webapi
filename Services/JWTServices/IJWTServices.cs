namespace Ecommerce_Webapi.Services.JWTServices
{
    public interface IJWTServices
    {
        int GetUserId(string token);
    }
}
