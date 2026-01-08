using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieTypyDokumentuMagazynowegoViewModel
        : WszystkieViewModel<TypDokumentuMagazynowego>
    {
        private string _kod;
        private string _nazwa;
        private bool _czyAktywny = true;

        public WszystkieTypyDokumentuMagazynowegoViewModel()
            : base()
        {
            DisplayName = "Typy dokumentów magazynowych";
            SetSortOptions(new[] { "Id", "Kod", "Nazwa" });
            AddCommand = new BaseCommand(Dodaj, CanDodaj);
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

        protected override IEnumerable<TypDokumentuMagazynowego> LoadData()
        {
            return fakturyEntities.TypDokumentuMagazynowego.ToList();
        }

        protected override bool MatchesFilter(TypDokumentuMagazynowego item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdTypuDokumentuMagazynowego.ToString().Contains(text)
                   || (item.Kod != null && item.Kod.ToLowerInvariant().Contains(text))
                   || (item.Nazwa != null && item.Nazwa.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<TypDokumentuMagazynowego> ApplySort(
            IEnumerable<TypDokumentuMagazynowego> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Kod" => descending
                    ? query.OrderByDescending(t => t.Kod)
                    : query.OrderBy(t => t.Kod),

                "Nazwa" => descending
                    ? query.OrderByDescending(t => t.Nazwa)
                    : query.OrderBy(t => t.Nazwa),

                _ => descending
                    ? query.OrderByDescending(t => t.IdTypuDokumentuMagazynowego)
                    : query.OrderBy(t => t.IdTypuDokumentuMagazynowego)
            };
        }

        private void Dodaj()
        {
            var nowy = new TypDokumentuMagazynowego
            {
                Kod = Kod,
                Nazwa = Nazwa,
                CzyAktywny = CzyAktywny
            };

            fakturyEntities.TypDokumentuMagazynowego.Add(nowy);
            fakturyEntities.SaveChanges();
            load();

            Kod = string.Empty;
            Nazwa = string.Empty;
            CzyAktywny = true;
        }

        private bool CanDodaj()
        {
            return !string.IsNullOrWhiteSpace(Kod)
                   && !string.IsNullOrWhiteSpace(Nazwa);
        }
    }
}
