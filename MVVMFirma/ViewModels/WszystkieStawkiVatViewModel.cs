using System.Collections.ObjectModel;
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
            // tytuł zakładki
            base.DisplayName = "Stawki VAT";
            AddCommand = new BaseCommand(Dodaj, CanDodaj);
        }

        #region Właściwości bindowane

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

        #endregion

        #region Komendy

        public ICommand AddCommand { get; }

        #endregion

        public override void load()
        {
            List = new ObservableCollection<StawkaVat>(
                from s in fakturyEntities.StawkaVat
                select s
            );
        }

        private void Dodaj()
        {
            if (!TryParseWartosc(out var wartosc))
            {
                return;
            }

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