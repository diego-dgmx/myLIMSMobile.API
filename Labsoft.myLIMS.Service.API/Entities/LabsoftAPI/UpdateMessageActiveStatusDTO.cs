namespace LabsoftAPI
{
    public class UpdateMessageActiveStatusDTO
    {
        public required int MessageId { get; set; }
        public required bool Active { get; set; }
        public required string Reason { get; set; }
    }
}