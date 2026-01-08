using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieDokumentyMagazynoweViewModel : WszystkieViewModel<DokumentMagazynowy>
    {
        public WszystkieDokumentyMagazynoweViewModel()
        {
            DisplayName = "Dokumenty magazynowe";
            SetSortOptions(new[] { "Id", "Numer", "Data", "Magazyn" });
        }

        protected override IEnumerable<DokumentMagazynowy> LoadData()
        {
            return fakturyEntities.DokumentMagazynowy
                .Include(d => d.Magazyn)
                .Include(d => d.TypDokumentuMagazynowego)
                .ToList();
        }

        protected override bool MatchesFilter(DokumentMagazynowy item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdDokumentuMagazynowego.ToString().Contains(text)
                   || (item.Numer != null && item.Numer.ToLowerInvariant().Contains(text))
                   || (item.Magazyn != null
                       && item.Magazyn.Nazwa != null
                       && item.Magazyn.Nazwa.ToLowerInvariant().Contains(text))
                   || (item.TypDokumentuMagazynowego != null
                       && item.TypDokumentuMagazynowego.Nazwa != null
                       && item.TypDokumentuMagazynowego.Nazwa.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<DokumentMagazynowy> ApplySort(IEnumerable<DokumentMagazynowy> query, string sortField, bool descending)
        {
            return sortField switch
            {
                "Numer" => descending
                    ? query.OrderByDescending(d => d.Numer)
                    : query.OrderBy(d => d.Numer),

                "Data" => descending
                    ? query.OrderByDescending(d => d.DataDokumentu)
                    : query.OrderBy(d => d.DataDokumentu),

                "Magazyn" => descending
                    ? query.OrderByDescending(d => d.Magazyn != null ? d.Magazyn.Nazwa : null)
                    : query.OrderBy(d => d.Magazyn != null ? d.Magazyn.Nazwa : null),

                _ => descending
                    ? query.OrderByDescending(d => d.IdDokumentuMagazynowego)
                    : query.OrderBy(d => d.IdDokumentuMagazynowego)
            };
        }
    }
}
