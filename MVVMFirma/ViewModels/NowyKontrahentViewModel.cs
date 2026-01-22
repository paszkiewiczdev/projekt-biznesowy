using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using MVVMFirma.Helper;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class NowyKontrahentViewModel : JedenViewModel<Kontrahent>, IDataErrorInfo
    {
        public NowyKontrahentViewModel()
            : base()
        {
            DisplayName = "Nowy kontrahent";
            Item = new Kontrahent();

            TypyKontrahentow = new ObservableCollection<TypKontrahenta>(
                fakturyEntities.TypKontrahenta.ToList());

            CzyAktywny = true;
            Kraj = "Polska";
        }

        public ObservableCollection<TypKontrahenta> TypyKontrahentow { get; }

        private string _nip;
        private string _nazwaPelna;
        private string _nazwaSkrocona;
        private string _telefon;
        private string _email;
        private bool _czyAktywny;
        private TypKontrahenta _selectedTypKontrahenta;
        private string _ulica;
        private string _nrDomu;
        private string _nrLokalu;
        private string _kodPocztowy;
        private string _miasto;
        private string _kraj;

        public string NIP
        {
            get => _nip;
            set
            {
                if (_nip != value)
                {
                    _nip = value;
                    OnPropertyChanged(() => NIP);
                }
            }
        }

        public string NazwaPelna
        {
            get => _nazwaPelna;
            set
            {
                if (_nazwaPelna != value)
                {
                    _nazwaPelna = value;
                    OnPropertyChanged(() => NazwaPelna);
                }
            }
        }

        public string NazwaSkrocona
        {
            get => _nazwaSkrocona;
            set
            {
                if (_nazwaSkrocona != value)
                {
                    _nazwaSkrocona = value;
                    OnPropertyChanged(() => NazwaSkrocona);
                }
            }
        }

        public string Telefon
        {
            get => _telefon;
            set
            {
                if (_telefon != value)
                {
                    _telefon = value;
                    OnPropertyChanged(() => Telefon);
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(() => Email);
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

        public TypKontrahenta SelectedTypKontrahenta
        {
            get => _selectedTypKontrahenta;
            set
            {
                if (_selectedTypKontrahenta != value)
                {
                    _selectedTypKontrahenta = value;
                    OnPropertyChanged(() => SelectedTypKontrahenta);
                }
            }
        }

        public string Ulica
        {
            get => _ulica;
            set
            {
                if (_ulica != value)
                {
                    _ulica = value;
                    OnPropertyChanged(() => Ulica);
                }
            }
        }

        public string NrDomu
        {
            get => _nrDomu;
            set
            {
                if (_nrDomu != value)
                {
                    _nrDomu = value;
                    OnPropertyChanged(() => NrDomu);
                }
            }
        }

        public string NrLokalu
        {
            get => _nrLokalu;
            set
            {
                if (_nrLokalu != value)
                {
                    _nrLokalu = value;
                    OnPropertyChanged(() => NrLokalu);
                }
            }
        }

        public string KodPocztowy
        {
            get => _kodPocztowy;
            set
            {
                if (_kodPocztowy != value)
                {
                    _kodPocztowy = value;
                    OnPropertyChanged(() => KodPocztowy);
                }
            }
        }

        public string Miasto
        {
            get => _miasto;
            set
            {
                if (_miasto != value)
                {
                    _miasto = value;
                    OnPropertyChanged(() => Miasto);
                }
            }
        }

        public string Kraj
        {
            get => _kraj;
            set
            {
                if (_kraj != value)
                {
                    _kraj = value;
                    OnPropertyChanged(() => Kraj);
                }
            }
        }

        public bool WasSaved { get; private set; }

        public override void Save()
        {
            if (!IsValid())
                return;

            var nowyKontrahent = new Kontrahent
            {
                NIP = NIP?.Trim(),
                NazwaPelna = NazwaPelna?.Trim(),
                NazwaSkrocona = NazwaSkrocona?.Trim(),
                Telefon = string.IsNullOrWhiteSpace(Telefon) ? null : Telefon.Trim(),
                Email = string.IsNullOrWhiteSpace(Email) ? null : Email.Trim(),
                CzyAktywny = CzyAktywny,
                IdTypuKontrahenta = SelectedTypKontrahenta?.IdTypuKontrahenta
            };

            var adres = new AdresKontrahenta
            {
                Ulica = Ulica?.Trim(),
                NrDomu = NrDomu?.Trim(),
                NrLokalu = string.IsNullOrWhiteSpace(NrLokalu) ? null : NrLokalu.Trim(),
                KodPocztowy = KodPocztowy?.Trim(),
                Miasto = Miasto?.Trim(),
                Kraj = Kraj?.Trim(),
                CzyDomyslny = true,
                Kontrahent = nowyKontrahent
            };

            nowyKontrahent.AdresKontrahenta.Add(adres);
            fakturyEntities.Kontrahent.Add(nowyKontrahent);
            fakturyEntities.SaveChanges();

            WasSaved = true;
            OnRequestClose();
        }

        public string Error => null;

        public string this[string name]
        {
            get
            {
                return name switch
                {
                    nameof(NIP) => StringValidator.RequiredAndLength(NIP, "NIP", 10, 10),
                    nameof(NazwaPelna) => StringValidator.RequiredAndLength(NazwaPelna, "Nazwa pełna", 2, 160),
                    nameof(NazwaSkrocona) => StringValidator.RequiredAndLength(NazwaSkrocona, "Nazwa skrócona", 2, 80),
                    nameof(SelectedTypKontrahenta) => Validator.Required(SelectedTypKontrahenta, "Typ kontrahenta"),
                    nameof(Ulica) => StringValidator.RequiredAndLength(Ulica, "Ulica", 2, 120),
                    nameof(NrDomu) => StringValidator.RequiredAndLength(NrDomu, "Nr domu", 1, 20),
                    nameof(KodPocztowy) => StringValidator.RequiredAndLength(KodPocztowy, "Kod pocztowy", 3, 12),
                    nameof(Miasto) => StringValidator.RequiredAndLength(Miasto, "Miasto", 2, 100),
                    nameof(Kraj) => StringValidator.RequiredAndLength(Kraj, "Kraj", 2, 80),
                    nameof(Email) => ValidateEmail(),
                    _ => null
                };
            }
        }

        private string ValidateEmail()
        {
            if (string.IsNullOrWhiteSpace(Email))
                return null;

            if (!Email.Contains("@"))
                return "Email ma niepoprawny format.";

            return null;
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
                nameof(NIP),
                nameof(NazwaPelna),
                nameof(NazwaSkrocona),
                nameof(SelectedTypKontrahenta),
                nameof(Ulica),
                nameof(NrDomu),
                nameof(KodPocztowy),
                nameof(Miasto),
                nameof(Kraj),
                nameof(Email)
            };
    }
}
