using System.Collections.Generic;
using System.Linq;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieTowaryViewModel : WszystkieViewModel<Towar>
    {
        public WszystkieTowaryViewModel()
            : base()
        {
            DisplayName = "Towary";
            SetSortOptions(new[] { "Id", "Kod", "Nazwa", "Cena" });
        }

        protected override IEnumerable<Towar> LoadData()
        {
            return fakturyEntities.Towar.ToList();
        }

        protected override bool MatchesFilter(Towar item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdTowaru.ToString().Contains(text)
                   || (item.Kod != null && item.Kod.ToLowerInvariant().Contains(text))
                   || (item.Nazwa != null && item.Nazwa.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<Towar> ApplySort(
            IEnumerable<Towar> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Kod" => descending
                    ? query.OrderByDescending(t => t.Kod)
                    : query.OrderBy(t => t.Kod),

                "Nazwa" => descending
                    ? query.OrderByDescending(t => t.Nazwa)
                    : query.OrderBy(t => t.Nazwa),

                "Cena" => descending
                    ? query.OrderByDescending(t => t.Cena)
                    : query.OrderBy(t => t.Cena),

                _ => descending
                    ? query.OrderByDescending(t => t.IdTowaru)
                    : query.OrderBy(t => t.IdTowaru)
            };
        }
    }
}
