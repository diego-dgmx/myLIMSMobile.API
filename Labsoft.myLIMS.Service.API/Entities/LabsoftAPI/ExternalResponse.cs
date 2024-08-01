namespace LabsoftAPI
{
    public class ExternalResponse<TSuccess, TError> {
        public int StatusCode { get; set; }
        public TSuccess? Success { get; set; }
        public TError? Error { get; set; }
    }
}