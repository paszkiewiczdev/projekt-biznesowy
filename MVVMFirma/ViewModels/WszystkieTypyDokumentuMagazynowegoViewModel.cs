using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieTypyDokumentuMagazynowegoViewModel
        : WszystkieViewModel<TypDokumentuMagazynowego>
    {
        public WszystkieTypyDokumentuMagazynowegoViewModel()
            : base()
        {
            base.DisplayName = "Typy dokumentów magazynowych";
        }

        public override void load()
        {
            List = new ObservableCollection<TypDokumentuMagazynowego>(
                from t in fakturyEntities.TypDokumentuMagazynowego
                select t
            );
        }
    }
}
