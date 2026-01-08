using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieStanyMagazynoweViewModel : WszystkieViewModel<StanMagazynowy>
    {
        public WszystkieStanyMagazynoweViewModel()
        {
            DisplayName = "Stany magazynowe";
            SetSortOptions(new[] { "Id", "Magazyn", "Towar", "Ilość" });
        }

        protected override IEnumerable<StanMagazynowy> LoadData()
        {
            return fakturyEntities.StanMagazynowy
                .Include(s => s.Magazyn)
                .Include(s => s.Towar)
                .ToList();
        }

        protected override bool MatchesFilter(StanMagazynowy item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdStanuMagazynowego.ToString().Contains(text)
                   || (item.Magazyn != null
                       && item.Magazyn.Nazwa != null
                       && item.Magazyn.Nazwa.ToLowerInvariant().Contains(text))
                   || (item.Towar != null
                       && item.Towar.Nazwa != null
                       && item.Towar.Nazwa.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<StanMagazynowy> ApplySort(
            IEnumerable<StanMagazynowy> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Magazyn" => descending
                    ? query.OrderByDescending(s => s.Magazyn != null ? s.Magazyn.Nazwa : null)
                    : query.OrderBy(s => s.Magazyn != null ? s.Magazyn.Nazwa : null),

                "Towar" => descending
                    ? query.OrderByDescending(s => s.Towar != null ? s.Towar.Nazwa : null)
                    : query.OrderBy(s => s.Towar != null ? s.Towar.Nazwa : null),

                "Ilość" => descending
                    ? query.OrderByDescending(s => s.Ilosc)
                    : query.OrderBy(s => s.Ilosc),

                _ => descending
                    ? query.OrderByDescending(s => s.IdStanuMagazynowego)
                    : query.OrderBy(s => s.IdStanuMagazynowego)
            };
        }
    }
}
