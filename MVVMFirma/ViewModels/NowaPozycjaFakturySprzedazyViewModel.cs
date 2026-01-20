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
    public class NowaPozycjaFakturySprzedazyViewModel : JedenViewModel<PozycjaFakturySprzedazy>, IDataErrorInfo
    {
        public NowaPozycjaFakturySprzedazyViewModel()
        {
            DisplayName = "Nowa pozycja faktury";

            Item = new PozycjaFakturySprzedazy();
            Faktury = new ObservableCollection<FakturaSprzedazy>(fakturyEntities.FakturaSprzedazy.ToList());
            Towary = new ObservableCollection<Towar>(fakturyEntities.Towar.ToList());
            StawkiVat = new ObservableCollection<StawkaVat>(fakturyEntities.StawkaVat.ToList());
        }

        public override void Save()
        {
            if (!IsValid())
                return;

            if (!decimal.TryParse(IloscText, NumberStyles.Number, CultureInfo.CurrentCulture, out var ilosc))
                return;

            if (!decimal.TryParse(CenaNettoText, NumberStyles.Number, CultureInfo.CurrentCulture, out var cenaNetto))
                return;

            if (!decimal.TryParse(WartoscNettoText, NumberStyles.Number, CultureInfo.CurrentCulture, out var wartoscNetto))
                return;

            if (!decimal.TryParse(WartoscVatText, NumberStyles.Number, CultureInfo.CurrentCulture, out var wartoscVat))
                return;

            if (!decimal.TryParse(WartoscBruttoText, NumberStyles.Number, CultureInfo.CurrentCulture, out var wartoscBrutto))
                return;

            if (SelectedFaktura == null || SelectedTowar == null || SelectedStawkaVat == null)
                return;

            var pozycja = new PozycjaFakturySprzedazy
            {
                IdFakturySprzedazy = SelectedFaktura.IdFakturySprzedazy,
                IdTowaru = SelectedTowar.IdTowaru,
                IdStawkiVat = SelectedStawkaVat.IdStawkiVat,
                Ilosc = ilosc,
                CenaNetto = cenaNetto,
                WartoscNetto = wartoscNetto,
                WartoscVat = wartoscVat,
                WartoscBrutto = wartoscBrutto
            };

            fakturyEntities.PozycjaFakturySprzedazy.Add(pozycja);
            fakturyEntities.SaveChanges();

            WasSaved = true;
            OnRequestClose();
        }

        private FakturaSprzedazy _selectedFaktura;
        private Towar _selectedTowar;
        private StawkaVat _selectedStawkaVat;
        private string _iloscText;
        private string _cenaNettoText;
        private string _wartoscNettoText;
        private string _wartoscVatText;
        private string _wartoscBruttoText;

        public ObservableCollection<FakturaSprzedazy> Faktury { get; }

        public ObservableCollection<Towar> Towary { get; }

        public ObservableCollection<StawkaVat> StawkiVat { get; }

        public FakturaSprzedazy SelectedFaktura
        {
            get => _selectedFaktura;
            set
            {
                if (_selectedFaktura != value)
                {
                    _selectedFaktura = value;
                    OnPropertyChanged(() => SelectedFaktura);
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

        public StawkaVat SelectedStawkaVat
        {
            get => _selectedStawkaVat;
            set
            {
                if (_selectedStawkaVat != value)
                {
                    _selectedStawkaVat = value;
                    OnPropertyChanged(() => SelectedStawkaVat);
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

        public string WartoscNettoText
        {
            get => _wartoscNettoText;
            set
            {
                if (_wartoscNettoText != value)
                {
                    _wartoscNettoText = value;
                    OnPropertyChanged(() => WartoscNettoText);
                }
            }
        }

        public string WartoscVatText
        {
            get => _wartoscVatText;
            set
            {
                if (_wartoscVatText != value)
                {
                    _wartoscVatText = value;
                    OnPropertyChanged(() => WartoscVatText);
                }
            }
        }

        public string WartoscBruttoText
        {
            get => _wartoscBruttoText;
            set
            {
                if (_wartoscBruttoText != value)
                {
                    _wartoscBruttoText = value;
                    OnPropertyChanged(() => WartoscBruttoText);
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
                    nameof(SelectedFaktura) => Validator.Required(SelectedFaktura, "Faktura"),
                    nameof(SelectedTowar) => Validator.Required(SelectedTowar, "Towar"),
                    nameof(SelectedStawkaVat) => Validator.Required(SelectedStawkaVat, "Stawka VAT"),
                    nameof(IloscText) => Validator.PositiveDecimal(IloscText, "Ilość"),
                    nameof(CenaNettoText) => Validator.PositiveDecimal(CenaNettoText, "Cena netto"),
                    nameof(WartoscNettoText) => Validator.PositiveDecimal(WartoscNettoText, "Wartość netto"),
                    nameof(WartoscVatText) => Validator.PositiveDecimal(WartoscVatText, "Wartość VAT"),
                    nameof(WartoscBruttoText) => Validator.PositiveDecimal(WartoscBruttoText, "Wartość brutto"),
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
                nameof(SelectedFaktura),
                nameof(SelectedTowar),
                nameof(SelectedStawkaVat),
                nameof(IloscText),
                nameof(CenaNettoText),
                nameof(WartoscNettoText),
                nameof(WartoscVatText),
                nameof(WartoscBruttoText)
            };
    }
}