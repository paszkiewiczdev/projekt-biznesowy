using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models.BusinessViews;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieRaportTopTowaryViewModel : WszystkieViewModel<RaportTopTowaryDto>
    {
        public WszystkieRaportTopTowaryViewModel()
            : base()
        {
            base.DisplayName = "Raport: Top towary";
        }

        public override void load()
        {
            List = new ObservableCollection<RaportTopTowaryDto>(
                fakturyEntities.Database.SqlQuery<RaportTopTowaryDto>(
                    "SELECT IdTowaru, TowarNazwa, IloscRazem, WartoscNetto, WartoscVat, WartoscBrutto FROM dbo.v_RaportTopTowary"
                ).ToList()
            );
        }
    }
}