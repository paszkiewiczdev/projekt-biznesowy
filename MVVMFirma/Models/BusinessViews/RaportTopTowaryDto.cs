namespace MVVMFirma.Models.BusinessViews
{
    public class RaportTopTowaryDto
    {
        public int IdTowaru { get; set; }
        public string TowarNazwa { get; set; }
        public decimal IloscRazem { get; set; }
        public decimal WartoscNetto { get; set; }
        public decimal WartoscVat { get; set; }
        public decimal WartoscBrutto { get; set; }
    }
}