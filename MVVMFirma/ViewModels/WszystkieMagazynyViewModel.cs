using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieMagazynyViewModel : WszystkieViewModel<Magazyn>
    {
        private string _kod;
        private string _nazwa;
        private string _opis;
        private bool _czyAktywny = true;

        public WszystkieMagazynyViewModel()
            : base()
        {
            DisplayName = "Magazyny";
            SetSortOptions(new[] { "Id", "Kod", "Nazwa" });
            AddCommand = new BaseCommand(Dodaj, CanDodaj);
            RefreshCommand = new BaseCommand(load);
        }

        public string Kod
        {
            get => _kod;
            set
            {
                if (_kod != value)
                {
                    _kod = value;
                    OnPropertyChanged(() => Kod);
                }
            }
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
        public ICommand RefreshCommand { get; }

        protected override IEnumerable<Magazyn> LoadData()
        {
            return fakturyEntities.Magazyn.ToList();
        }

        protected override bool MatchesFilter(Magazyn item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdMagazynu.ToString().Contains(text)
                   || (item.Kod != null && item.Kod.ToLowerInvariant().Contains(text))
                   || (item.Nazwa != null && item.Nazwa.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<Magazyn> ApplySort(IEnumerable<Magazyn> query, string sortField, bool descending)
        {
            return sortField switch
            {
                "Kod" => descending
                    ? query.OrderByDescending(m => m.Kod)
                    : query.OrderBy(m => m.Kod),

                "Nazwa" => descending
                    ? query.OrderByDescending(m => m.Nazwa)
                    : query.OrderBy(m => m.Nazwa),

                _ => descending
                    ? query.OrderByDescending(m => m.IdMagazynu)
                    : query.OrderBy(m => m.IdMagazynu)
            };
        }

        private void Dodaj()
        {
            var nowy = new Magazyn
            {
                Kod = Kod,
                Nazwa = Nazwa,
                Opis = Opis,
                CzyAktywny = CzyAktywny
            };

            fakturyEntities.Magazyn.Add(nowy);
            fakturyEntities.SaveChanges();

            load();

            Kod = string.Empty;
            Nazwa = string.Empty;
            Opis = string.Empty;
            CzyAktywny = true;
        }

        private bool CanDodaj()
        {
            return !string.IsNullOrWhiteSpace(Kod)
                && !string.IsNullOrWhiteSpace(Nazwa);
        }
    }
}
