using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models.BusinessViews;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieRaportSprzedazMiesiecznaViewModel : WszystkieViewModel<RaportSprzedazMiesiecznaDto>
    {
        public WszystkieRaportSprzedazMiesiecznaViewModel()
            : base()
        {
            base.DisplayName = "Raport: Sprzedaż miesięczna";
        }

        public override void load()
        {
            List = new ObservableCollection<RaportSprzedazMiesiecznaDto>(
                fakturyEntities.Database.SqlQuery<RaportSprzedazMiesiecznaDto>(
                    "SELECT Rok, Miesiac, LiczbaFaktur, SumaNetto, SumaVat, SumaBrutto FROM dbo.v_RaportSprzedazMiesieczna"
                ).ToList()
            );
        }
    }
}