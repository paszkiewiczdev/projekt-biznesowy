using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieFakturyViewModel : WszystkieViewModel<FakturaSprzedazy>
    {
        public WszystkieFakturyViewModel()
        {
            DisplayName = "Faktury";
        }

        public override void load()
        {
            List = new ObservableCollection<FakturaSprzedazy>(
                from f in fakturyEntities.FakturaSprzedazy
                select f
            );
        }
    }
}
