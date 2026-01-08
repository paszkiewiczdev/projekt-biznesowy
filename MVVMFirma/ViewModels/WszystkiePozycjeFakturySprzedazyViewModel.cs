using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkiePozycjeFakturySprzedazyViewModel : WszystkieViewModel<PozycjaFakturySprzedazy>
    {
        public WszystkiePozycjeFakturySprzedazyViewModel()
        {
            DisplayName = "Pozycje faktur sprzedaży";
            SetSortOptions(new[] { "Id", "Faktura", "Towar", "Ilość" });
        }

        protected override IEnumerable<PozycjaFakturySprzedazy> LoadData()
        {
            return fakturyEntities.PozycjaFakturySprzedazy
                .Include(p => p.FakturaSprzedazy)
                .Include(p => p.FakturaSprzedazy.Kontrahent)
                .Include(p => p.Towar)
                .Include(p => p.StawkaVat)
                .ToList();
        }

        protected override bool MatchesFilter(PozycjaFakturySprzedazy item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdPozycjiFakturySprzedazy.ToString().Contains(text)
                   || (item.FakturaSprzedazy != null
                       && item.FakturaSprzedazy.Numer != null
                       && item.FakturaSprzedazy.Numer.ToLowerInvariant().Contains(text))
                   || (item.Towar != null
                       && item.Towar.Nazwa != null
                       && item.Towar.Nazwa.ToLowerInvariant().Contains(text))
                   || (item.FakturaSprzedazy != null
                       && item.FakturaSprzedazy.Kontrahent != null
                       && item.FakturaSprzedazy.Kontrahent.NazwaPelna != null
                       && item.FakturaSprzedazy.Kontrahent.NazwaPelna.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<PozycjaFakturySprzedazy> ApplySort(
            IEnumerable<PozycjaFakturySprzedazy> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Faktura" => descending
                    ? query.OrderByDescending(p => p.FakturaSprzedazy != null ? p.FakturaSprzedazy.Numer : null)
                    : query.OrderBy(p => p.FakturaSprzedazy != null ? p.FakturaSprzedazy.Numer : null),

                "Towar" => descending
                    ? query.OrderByDescending(p => p.Towar != null ? p.Towar.Nazwa : null)
                    : query.OrderBy(p => p.Towar != null ? p.Towar.Nazwa : null),

                "Ilość" => descending
                    ? query.OrderByDescending(p => p.Ilosc)
                    : query.OrderBy(p => p.Ilosc),

                _ => descending
                    ? query.OrderByDescending(p => p.IdPozycjiFakturySprzedazy)
                    : query.OrderBy(p => p.IdPozycjiFakturySprzedazy)
            };
        }
    }
}
