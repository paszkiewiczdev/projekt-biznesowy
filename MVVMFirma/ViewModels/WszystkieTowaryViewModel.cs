using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieTowaryViewModel : WszystkieViewModel<Towar>
    {
        public WszystkieTowaryViewModel()
            : base()
        {
            base.DisplayName = "Towary";
        }

        public override void load()
        {
            List = new ObservableCollection<Towar>(
                from t in fakturyEntities.Towar
                select t
            );
        }
    }
}
