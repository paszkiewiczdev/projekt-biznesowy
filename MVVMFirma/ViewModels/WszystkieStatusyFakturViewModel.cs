using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieStatusyFakturViewModel : WszystkieViewModel<StatusFaktury>
    {
        private string _nazwa;
        private string _opis;
        private bool _czyAktywny = true;

        public WszystkieStatusyFakturViewModel()
            : base()
        {
            DisplayName = "Statusy faktur";
            SetSortOptions(new[] { "Id", "Nazwa" });
            AddCommand = new BaseCommand(Dodaj, CanDodaj);
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

        public string Opis
        {
            get => _opis;
            set
            {
                if (_opis != value)
                {
                    _opis = value;
                    OnPropertyChanged(() => Opis);
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

        protected override IEnumerable<StatusFaktury> LoadData()
        {
            return fakturyEntities.StatusFaktury.ToList();
        }

        protected override bool MatchesFilter(StatusFaktury item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdStatusuFaktury.ToString().Contains(text)
                   || (item.Nazwa != null && item.Nazwa.ToLowerInvariant().Contains(text))
                   || (item.Opis != null && item.Opis.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<StatusFaktury> ApplySort(
            IEnumerable<StatusFaktury> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Nazwa" => descending
                    ? query.OrderByDescending(s => s.Nazwa)
                    : query.OrderBy(s => s.Nazwa),

                _ => descending
                    ? query.OrderByDescending(s => s.IdStatusuFaktury)
                    : query.OrderBy(s => s.IdStatusuFaktury)
            };
        }

        private void Dodaj()
        {
            var nowy = new StatusFaktury
            {
                Nazwa = Nazwa,
                Opis = Opis,
                CzyAktywny = CzyAktywny
            };

            fakturyEntities.StatusFaktury.Add(nowy);
            fakturyEntities.SaveChanges();
            load();

            Nazwa = string.Empty;
            Opis = string.Empty;
            CzyAktywny = true;
        }

        private bool CanDodaj()
        {
            return !string.IsNullOrWhiteSpace(Nazwa);
        }
    }
}
