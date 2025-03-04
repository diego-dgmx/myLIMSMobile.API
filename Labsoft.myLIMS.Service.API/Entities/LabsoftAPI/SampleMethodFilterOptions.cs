namespace LabsoftAPI {
    public class SampleMethodFilterOptions
    {
        public List<FilterOptionBasic>? Methods { get; set; }
        public List<FilterOptionBasic>? SampleTypes { get; set; }
        public List<FilterOptionBasic>? CurrentStatus { get; set; }
        public List<FilterOptionBasic>? ServiceAreas { get; set; }
        public List<FilterOptionBasic>? SampleReasons { get; set; }
        public List<ControlFilterOptionBasic>? Works { get; set; }
        public List<FilterOptionBasic>? ResponsableStartUsers { get; set; }
        public List<FilterOptionBasic>? CollectionPoints { get; set; }
        public List<ControlFilterOptionBasic>? QcTests { get; set; }
        public List<ControlFilterOptionBasic>? Samples { get; set; }
    }

    public class FilterOptionBasic
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class ControlFilterOptionBasic
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
        public string? ControlNumber { get; set; }
    }
}