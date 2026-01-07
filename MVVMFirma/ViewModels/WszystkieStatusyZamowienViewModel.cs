using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieStatusyZamowienViewModel : WszystkieViewModel<StatusZamowienia>
    {
        public WszystkieStatusyZamowienViewModel()
            : base()
        {
            base.DisplayName = "Statusy zamówień";
        }

        public override void load()
        {
            List = new ObservableCollection<StatusZamowienia>(
                from s in fakturyEntities.StatusZamowienia
                select s
            );
        }
    }
}
