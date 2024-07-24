public interface IAuthServices
{
    Task<LoginResponse<LoginSuccessResponse, LoginErrorResponse>> Login(string email, string password);
}