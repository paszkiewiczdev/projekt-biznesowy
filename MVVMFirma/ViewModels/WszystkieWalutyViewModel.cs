using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieWalutyViewModel : WszystkieViewModel<Waluta>
    {
        private string _kod;
        private string _nazwa;
        private string _kursText;
        private bool _czyDomyslna;
        private bool _czyAktywna = true;

        public WszystkieWalutyViewModel()
            : base()
        {
            DisplayName = "Waluty";
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

        public string KursText
        {
            get => _kursText;
            set
            {
                if (_kursText != value)
                {
                    _kursText = value;
                    OnPropertyChanged(() => KursText);
                }
            }
        }

        public bool CzyDomyslna
        {
            get => _czyDomyslna;
            set
            {
                if (_czyDomyslna != value)
                {
                    _czyDomyslna = value;
                    OnPropertyChanged(() => CzyDomyslna);
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

        protected override IEnumerable<Waluta> LoadData()
        {
            return fakturyEntities.Waluta.ToList();
        }

        protected override bool MatchesFilter(Waluta item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdWaluty.ToString().Contains(text)
                   || (item.Kod != null && item.Kod.ToLowerInvariant().Contains(text))
                   || (item.Nazwa != null && item.Nazwa.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<Waluta> ApplySort(
            IEnumerable<Waluta> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Kod" => descending
                    ? query.OrderByDescending(w => w.Kod)
                    : query.OrderBy(w => w.Kod),

                "Nazwa" => descending
                    ? query.OrderByDescending(w => w.Nazwa)
                    : query.OrderBy(w => w.Nazwa),

                _ => descending
                    ? query.OrderByDescending(w => w.IdWaluty)
                    : query.OrderBy(w => w.IdWaluty)
            };
        }

        private void Dodaj()
        {
            var kurs = ParseKurs();
            var nowa = new Waluta
            {
                Kod = Kod,
                Nazwa = Nazwa,
                Kurs = kurs,
                CzyDomyslna = CzyDomyslna,
                CzyAktywna = CzyAktywna
            };

            fakturyEntities.Waluta.Add(nowa);
            fakturyEntities.SaveChanges();
            load();

            Kod = string.Empty;
            Nazwa = string.Empty;
            KursText = string.Empty;
            CzyDomyslna = false;
            CzyAktywna = true;
        }

        private bool CanDodaj()
        {
            return !string.IsNullOrWhiteSpace(Kod)
                   && !string.IsNullOrWhiteSpace(Nazwa)
                   && TryParseKurs(out _);
        }

        private decimal? ParseKurs()
        {
            if (TryParseKurs(out var kurs))
                return kurs;

            return null;
        }

        private bool TryParseKurs(out decimal kurs)
        {
            if (string.IsNullOrWhiteSpace(KursText))
            {
                kurs = 0m;
                return true;
            }

            return decimal.TryParse(
                KursText,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out kurs);
        }
    }
}
