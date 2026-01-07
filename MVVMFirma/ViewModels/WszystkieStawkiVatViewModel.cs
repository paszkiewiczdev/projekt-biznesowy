using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieStawkiVatViewModel : WszystkieViewModel<StawkaVat>
    {
        public WszystkieStawkiVatViewModel()
            : base()
        {
            // tytuł zakładki
            base.DisplayName = "Stawki VAT";
        }

        public override void load()
        {
            List = new ObservableCollection<StawkaVat>(
                from s in fakturyEntities.StawkaVat
                select s
            );
        }
    }
}
