using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieKategorieTowaruViewModel : WszystkieViewModel<KategoriaTowaru>
    {
        public WszystkieKategorieTowaruViewModel()
            : base()
        {
            base.DisplayName = "Kategorie towaru";
        }

        public override void load()
        {
            List = new ObservableCollection<KategoriaTowaru>(
                from k in fakturyEntities.KategoriaTowaru
                select k
            );
        }
    }
}
