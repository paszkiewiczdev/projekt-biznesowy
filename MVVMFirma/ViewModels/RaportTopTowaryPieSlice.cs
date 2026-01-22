using System.Windows.Media;

namespace MVVMFirma.ViewModels
{
    public class RaportTopTowaryPieSlice
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public double Percentage { get; set; }
        public Geometry Geometry { get; set; }
        public Brush Fill { get; set; }
    }
}
