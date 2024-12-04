namespace LabsoftAPI
{
    public class MyLIMSResponseBasic<T>
    {
        public object? NextPageLink { get; set; }
        public int? Count { get; set; }
        public T? Items { get; set; }
    }
}