namespace MVVMFirma.Models.BusinessViews
{
    public class RaportStanMagazynowyDto
    {
        public string MagazynKod { get; set; }
        public string MagazynNazwa { get; set; }
        public int IdTowaru { get; set; }
        public string TowarNazwa { get; set; }
        public decimal Ilosc { get; set; }
    }
}