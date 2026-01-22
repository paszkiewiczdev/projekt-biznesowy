using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using MVVMFirma.Helper;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class NowyAdresKontrahentaViewModel : JedenViewModel<AdresKontrahenta>, IDataErrorInfo
    {
        public NowyAdresKontrahentaViewModel()
            : base()
        {
            DisplayName = "Nowy adres kontrahenta";
            Item = new AdresKontrahenta();

            Kontrahenci = new ObservableCollection<Kontrahent>(
                fakturyEntities.Kontrahent.ToList());

            Kraj = "Polska";
        }

        public ObservableCollection<Kontrahent> Kontrahenci { get; }

        private Kontrahent _selectedKontrahent;
        private string _ulica;
        private string _nrDomu;
        private string _nrLokalu;
        private string _kodPocztowy;
        private string _miasto;
        private string _kraj;
        private bool _czyDomyslny;

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

        public bool CzyDomyslny
        {
            get => _czyDomyslny;
            set
            {
                if (_czyDomyslny != value)
                {
                    _czyDomyslny = value;
                    OnPropertyChanged(() => CzyDomyslny);
                }
            }
        }

        public bool WasSaved { get; private set; }

        public override void Save()
        {
            if (!IsValid())
                return;

            var kontrahent = SelectedKontrahent;
            if (kontrahent == null)
                return;

            var existing = fakturyEntities.AdresKontrahenta
                .Where(a => a.IdKontrahenta == kontrahent.IdKontrahenta)
                .ToList();

            var shouldBeDefault = CzyDomyslny || !existing.Any();

            if (shouldBeDefault)
            {
                foreach (var adres in existing)
                    adres.CzyDomyslny = false;
            }

            var nowyAdres = new AdresKontrahenta
            {
                IdKontrahenta = kontrahent.IdKontrahenta,
                Ulica = Ulica?.Trim(),
                NrDomu = NrDomu?.Trim(),
                NrLokalu = string.IsNullOrWhiteSpace(NrLokalu) ? null : NrLokalu.Trim(),
                KodPocztowy = KodPocztowy?.Trim(),
                Miasto = Miasto?.Trim(),
                Kraj = Kraj?.Trim(),
                CzyDomyslny = shouldBeDefault
            };

            fakturyEntities.AdresKontrahenta.Add(nowyAdres);
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
                    nameof(SelectedKontrahent) => Validator.Required(SelectedKontrahent, "Kontrahent"),
                    nameof(Ulica) => StringValidator.RequiredAndLength(Ulica, "Ulica", 2, 120),
                    nameof(NrDomu) => StringValidator.RequiredAndLength(NrDomu, "Nr domu", 1, 20),
                    nameof(KodPocztowy) => StringValidator.RequiredAndLength(KodPocztowy, "Kod pocztowy", 3, 12),
                    nameof(Miasto) => StringValidator.RequiredAndLength(Miasto, "Miasto", 2, 100),
                    nameof(Kraj) => StringValidator.RequiredAndLength(Kraj, "Kraj", 2, 80),
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
                nameof(SelectedKontrahent),
                nameof(Ulica),
                nameof(NrDomu),
                nameof(KodPocztowy),
                nameof(Miasto),
                nameof(Kraj)
            };
    }
}