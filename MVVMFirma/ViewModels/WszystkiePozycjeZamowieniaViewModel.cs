using System;
using System.Collections.ObjectModel;
using System.Data.Entity;          
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVMFirma.Models;

namespace MVVMFirma.ViewModels
{
    public class WszystkiePozycjeZamowieniaViewModel : WorkspaceViewModel
    {
        private readonly FakturyEntities _db;

        
        public ObservableCollection<PozycjaZamowieniaSprzedazy> PozycjeZamowien
        {
            get { return _db.PozycjaZamowieniaSprzedazy.Local; }
        }

        public WszystkiePozycjeZamowieniaViewModel()
        {
            base.DisplayName = "Pozycje zamówień sprzedaży";

            _db = new FakturyEntities();

            
            _db.PozycjaZamowieniaSprzedazy.Load();
        }
    }
}
