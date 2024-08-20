namespace LabsoftAPI
{
    public class MyLIMSResponseBase<T>
    {
        public object? Skip { get; set; }
        public int? Count { get; set; }
        public int? TotalCount { get; set; }
        public List<T>? Result { get; set; }
    }
}