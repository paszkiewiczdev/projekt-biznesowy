using System.Collections.ObjectModel;
using System.Data.Entity;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieZamowieniaViewModel : WorkspaceViewModel
    {

        private readonly FakturyEntities db;


        public ObservableCollection<ZamowienieSprzedazy> Zamowienia { get; set; }

        public WszystkieZamowieniaViewModel()
        {

            base.DisplayName = "Zamówienia sprzedaży";

            db = new FakturyEntities();

            Load();
        }


        private void Load()
        {
            Zamowienia = new ObservableCollection<ZamowienieSprzedazy>(
                db.ZamowienieSprzedazy
                  .Include(z => z.Kontrahent)
                  .Include(z => z.StatusZamowienia)
            );
        }
    }
}
