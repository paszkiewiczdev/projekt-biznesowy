using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkiePozycjeZamowieniaViewModel : WszystkieViewModel<PozycjaZamowieniaSprzedazy>
    {
        public WszystkiePozycjeZamowieniaViewModel()
        {
            DisplayName = "Pozycje zamówień sprzedaży";
            SetSortOptions(new[] { "Id", "Zamówienie", "Towar", "Ilość" });
        }

        protected override IEnumerable<PozycjaZamowieniaSprzedazy> LoadData()
        {
            return fakturyEntities.PozycjaZamowieniaSprzedazy
                .Include(p => p.ZamowienieSprzedazy)
                .Include(p => p.ZamowienieSprzedazy.Kontrahent)
                .Include(p => p.Towar)
                .ToList();
        }

        protected override bool MatchesFilter(PozycjaZamowieniaSprzedazy item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdPozycjiZamowieniaSprzedazy.ToString().Contains(text)
                   || (item.ZamowienieSprzedazy != null
                       && item.ZamowienieSprzedazy.Numer != null
                       && item.ZamowienieSprzedazy.Numer.ToLowerInvariant().Contains(text))
                   || (item.Towar != null
                       && item.Towar.Nazwa != null
                       && item.Towar.Nazwa.ToLowerInvariant().Contains(text))
                   || (item.ZamowienieSprzedazy != null
                       && item.ZamowienieSprzedazy.Kontrahent != null
                       && item.ZamowienieSprzedazy.Kontrahent.NazwaPelna != null
                       && item.ZamowienieSprzedazy.Kontrahent.NazwaPelna.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<PozycjaZamowieniaSprzedazy> ApplySort(
            IEnumerable<PozycjaZamowieniaSprzedazy> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Zamówienie" => descending
                    ? query.OrderByDescending(p => p.ZamowienieSprzedazy != null ? p.ZamowienieSprzedazy.Numer : null)
                    : query.OrderBy(p => p.ZamowienieSprzedazy != null ? p.ZamowienieSprzedazy.Numer : null),

                "Towar" => descending
                    ? query.OrderByDescending(p => p.Towar != null ? p.Towar.Nazwa : null)
                    : query.OrderBy(p => p.Towar != null ? p.Towar.Nazwa : null),

                "Ilość" => descending
                    ? query.OrderByDescending(p => p.Ilosc)
                    : query.OrderBy(p => p.Ilosc),

                _ => descending
                    ? query.OrderByDescending(p => p.IdPozycjiZamowieniaSprzedazy)
                    : query.OrderBy(p => p.IdPozycjiZamowieniaSprzedazy)
            };
        }
    }
}
