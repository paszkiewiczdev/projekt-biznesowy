using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieAdresyKontrahentowViewModel : WszystkieViewModel<AdresKontrahenta>
    {
        public WszystkieAdresyKontrahentowViewModel()
        {
            DisplayName = "Adresy kontrahentów";
            SetSortOptions(new[] { "Id", "Kontrahent", "Miasto" });
        }

        protected override IEnumerable<AdresKontrahenta> LoadData()
        {
            return fakturyEntities.AdresKontrahenta
                .Include(a => a.Kontrahent)
                .ToList();
        }

        protected override bool MatchesFilter(AdresKontrahenta item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdAdresu.ToString().Contains(text)
                   || (item.Kontrahent != null
                       && item.Kontrahent.NazwaPelna != null
                       && item.Kontrahent.NazwaPelna.ToLowerInvariant().Contains(text))
                   || (item.Miasto != null && item.Miasto.ToLowerInvariant().Contains(text))
                   || (item.Ulica != null && item.Ulica.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<AdresKontrahenta> ApplySort(IEnumerable<AdresKontrahenta> query, string sortField, bool descending)
        {
            if (sortField == "Kontrahent")
            {
                return descending
                    ? query.OrderByDescending(a => a.Kontrahent != null ? a.Kontrahent.NazwaPelna : null)
                    : query.OrderBy(a => a.Kontrahent != null ? a.Kontrahent.NazwaPelna : null);
            }

            if (sortField == "Miasto")
            {
                return descending
                    ? query.OrderByDescending(a => a.Miasto)
                    : query.OrderBy(a => a.Miasto);
            }

            return descending
                ? query.OrderByDescending(a => a.IdAdresu)
                : query.OrderBy(a => a.IdAdresu);
        }
    }
}
