namespace MVVMFirma.Models.BusinessViews
{
    public class RaportSprzedazMiesiecznaDto
    {
        public int Rok { get; set; }
        public int Miesiac { get; set; }
        public int LiczbaFaktur { get; set; }
        public decimal SumaNetto { get; set; }
        public decimal SumaVat { get; set; }
        public decimal SumaBrutto { get; set; }
    }
}