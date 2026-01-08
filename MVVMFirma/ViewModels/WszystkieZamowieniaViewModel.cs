using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieZamowieniaViewModel : WszystkieViewModel<ZamowienieSprzedazy>
    {
        public WszystkieZamowieniaViewModel()
        {
            DisplayName = "Zamówienia sprzedaży";
            SetSortOptions(new[] { "Id", "Numer", "Data" });
        }

        protected override IEnumerable<ZamowienieSprzedazy> LoadData()
        {
            return fakturyEntities.ZamowienieSprzedazy
                .Include(z => z.Kontrahent)
                .Include(z => z.StatusZamowienia)
                .ToList();
        }

        protected override bool MatchesFilter(ZamowienieSprzedazy item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdZamowieniaSprzedazy.ToString().Contains(text)
                   || (item.Numer != null && item.Numer.ToLowerInvariant().Contains(text))
                   || (item.Kontrahent != null
                       && item.Kontrahent.NazwaPelna != null
                       && item.Kontrahent.NazwaPelna.ToLowerInvariant().Contains(text))
                   || (item.StatusZamowienia != null
                       && item.StatusZamowienia.Nazwa != null
                       && item.StatusZamowienia.Nazwa.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<ZamowienieSprzedazy> ApplySort(
            IEnumerable<ZamowienieSprzedazy> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Numer" => descending
                    ? query.OrderByDescending(z => z.Numer)
                    : query.OrderBy(z => z.Numer),

                "Data" => descending
                    ? query.OrderByDescending(z => z.DataZamowienia)
                    : query.OrderBy(z => z.DataZamowienia),

                _ => descending
                    ? query.OrderByDescending(z => z.IdZamowieniaSprzedazy)
                    : query.OrderBy(z => z.IdZamowieniaSprzedazy)
            };
        }
    }
}
