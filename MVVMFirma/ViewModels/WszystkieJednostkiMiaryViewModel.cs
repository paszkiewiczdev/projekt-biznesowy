using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieJednostkiMiaryViewModel : WszystkieViewModel<JednostkaMiary>
    {
        public WszystkieJednostkiMiaryViewModel()
            : base()
        {
            base.DisplayName = "Jednostki miary";
        }

        public override void load()
        {
            List = new ObservableCollection<JednostkaMiary>(
                from j in fakturyEntities.JednostkaMiary
                select j
            );
        }
    }
}
