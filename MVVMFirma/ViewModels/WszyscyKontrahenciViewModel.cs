using System.Collections.Generic;
using System.Linq;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszyscyKontrahenciViewModel : WszystkieViewModel<Kontrahent>
    {
        public WszyscyKontrahenciViewModel()
        {
            DisplayName = "Kontrahenci";
            SetSortOptions(new[] { "Id", "NIP", "Nazwa pełna", "Nazwa skrócona" });
        }

        protected override IEnumerable<Kontrahent> LoadData()
        {
            return fakturyEntities.Kontrahent.ToList();
        }

        protected override bool MatchesFilter(Kontrahent item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdKontrahenta.ToString().Contains(text)
                   || (item.NIP != null && item.NIP.ToLowerInvariant().Contains(text))
                   || (item.NazwaPelna != null && item.NazwaPelna.ToLowerInvariant().Contains(text))
                   || (item.NazwaSkrocona != null && item.NazwaSkrocona.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<Kontrahent> ApplySort(IEnumerable<Kontrahent> query, string sortField, bool descending)
        {
            return sortField switch
            {
                "NIP" => descending ? query.OrderByDescending(k => k.NIP) : query.OrderBy(k => k.NIP),
                "Nazwa pełna" => descending ? query.OrderByDescending(k => k.NazwaPelna) : query.OrderBy(k => k.NazwaPelna),
                "Nazwa skrócona" => descending ? query.OrderByDescending(k => k.NazwaSkrocona) : query.OrderBy(k => k.NazwaSkrocona),
                _ => descending ? query.OrderByDescending(k => k.IdKontrahenta) : query.OrderBy(k => k.IdKontrahenta)
            };
        }
    }
}
