using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieStatusyFakturViewModel : WszystkieViewModel<StatusFaktury>
    {
        public WszystkieStatusyFakturViewModel()
            : base()
        {
            base.DisplayName = "Statusy faktur";
        }

        public override void load()
        {
            List = new ObservableCollection<StatusFaktury>(
                from s in fakturyEntities.StatusFaktury
                select s
            );
        }
    }
}
