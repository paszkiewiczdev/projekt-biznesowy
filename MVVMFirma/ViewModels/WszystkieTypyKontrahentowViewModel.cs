using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieTypyKontrahentowViewModel : WszystkieViewModel<TypKontrahenta>
    {
        public WszystkieTypyKontrahentowViewModel()
            : base()
        {
            base.DisplayName = "Typy kontrahentów";
        }

        public override void load()
        {
            List = new ObservableCollection<TypKontrahenta>(
                from t in fakturyEntities.TypKontrahenta
                select t
            );
        }
    }
}
