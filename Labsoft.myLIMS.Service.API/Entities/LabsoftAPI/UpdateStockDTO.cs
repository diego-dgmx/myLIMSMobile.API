namespace LabsoftAPI
{
    public class UpdateStockDTO
    {
        public required string ConsumableId { get; set; }
        public required int Quantity { get; set; }
        public required string Notes { get; set; }
    }
}