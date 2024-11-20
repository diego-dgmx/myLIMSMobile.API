namespace LabsoftAPI
{
    public class MyLIMSResponseSimpleBase<T>
    {
        public object? Skip { get; set; }
        public int? Count { get; set; }
        public int? TotalCount { get; set; }
        public T? Result { get; set; }
    }
}