using System.Collections.Generic;
using System.Linq;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieFakturyViewModel : WszystkieViewModel<FakturaSprzedazy>
    {
        public WszystkieFakturyViewModel()
        {
            DisplayName = "Faktury";
            SetSortOptions(new[] { "Id", "Numer", "Data wystawienia" });
        }

        protected override IEnumerable<FakturaSprzedazy> LoadData()
        {
            return fakturyEntities.FakturaSprzedazy.ToList();
        }

        protected override bool MatchesFilter(FakturaSprzedazy item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdFakturySprzedazy.ToString().Contains(text)
                   || (item.Numer != null && item.Numer.ToLowerInvariant().Contains(text))
                   || (item.Kontrahent != null
                       && item.Kontrahent.NazwaPelna != null
                       && item.Kontrahent.NazwaPelna.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<FakturaSprzedazy> ApplySort(
            IEnumerable<FakturaSprzedazy> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Numer" => descending
                    ? query.OrderByDescending(f => f.Numer)
                    : query.OrderBy(f => f.Numer),

                "Data wystawienia" => descending
                    ? query.OrderByDescending(f => f.DataWystawienia)
                    : query.OrderBy(f => f.DataWystawienia),

                _ => descending
                    ? query.OrderByDescending(f => f.IdFakturySprzedazy)
                    : query.OrderBy(f => f.IdFakturySprzedazy)
            };
        }
    }
}
