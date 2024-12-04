using LabsoftAPI;

namespace Services {
    public interface IMessagesServices
    {
        Task<ExternalResponse<List<MessageBasic>, ErrorResponse>> GetMessagesBySampleId(int sampleId);
        Task<ExternalResponse<List<MessageTypeBasic>, ErrorResponse>> GetMessageTypes();
    }
}