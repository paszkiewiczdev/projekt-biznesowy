using System.Collections.ObjectModel;
using System.Data.Entity;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkiePozycjeFakturySprzedazyViewModel : WorkspaceViewModel
    {
        private readonly FakturyEntities db;

        public ObservableCollection<PozycjaFakturySprzedazy> PozycjeFakturySprzedazy { get; set; }

        public WszystkiePozycjeFakturySprzedazyViewModel()
        {
            base.DisplayName = "Pozycje faktur sprzedaży";
            db = new FakturyEntities();
            Load();
        }

        private void Load()
        {
            PozycjeFakturySprzedazy = new ObservableCollection<PozycjaFakturySprzedazy>(
                db.PozycjaFakturySprzedazy
                  .Include(p => p.FakturaSprzedazy)
                  .Include(p => p.FakturaSprzedazy.Kontrahent)
                  .Include(p => p.Towar)
                  .Include(p => p.StawkaVat)
            );
        }
    }
}
