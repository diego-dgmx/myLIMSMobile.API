using LabsoftAPI;

public interface IAuthServices
{
    Task<ExternalResponse<LoginResponse, ErrorResponse>> Login(string email, string password);
    Task<ExternalResponse<MeResponse, ErrorResponse>> Me(string authToken, string email);
}