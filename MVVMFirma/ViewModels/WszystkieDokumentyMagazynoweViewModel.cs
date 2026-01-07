using System.Collections.ObjectModel;
using System.Data.Entity;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieDokumentyMagazynoweViewModel : WorkspaceViewModel
    {
        private readonly FakturyEntities db;

        public ObservableCollection<DokumentMagazynowy> DokumentyMagazynowe { get; set; }

        public WszystkieDokumentyMagazynoweViewModel()
        {
            base.DisplayName = "Dokumenty magazynowe";
            db = new FakturyEntities();
            Load();
        }

        private void Load()
        {
            DokumentyMagazynowe = new ObservableCollection<DokumentMagazynowy>(
                db.DokumentMagazynowy
                  .Include(d => d.Magazyn)
                  .Include(d => d.TypDokumentuMagazynowego)
            );
        }
    }
}
