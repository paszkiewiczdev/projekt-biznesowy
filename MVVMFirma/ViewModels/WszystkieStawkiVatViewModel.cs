using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieStawkiVatViewModel : WszystkieViewModel<StawkaVat>
    {
        private string _nazwa;
        private string _wartoscText;
        private bool _czyAktywna = true;

        public WszystkieStawkiVatViewModel()
            : base()
        {
            DisplayName = "Stawki VAT";
            SetSortOptions(new[] { "Id", "Nazwa", "Wartość" });
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

        public string WartoscText
        {
            get => _wartoscText;
            set
            {
                if (_wartoscText != value)
                {
                    _wartoscText = value;
                    OnPropertyChanged(() => WartoscText);
                }
            }
        }

        public bool CzyAktywna
        {
            get => _czyAktywna;
            set
            {
                if (_czyAktywna != value)
                {
                    _czyAktywna = value;
                    OnPropertyChanged(() => CzyAktywna);
                }
            }
        }

        public ICommand AddCommand { get; }
        public ICommand RefreshCommand { get; }

        protected override IEnumerable<StawkaVat> LoadData()
        {
            return fakturyEntities.StawkaVat.ToList();
        }

        protected override bool MatchesFilter(StawkaVat item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdStawkiVat.ToString().Contains(text)
                   || (item.Nazwa != null && item.Nazwa.ToLowerInvariant().Contains(text))
                   || item.Wartosc.ToString(CultureInfo.CurrentCulture).ToLowerInvariant().Contains(text);
        }

        protected override IOrderedEnumerable<StawkaVat> ApplySort(
            IEnumerable<StawkaVat> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Nazwa" => descending
                    ? query.OrderByDescending(s => s.Nazwa)
                    : query.OrderBy(s => s.Nazwa),

                "Wartość" => descending
                    ? query.OrderByDescending(s => s.Wartosc)
                    : query.OrderBy(s => s.Wartosc),

                _ => descending
                    ? query.OrderByDescending(s => s.IdStawkiVat)
                    : query.OrderBy(s => s.IdStawkiVat)
            };
        }

        private void Dodaj()
        {
            if (!TryParseWartosc(out var wartosc))
                return;

            var nowa = new StawkaVat
            {
                Nazwa = Nazwa,
                Wartosc = wartosc,
                CzyAktywna = CzyAktywna
            };

            fakturyEntities.StawkaVat.Add(nowa);
            fakturyEntities.SaveChanges();
            load();

            Nazwa = string.Empty;
            WartoscText = string.Empty;
            CzyAktywna = true;
        }

        private bool CanDodaj()
        {
            return !string.IsNullOrWhiteSpace(Nazwa)
                   && TryParseWartosc(out _);
        }

        private bool TryParseWartosc(out decimal wartosc)
        {
            return decimal.TryParse(
                WartoscText,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out wartosc);
        }
    }
}
