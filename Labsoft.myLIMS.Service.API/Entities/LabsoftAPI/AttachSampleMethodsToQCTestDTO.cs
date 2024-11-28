namespace LabsoftAPI
{
    public class AttachSampleMethodsToQCTestDTO
    {
        public required int QCTestId { get; set; }
        public required List<int> SampleMethodIds { get; set; }
    }
}