using System.Collections.ObjectModel;
using System.Data.Entity;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkiePozycjeDokumentuMagazynowegoViewModel : WorkspaceViewModel
    {
        private readonly FakturyEntities db;

        public ObservableCollection<PozycjaDokumentuMagazynowego> PozycjeDokumentuMagazynowego { get; set; }

        public WszystkiePozycjeDokumentuMagazynowegoViewModel()
        {
            base.DisplayName = "Pozycje dokumentów magazynowych";
            db = new FakturyEntities();
            Load();
        }

        private void Load()
        {
            PozycjeDokumentuMagazynowego = new ObservableCollection<PozycjaDokumentuMagazynowego>(
                db.PozycjaDokumentuMagazynowego
                  .Include(p => p.DokumentMagazynowy)
                  .Include(p => p.Towar)
            );
        }
    }
}
