using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkiePozycjeDokumentuMagazynowegoViewModel
        : WszystkieViewModel<PozycjaDokumentuMagazynowego>
    {
        public WszystkiePozycjeDokumentuMagazynowegoViewModel()
        {
            DisplayName = "Pozycje dokumentów magazynowych";
            SetSortOptions(new[] { "Id", "Dokument", "Towar", "Ilość" });
        }

        protected override IEnumerable<PozycjaDokumentuMagazynowego> LoadData()
        {
            return fakturyEntities.PozycjaDokumentuMagazynowego
                .Include(p => p.DokumentMagazynowy)
                .Include(p => p.Towar)
                .ToList();
        }

        protected override bool MatchesFilter(PozycjaDokumentuMagazynowego item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdPozycjiDokumentuMagazynowego.ToString().Contains(text)
                   || (item.DokumentMagazynowy != null
                       && item.DokumentMagazynowy.Numer != null
                       && item.DokumentMagazynowy.Numer.ToLowerInvariant().Contains(text))
                   || (item.Towar != null
                       && item.Towar.Nazwa != null
                       && item.Towar.Nazwa.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<PozycjaDokumentuMagazynowego> ApplySort(
            IEnumerable<PozycjaDokumentuMagazynowego> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Dokument" => descending
                    ? query.OrderByDescending(p => p.DokumentMagazynowy != null ? p.DokumentMagazynowy.Numer : null)
                    : query.OrderBy(p => p.DokumentMagazynowy != null ? p.DokumentMagazynowy.Numer : null),

                "Towar" => descending
                    ? query.OrderByDescending(p => p.Towar != null ? p.Towar.Nazwa : null)
                    : query.OrderBy(p => p.Towar != null ? p.Towar.Nazwa : null),

                "Ilość" => descending
                    ? query.OrderByDescending(p => p.Ilosc)
                    : query.OrderBy(p => p.Ilosc),

                _ => descending
                    ? query.OrderByDescending(p => p.IdPozycjiDokumentuMagazynowego)
                    : query.OrderBy(p => p.IdPozycjiDokumentuMagazynowego)
            };
        }
    }
}
