using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieMagazynyViewModel : WszystkieViewModel<Magazyn>
    {
        public WszystkieMagazynyViewModel()
            : base()
        {
            base.DisplayName = "Magazyny";
        }

        public override void load()
        {
            List = new ObservableCollection<Magazyn>(
                from m in fakturyEntities.Magazyn
                select m
            );
        }
    }
}
