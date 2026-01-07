using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieWalutyViewModel : WszystkieViewModel<Waluta>
    {
        public WszystkieWalutyViewModel()
            : base()
        {
            base.DisplayName = "Waluty";
        }

        public override void load()
        {
            List = new ObservableCollection<Waluta>(
                from w in fakturyEntities.Waluta
                select w
            );
        }
    }
}