using System.Collections.Generic;
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
            DisplayName = "Raport: Top towary";
            SetSortOptions(new[] { "Id", "Towar", "Ilość" });
        }

        protected override IEnumerable<RaportTopTowaryDto> LoadData()
        {
            return fakturyEntities.Database.SqlQuery<RaportTopTowaryDto>(
                "SELECT IdTowaru, TowarNazwa, IloscRazem, WartoscNetto, WartoscVat, WartoscBrutto FROM dbo.v_RaportTopTowary"
            ).ToList();
        }

        protected override bool MatchesFilter(RaportTopTowaryDto item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdTowaru.ToString().Contains(text)
                   || (item.TowarNazwa != null && item.TowarNazwa.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<RaportTopTowaryDto> ApplySort(
            IEnumerable<RaportTopTowaryDto> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Towar" => descending
                    ? query.OrderByDescending(r => r.TowarNazwa)
                    : query.OrderBy(r => r.TowarNazwa),

                "Ilość" => descending
                    ? query.OrderByDescending(r => r.IloscRazem)
                    : query.OrderBy(r => r.IloscRazem),

                _ => descending
                    ? query.OrderByDescending(r => r.IdTowaru)
                    : query.OrderBy(r => r.IdTowaru)
            };
        }
    }
}
