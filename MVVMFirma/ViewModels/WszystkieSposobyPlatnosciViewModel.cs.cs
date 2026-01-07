using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieSposobyPlatnosciViewModel : WszystkieViewModel<SposobPlatnosci>
    {
        public WszystkieSposobyPlatnosciViewModel()
            : base()
        {
            base.DisplayName = "Sposoby płatności";
        }

        public override void load()
        {
            List = new ObservableCollection<SposobPlatnosci>(
                from s in fakturyEntities.SposobPlatnosci
                select s
            );
        }
    }
}
