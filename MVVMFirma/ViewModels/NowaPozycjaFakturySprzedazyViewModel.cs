using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
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

            if (!TryReduceStock(SelectedTowar.IdTowaru, ilosc))
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

            UpdateFakturaTotals(pozycja.IdFakturySprzedazy);

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
                    ApplyTowarDefaults();
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
                    RecalculateTotals();
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
                    RecalculateTotals();
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
                    RecalculateTotals();
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
                    nameof(WartoscVatText) => Validator.NonNegativeDecimal(WartoscVatText, "Wartość VAT"),
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

        private void ApplyTowarDefaults()
        {
            if (SelectedTowar == null)
                return;

            SetFieldValue(ref _cenaNettoText, FormatDecimal(SelectedTowar.Cena), () => CenaNettoText);

            if (SelectedTowar.IdStawkiVatSprzedazy.HasValue)
            {
                SelectedStawkaVat = StawkiVat.FirstOrDefault(v =>
                    v.IdStawkiVat == SelectedTowar.IdStawkiVatSprzedazy.Value);
            }

            RecalculateTotals();
        }

        private void RecalculateTotals()
        {
            if (!decimal.TryParse(IloscText, NumberStyles.Number, CultureInfo.CurrentCulture, out var ilosc))
                return;

            if (!decimal.TryParse(CenaNettoText, NumberStyles.Number, CultureInfo.CurrentCulture, out var cenaNetto))
                return;

            var wartoscNetto = ilosc * cenaNetto;
            var vatRate = SelectedStawkaVat?.Wartosc ?? 0m;
            var wartoscVat = wartoscNetto * vatRate / 100m;
            var wartoscBrutto = wartoscNetto + wartoscVat;

            SetFieldValue(ref _wartoscNettoText, FormatDecimal(wartoscNetto), () => WartoscNettoText);
            SetFieldValue(ref _wartoscVatText, FormatDecimal(wartoscVat), () => WartoscVatText);
            SetFieldValue(ref _wartoscBruttoText, FormatDecimal(wartoscBrutto), () => WartoscBruttoText);
        }

        private void UpdateFakturaTotals(int idFakturySprzedazy)
        {
            var totals = fakturyEntities.PozycjaFakturySprzedazy
                .Where(p => p.IdFakturySprzedazy == idFakturySprzedazy)
                .GroupBy(p => p.IdFakturySprzedazy)
                .Select(g => new
                {
                    Net = g.Sum(p => p.WartoscNetto) ?? 0m,
                    Vat = g.Sum(p => p.WartoscVat) ?? 0m,
                    Brutto = g.Sum(p => p.WartoscBrutto) ?? 0m
                })
                .FirstOrDefault();

            if (totals == null)
                return;

            var faktura = fakturyEntities.FakturaSprzedazy
                .FirstOrDefault(f => f.IdFakturySprzedazy == idFakturySprzedazy);

            if (faktura == null)
                return;

            faktura.RazemNetto = totals.Net;
            faktura.RazemVat = totals.Vat;
            faktura.RazemBrutto = totals.Brutto;
            fakturyEntities.SaveChanges();
        }

        private bool TryReduceStock(int towarId, decimal ilosc)
        {
            var stany = fakturyEntities.StanMagazynowy
                .Where(s => s.IdTowaru == towarId)
                .OrderByDescending(s => s.Ilosc)
                .ToList();

            if (!stany.Any())
            {
                MessageBox.Show("Brak stanu magazynowego dla wybranego towaru.");
                return false;
            }

            var dostepnaIlosc = stany.Sum(s => s.Ilosc);

            if (dostepnaIlosc < ilosc)
            {
                MessageBox.Show("Brak wystarczającego stanu magazynowego dla wybranego towaru.");
                return false;
            }

            var remaining = ilosc;

            foreach (var stan in stany)
            {
                if (remaining <= 0)
                    break;

                var doOdjecia = Math.Min(stan.Ilosc, remaining);
                stan.Ilosc -= doOdjecia;
                remaining -= doOdjecia;
            }

            return true;
        }

        private static string FormatDecimal(decimal value)
        {
            return value.ToString("0.00", CultureInfo.CurrentCulture);
        }

        private void SetFieldValue(ref string field, string value, System.Linq.Expressions.Expression<System.Func<string>> property)
        {
            if (field == value)
                return;

            field = value;
            OnPropertyChanged(property);
        }
    }
}
