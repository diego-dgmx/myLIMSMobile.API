using LabsoftAPI;

namespace Entities {
    public class AnalyticsSamples {
        public required List<Sample> PrepareAnalytics { get; set; }
        public required List<Sample> CarriedOutAnalytics { get; set; }
        public required List<Sample> ReviewAnalytics { get; set; }
        public required List<QCTest?> BatchQC { get; set; }
    }
}