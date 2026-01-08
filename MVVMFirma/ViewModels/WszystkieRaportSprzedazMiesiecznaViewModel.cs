using System.Collections.Generic;
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
            DisplayName = "Raport: Sprzedaż miesięczna";
            SetSortOptions(new[] { "Rok", "Miesiąc", "Liczba faktur" });
        }

        protected override IEnumerable<RaportSprzedazMiesiecznaDto> LoadData()
        {
            return fakturyEntities.Database.SqlQuery<RaportSprzedazMiesiecznaDto>(
                "SELECT Rok, Miesiac, LiczbaFaktur, SumaNetto, SumaVat, SumaBrutto FROM dbo.v_RaportSprzedazMiesieczna"
            ).ToList();
        }

        protected override bool MatchesFilter(RaportSprzedazMiesiecznaDto item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.Rok.ToString().Contains(text)
                   || item.Miesiac.ToString().Contains(text);
        }

        protected override IOrderedEnumerable<RaportSprzedazMiesiecznaDto> ApplySort(
            IEnumerable<RaportSprzedazMiesiecznaDto> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Miesiąc" => descending ? query.OrderByDescending(r => r.Miesiac) : query.OrderBy(r => r.Miesiac),
                "Liczba faktur" => descending ? query.OrderByDescending(r => r.LiczbaFaktur) : query.OrderBy(r => r.LiczbaFaktur),
                _ => descending ? query.OrderByDescending(r => r.Rok) : query.OrderBy(r => r.Rok)
            };
        }
    }
}
