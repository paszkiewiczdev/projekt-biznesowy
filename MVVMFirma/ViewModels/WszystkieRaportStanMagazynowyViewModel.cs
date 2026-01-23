using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models.BusinessViews;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieRaportStanMagazynowyViewModel : WszystkieViewModel<RaportStanMagazynowyDto>
    {
        private decimal _lowStockThreshold = 5;
        private bool _showOnlyLowStock;

        public WszystkieRaportStanMagazynowyViewModel()
            : base()
        {
            DisplayName = "Raport: Stan magazynowy";
            SetSortOptions(new[] { "Id", "Magazyn", "Towar", "Ilość" });
            RefreshCommand = new BaseCommand(load);
        }

        public ICommand RefreshCommand { get; }

        public decimal LowStockThreshold
        {
            get => _lowStockThreshold;
            set
            {
                if (_lowStockThreshold != value)
                {
                    _lowStockThreshold = value < 0 ? 0 : value;
                    OnPropertyChanged(() => LowStockThreshold);
                    UpdateLowStockFlags();
                    ApplyFilterAndSort();
                }
            }
        }

        public bool ShowOnlyLowStock
        {
            get => _showOnlyLowStock;
            set
            {
                if (_showOnlyLowStock != value)
                {
                    _showOnlyLowStock = value;
                    OnPropertyChanged(() => ShowOnlyLowStock);
                    ApplyFilterAndSort();
                }
            }
        }

        protected override IEnumerable<RaportStanMagazynowyDto> LoadData()
        {
            return fakturyEntities.Database.SqlQuery<RaportStanMagazynowyDto>(
                "SELECT MagazynKod, MagazynNazwa, IdTowaru, TowarNazwa, Ilosc FROM dbo.v_RaportStanMagazynowy"
            ).ToList();
        }

        protected override bool MatchesFilter(RaportStanMagazynowyDto item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdTowaru.ToString().Contains(text)
                   || (item.MagazynNazwa != null && item.MagazynNazwa.ToLowerInvariant().Contains(text))
                   || (item.TowarNazwa != null && item.TowarNazwa.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<RaportStanMagazynowyDto> ApplySort(
            IEnumerable<RaportStanMagazynowyDto> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Magazyn" => descending
                    ? query.OrderByDescending(r => r.MagazynNazwa)
                    : query.OrderBy(r => r.MagazynNazwa),

                "Towar" => descending
                    ? query.OrderByDescending(r => r.TowarNazwa)
                    : query.OrderBy(r => r.TowarNazwa),

                "Ilość" => descending
                    ? query.OrderByDescending(r => r.Ilosc)
                    : query.OrderBy(r => r.Ilosc),

                _ => descending
                    ? query.OrderByDescending(r => r.IdTowaru)
                    : query.OrderBy(r => r.IdTowaru)
            };
        }

        protected override void ApplyFilterAndSort()
        {
            if (AllItems == null)
                return;

            UpdateLowStockFlags();

            IEnumerable<RaportStanMagazynowyDto> query = AllItems;

            if (ShowOnlyLowStock)
            {
                query = query.Where(item => item.IsLowStock);
            }

            if (!string.IsNullOrWhiteSpace(FilterText))
            {
                var filter = FilterText.Trim();
                query = query.Where(item => MatchesFilter(item, filter));
            }

            EnsureDefaultSortField();

            var sorted = ApplySort(query, SelectedSortField, SortDescending) ?? query.OrderBy(x => 0);

            List = new System.Collections.ObjectModel.ObservableCollection<RaportStanMagazynowyDto>(sorted);
        }

        private void UpdateLowStockFlags()
        {
            if (AllItems == null)
                return;

            foreach (var item in AllItems)
            {
                item.MinimalnyStan = LowStockThreshold;
                item.IsLowStock = item.Ilosc < LowStockThreshold;
                item.StatusLabel = item.IsLowStock ? "NISKI" : "OK";
            }
        }
    }
}
