using System.Collections.ObjectModel;
using System.Data.Entity;      
using MVVMFirma.Models;

namespace MVVMFirma.ViewModels
{
    public class WszystkieStanyMagazynoweViewModel : WorkspaceViewModel
    {
        private readonly FakturyEntities _db;

        
        public ObservableCollection<StanMagazynowy> StanyMagazynowe
        {
            get { return _db.StanMagazynowy.Local; }
        }

        public WszystkieStanyMagazynoweViewModel()
        {
            base.DisplayName = "Stany magazynowe";

            _db = new FakturyEntities();

            
            _db.StanMagazynowy.Load();
        }
    }
}
