using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models.BusinessViews;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieRaportStanMagazynowyViewModel : WszystkieViewModel<RaportStanMagazynowyDto>
    {
        public WszystkieRaportStanMagazynowyViewModel()
            : base()
        {
            base.DisplayName = "Raport: Stan magazynowy";
        }

        public override void load()
        {
            List = new ObservableCollection<RaportStanMagazynowyDto>(
                fakturyEntities.Database.SqlQuery<RaportStanMagazynowyDto>(
                    "SELECT MagazynKod, MagazynNazwa, IdTowaru, TowarNazwa, Ilosc FROM dbo.v_RaportStanMagazynowy"
                ).ToList()
            );
        }
    }
}