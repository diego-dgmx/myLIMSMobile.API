namespace Entities
{
    class Pagination<T>
    {
        public required int CurrentPage { get; set; }
        public required int PerPage { get; set; }
        public required int TotalPages { get; set; }
        public required int TotalItems { get; set; }
        public required List<T> Items { get; set; }
    }
}