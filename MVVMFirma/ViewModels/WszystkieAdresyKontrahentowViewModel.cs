using System.Collections.ObjectModel;
using System.Data.Entity;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieAdresyKontrahentowViewModel : WorkspaceViewModel
    {
        private readonly FakturyEntities db;

        public ObservableCollection<AdresKontrahenta> AdresyKontrahentow { get; set; }

        public WszystkieAdresyKontrahentowViewModel()
        {
            base.DisplayName = "Adresy kontrahentów";
            db = new FakturyEntities();
            Load();
        }

        private void Load()
        {
            AdresyKontrahentow = new ObservableCollection<AdresKontrahenta>(
                db.AdresKontrahenta
                  .Include(a => a.Kontrahent)
            );
        }
    }
}
