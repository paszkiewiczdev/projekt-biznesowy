using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using MVVMFirma.Helper;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class NowaPozycjaZamowieniaSprzedazyViewModel : JedenViewModel<PozycjaZamowieniaSprzedazy>, IDataErrorInfo
    {
        public NowaPozycjaZamowieniaSprzedazyViewModel()
        {
            DisplayName = "Nowa pozycja zamówienia";

            Item = new PozycjaZamowieniaSprzedazy();
            Zamowienia = new ObservableCollection<ZamowienieSprzedazy>(fakturyEntities.ZamowienieSprzedazy.ToList());
            Towary = new ObservableCollection<Towar>(fakturyEntities.Towar.ToList());
        }

        public override void Save()
        {
            if (!IsValid())
                return;

            if (!decimal.TryParse(IloscText, NumberStyles.Number, CultureInfo.CurrentCulture, out var ilosc))
                return;

            if (!decimal.TryParse(CenaNettoText, NumberStyles.Number, CultureInfo.CurrentCulture, out var cenaNetto))
                return;

            if (SelectedZamowienie == null || SelectedTowar == null)
                return;

            var pozycja = new PozycjaZamowieniaSprzedazy
            {
                IdZamowieniaSprzedazy = SelectedZamowienie.IdZamowieniaSprzedazy,
                IdTowaru = SelectedTowar.IdTowaru,
                Ilosc = ilosc,
                CenaNetto = cenaNetto
            };

            fakturyEntities.PozycjaZamowieniaSprzedazy.Add(pozycja);
            fakturyEntities.SaveChanges();

            WasSaved = true;
            OnRequestClose();
        }

        private ZamowienieSprzedazy _selectedZamowienie;
        private Towar _selectedTowar;
        private string _iloscText;
        private string _cenaNettoText;

        public ObservableCollection<ZamowienieSprzedazy> Zamowienia { get; }

        public ObservableCollection<Towar> Towary { get; }

        public ZamowienieSprzedazy SelectedZamowienie
        {
            get => _selectedZamowienie;
            set
            {
                if (_selectedZamowienie != value)
                {
                    _selectedZamowienie = value;
                    OnPropertyChanged(() => SelectedZamowienie);
                }
            }
        }

        public Towar SelectedTowar
        {
            get => _selectedTowar;
            set
            {
                if (_selectedTowar != value)
                {
                    _selectedTowar = value;
                    OnPropertyChanged(() => SelectedTowar);
                }
            }
        }

        public string IloscText
        {
            get => _iloscText;
            set
            {
                if (_iloscText != value)
                {
                    _iloscText = value;
                    OnPropertyChanged(() => IloscText);
                }
            }
        }

        public string CenaNettoText
        {
            get => _cenaNettoText;
            set
            {
                if (_cenaNettoText != value)
                {
                    _cenaNettoText = value;
                    OnPropertyChanged(() => CenaNettoText);
                }
            }
        }

        public bool WasSaved { get; private set; }

        public string Error => null;

        public string this[string name]
        {
            get
            {
                return name switch
                {
                    nameof(SelectedZamowienie) => Validator.Required(SelectedZamowienie, "Zamówienie"),
                    nameof(SelectedTowar) => Validator.Required(SelectedTowar, "Towar"),
                    nameof(IloscText) => Validator.PositiveDecimal(IloscText, "Ilość"),
                    nameof(CenaNettoText) => Validator.PositiveDecimal(CenaNettoText, "Cena netto"),
                    _ => null
                };
            }
        }

        private bool IsValid()
        {
            foreach (var property in ValidatedProperties)
            {
                if (!string.IsNullOrEmpty(this[property]))
                    return false;
            }

            return true;
        }

        private static IEnumerable<string> ValidatedProperties =>
            new[]
            {
                nameof(SelectedZamowienie),
                nameof(SelectedTowar),
                nameof(IloscText),
                nameof(CenaNettoText)
            };
    }
}