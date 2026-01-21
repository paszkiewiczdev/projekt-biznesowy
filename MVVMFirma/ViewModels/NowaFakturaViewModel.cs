using System;
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
    public class NowaFakturaViewModel : JedenViewModel<FakturaSprzedazy>, IDataErrorInfo
    {
        public NowaFakturaViewModel()
        {
            DisplayName = "Nowa faktura";

            Item = new FakturaSprzedazy();
            Kontrahenci = new ObservableCollection<Kontrahent>(fakturyEntities.Kontrahent.ToList());
            SposobyPlatnosci = new ObservableCollection<SposobPlatnosci>(fakturyEntities.SposobPlatnosci.ToList());
            StatusyFaktur = new ObservableCollection<StatusFaktury>(fakturyEntities.StatusFaktury.ToList());
            Waluty = new ObservableCollection<Waluta>(fakturyEntities.Waluta.ToList());

            RazemNettoText = FormatDecimal(0m);
            RazemVatText = FormatDecimal(0m);
            RazemBruttoText = FormatDecimal(0m);
        }

        public override void Save()
        {
            if (!IsValid())
                return;

            var razemNetto = ParseDecimalOrZero(RazemNettoText);
            var razemVat = ParseDecimalOrZero(RazemVatText);
            var razemBrutto = ParseDecimalOrZero(RazemBruttoText);

            if (!DataWystawienia.HasValue || !DataSprzedazy.HasValue)
                return;

            var faktura = new FakturaSprzedazy
            {
                Numer = Numer?.Trim(),
                DataWystawienia = DataWystawienia.Value,
                DataSprzedazy = DataSprzedazy.Value,
                TerminPlatnosci = TerminPlatnosci,
                IdKontrahenta = SelectedKontrahent.IdKontrahenta,
                IdSposobuPlatnosci = SelectedSposobPlatnosci?.IdSposobuPlatnosci,
                IdStatusuFaktury = SelectedStatusFaktury?.IdStatusuFaktury,
                IdWaluty = SelectedWaluta?.IdWaluty,
                RazemNetto = razemNetto,
                RazemVat = razemVat,
                RazemBrutto = razemBrutto
            };

            fakturyEntities.FakturaSprzedazy.Add(faktura);
            fakturyEntities.SaveChanges();

            WasSaved = true;
            OnRequestClose();
        }

        private string _numer;
        private DateTime? _dataWystawienia;
        private DateTime? _dataSprzedazy;
        private DateTime? _terminPlatnosci;
        private string _razemNettoText;
        private string _razemVatText;
        private string _razemBruttoText;
        private Kontrahent _selectedKontrahent;
        private SposobPlatnosci _selectedSposobPlatnosci;
        private StatusFaktury _selectedStatusFaktury;
        private Waluta _selectedWaluta;

        public string Numer
        {
            get => _numer;
            set
            {
                if (_numer != value)
                {
                    _numer = value;
                    OnPropertyChanged(() => Numer);
                }
            }
        }

        public DateTime? DataWystawienia
        {
            get => _dataWystawienia;
            set
            {
                if (_dataWystawienia != value)
                {
                    _dataWystawienia = value;
                    OnPropertyChanged(() => DataWystawienia);
                }
            }
        }

        public DateTime? DataSprzedazy
        {
            get => _dataSprzedazy;
            set
            {
                if (_dataSprzedazy != value)
                {
                    _dataSprzedazy = value;
                    OnPropertyChanged(() => DataSprzedazy);
                }
            }
        }

        public DateTime? TerminPlatnosci
        {
            get => _terminPlatnosci;
            set
            {
                if (_terminPlatnosci != value)
                {
                    _terminPlatnosci = value;
                    OnPropertyChanged(() => TerminPlatnosci);
                }
            }
        }

        public string RazemNettoText
        {
            get => _razemNettoText;
            set
            {
                if (_razemNettoText != value)
                {
                    _razemNettoText = value;
                    OnPropertyChanged(() => RazemNettoText);
                }
            }
        }

        public string RazemVatText
        {
            get => _razemVatText;
            set
            {
                if (_razemVatText != value)
                {
                    _razemVatText = value;
                    OnPropertyChanged(() => RazemVatText);
                }
            }
        }

        public string RazemBruttoText
        {
            get => _razemBruttoText;
            set
            {
                if (_razemBruttoText != value)
                {
                    _razemBruttoText = value;
                    OnPropertyChanged(() => RazemBruttoText);
                }
            }
        }

        public ObservableCollection<Kontrahent> Kontrahenci { get; }

        public ObservableCollection<SposobPlatnosci> SposobyPlatnosci { get; }

        public ObservableCollection<StatusFaktury> StatusyFaktur { get; }

        public ObservableCollection<Waluta> Waluty { get; }

        public Kontrahent SelectedKontrahent
        {
            get => _selectedKontrahent;
            set
            {
                if (_selectedKontrahent != value)
                {
                    _selectedKontrahent = value;
                    OnPropertyChanged(() => SelectedKontrahent);
                }
            }
        }

        public SposobPlatnosci SelectedSposobPlatnosci
        {
            get => _selectedSposobPlatnosci;
            set
            {
                if (_selectedSposobPlatnosci != value)
                {
                    _selectedSposobPlatnosci = value;
                    OnPropertyChanged(() => SelectedSposobPlatnosci);
                }
            }
        }

        public StatusFaktury SelectedStatusFaktury
        {
            get => _selectedStatusFaktury;
            set
            {
                if (_selectedStatusFaktury != value)
                {
                    _selectedStatusFaktury = value;
                    OnPropertyChanged(() => SelectedStatusFaktury);
                }
            }
        }

        public Waluta SelectedWaluta
        {
            get => _selectedWaluta;
            set
            {
                if (_selectedWaluta != value)
                {
                    _selectedWaluta = value;
                    OnPropertyChanged(() => SelectedWaluta);
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
                    nameof(Numer) => StringValidator.RequiredAndLength(Numer, "Numer", 2, 50),
                    nameof(DataWystawienia) => Validator.Required(DataWystawienia, "Data wystawienia"),
                    nameof(DataSprzedazy) => Validator.Required(DataSprzedazy, "Data sprzedaży"),
                    nameof(TerminPlatnosci) => Validator.Required(TerminPlatnosci, "Termin płatności"),
                    nameof(SelectedKontrahent) => Validator.Required(SelectedKontrahent, "Kontrahent"),
                    nameof(SelectedSposobPlatnosci) => Validator.Required(SelectedSposobPlatnosci, "Sposób płatności"),
                    nameof(SelectedStatusFaktury) => Validator.Required(SelectedStatusFaktury, "Status faktury"),
                    nameof(SelectedWaluta) => Validator.Required(SelectedWaluta, "Waluta"),
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
                nameof(Numer),
                nameof(DataWystawienia),
                nameof(DataSprzedazy),
                nameof(TerminPlatnosci),
                nameof(SelectedKontrahent),
                nameof(SelectedSposobPlatnosci),
                nameof(SelectedStatusFaktury),
                nameof(SelectedWaluta)
            };

        private static decimal ParseDecimalOrZero(string value)
        {
            return decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out var parsed)
                ? parsed
                : 0m;
        }

        private static string FormatDecimal(decimal value)
        {
            return value.ToString("0.00", CultureInfo.CurrentCulture);
        }
    }
}
