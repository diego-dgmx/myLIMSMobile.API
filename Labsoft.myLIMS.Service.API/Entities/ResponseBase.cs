namespace Entities
{
    public class ResponseBase<T> {
        public bool Ok { get; set; } = true;
        public T? Data { get; set; }
        public string? Message { get; set; }
        public ErrorBase? Error { get; set; }
    }

    public class ErrorBase {
        public string? Code { get; set; }
        public string? Description { get; set; }
    }
}