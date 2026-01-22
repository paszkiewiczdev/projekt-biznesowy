using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieStatusyZamowienViewModel : WszystkieViewModel<StatusZamowienia>
    {
        private string _nazwa;
        private bool _czyAktywny = true;

        public WszystkieStatusyZamowienViewModel()
            : base()
        {
            DisplayName = "Statusy zamówień";
            SetSortOptions(new[] { "Id", "Nazwa" });
            AddCommand = new BaseCommand(Dodaj, CanDodaj);
            RefreshCommand = new BaseCommand(load);
        }

        public string Nazwa
        {
            get => _nazwa;
            set
            {
                if (_nazwa != value)
                {
                    _nazwa = value;
                    OnPropertyChanged(() => Nazwa);
                }
            }
        }

        public bool CzyAktywny
        {
            get => _czyAktywny;
            set
            {
                if (_czyAktywny != value)
                {
                    _czyAktywny = value;
                    OnPropertyChanged(() => CzyAktywny);
                }
            }
        }

        public ICommand AddCommand { get; }
        public ICommand RefreshCommand { get; }

        protected override IEnumerable<StatusZamowienia> LoadData()
        {
            return fakturyEntities.StatusZamowienia.ToList();
        }

        protected override bool MatchesFilter(StatusZamowienia item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdStatusuZamowienia.ToString().Contains(text)
                   || (item.Nazwa != null && item.Nazwa.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<StatusZamowienia> ApplySort(
            IEnumerable<StatusZamowienia> query,
            string sortField,
            bool descending)
        {
            if (sortField == "Nazwa")
            {
                return descending
                    ? query.OrderByDescending(s => s.Nazwa)
                    : query.OrderBy(s => s.Nazwa);
            }

            return descending
                ? query.OrderByDescending(s => s.IdStatusuZamowienia)
                : query.OrderBy(s => s.IdStatusuZamowienia);
        }

        private void Dodaj()
        {
            var nowy = new StatusZamowienia
            {
                Nazwa = Nazwa,
                CzyAktywny = CzyAktywny
            };

            fakturyEntities.StatusZamowienia.Add(nowy);
            fakturyEntities.SaveChanges();
            load();

            Nazwa = string.Empty;
            CzyAktywny = true;
        }

        private bool CanDodaj()
        {
            return !string.IsNullOrWhiteSpace(Nazwa);
        }
    }
}
