using LabsoftAPI;
using LabsoftAPI.Auth;

namespace Services {
    public interface IAuthServices
    {
        Task<ExternalResponse<LoginResponse, ErrorResponse>> Login(string email, string password);
        Task<ExternalResponse<List<MeResponse>, ErrorResponse>> Me(string email);
    }
}