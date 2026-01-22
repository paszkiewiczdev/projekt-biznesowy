using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieTypyKontrahentowViewModel : WszystkieViewModel<TypKontrahenta>
    {
        private string _nazwa;
        private bool _czyAktywny = true;

        public WszystkieTypyKontrahentowViewModel()
            : base()
        {
            DisplayName = "Typy kontrahentów";
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

        protected override IEnumerable<TypKontrahenta> LoadData()
        {
            return fakturyEntities.TypKontrahenta.ToList();
        }

        protected override bool MatchesFilter(TypKontrahenta item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdTypuKontrahenta.ToString().Contains(text)
                   || (item.Nazwa != null && item.Nazwa.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<TypKontrahenta> ApplySort(
            IEnumerable<TypKontrahenta> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Nazwa" => descending
                    ? query.OrderByDescending(t => t.Nazwa)
                    : query.OrderBy(t => t.Nazwa),

                _ => descending
                    ? query.OrderByDescending(t => t.IdTypuKontrahenta)
                    : query.OrderBy(t => t.IdTypuKontrahenta)
            };
        }

        private void Dodaj()
        {
            var nowy = new TypKontrahenta
            {
                Nazwa = Nazwa,
                CzyAktywny = CzyAktywny
            };

            fakturyEntities.TypKontrahenta.Add(nowy);
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
