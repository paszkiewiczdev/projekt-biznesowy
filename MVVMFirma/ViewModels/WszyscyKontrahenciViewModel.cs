using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszyscyKontrahenciViewModel : WszystkieViewModel<Kontrahent>
    {
        public WszyscyKontrahenciViewModel()
        {
            DisplayName = "Kontrahenci";
        }

        public override void load()
        {
            List = new ObservableCollection<Kontrahent>(
                from k in fakturyEntities.Kontrahent
                select k
            );
        }
    }
}
