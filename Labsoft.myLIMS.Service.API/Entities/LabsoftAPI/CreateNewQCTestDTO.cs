namespace LabsoftAPI
{
    public class CreateNewQCTestDTO
    {
        public required int QCRoutineBatchId { get; set; }
        public int? EquipmentId { get; set; }
        public required List<int> SampleMethodIds { get; set; }
    }
}