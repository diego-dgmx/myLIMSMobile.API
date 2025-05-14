using LabsoftAPI;
using LabsoftAPI.Auth;

namespace Interfaces {
    public interface IAuthServices
    {
        Task<ExternalResponse<LoginResponse, ErrorResponse>> Login(string? email, string? password, string? refreshToken);
        Task<ExternalResponse<List<MeResponse>, ErrorResponse>> Me(string email);
        Task<ExternalResponse<UserDataBasic, ErrorResponse>> UserDataById(int id);
        Task<ExternalResponse<dynamic, ErrorResponse>> LogoutBySessionId(string sessionId);
    }
}