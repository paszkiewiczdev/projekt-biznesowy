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
    public class NowyTowarViewModel : JedenViewModel<Towar>, IDataErrorInfo
    {
        #region Constructor

        public NowyTowarViewModel()
            : base()
        {
            base.DisplayName = "Nowy towar";

            Item = new Towar();
            KategorieTowaru = new ObservableCollection<KategoriaTowaru>(fakturyEntities.KategoriaTowaru.ToList());
            JednostkiMiary = new ObservableCollection<JednostkaMiary>(fakturyEntities.JednostkaMiary.ToList());
            StawkiVat = new ObservableCollection<StawkaVat>(fakturyEntities.StawkaVat.ToList());
        }

        #endregion

        #region Helpers

        public override void Save()
        {
            if (!IsValid())
                return;

            if (!decimal.TryParse(CenaText, NumberStyles.Number, CultureInfo.CurrentCulture, out var cena))
                return;

            if (!decimal.TryParse(MarzaText, NumberStyles.Number, CultureInfo.CurrentCulture, out var marza))
                return;

            var nowy = new Towar
            {
                Kod = Kod?.Trim(),
                Nazwa = Nazwa?.Trim(),
                Cena = cena,
                Marza = marza,
                CzyAktywny = CzyAktywny,
                IdKategoriiTowaru = SelectedKategoriaTowaru?.IdKategoriiTowaru,
                IdJednostkiMiary = SelectedJednostkaMiary?.IdJednostkiMiary,
                IdStawkiVatSprzedazy = SelectedStawkaVatSprzedazy?.IdStawkiVat,
                IdStawkiVatZakupu = SelectedStawkaVatZakupu?.IdStawkiVat
            };

            fakturyEntities.Towar.Add(nowy);
            fakturyEntities.SaveChanges();

            WasSaved = true;
            OnRequestClose();
        }

        #endregion

        #region Properties

        private string _kod;
        private string _nazwa;
        private string _cenaText;
        private string _marzaText;
        private bool _czyAktywny = true;
        private KategoriaTowaru _selectedKategoriaTowaru;
        private JednostkaMiary _selectedJednostkaMiary;
        private StawkaVat _selectedStawkaVatSprzedazy;
        private StawkaVat _selectedStawkaVatZakupu;

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

        public string CenaText
        {
            get => _cenaText;
            set
            {
                if (_cenaText != value)
                {
                    _cenaText = value;
                    OnPropertyChanged(() => CenaText);
                }
            }
        }

        public string MarzaText
        {
            get => _marzaText;
            set
            {
                if (_marzaText != value)
                {
                    _marzaText = value;
                    OnPropertyChanged(() => MarzaText);
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

        public ObservableCollection<KategoriaTowaru> KategorieTowaru { get; }

        public ObservableCollection<JednostkaMiary> JednostkiMiary { get; }

        public ObservableCollection<StawkaVat> StawkiVat { get; }

        public KategoriaTowaru SelectedKategoriaTowaru
        {
            get => _selectedKategoriaTowaru;
            set
            {
                if (_selectedKategoriaTowaru != value)
                {
                    _selectedKategoriaTowaru = value;
                    OnPropertyChanged(() => SelectedKategoriaTowaru);
                }
            }
        }

        public JednostkaMiary SelectedJednostkaMiary
        {
            get => _selectedJednostkaMiary;
            set
            {
                if (_selectedJednostkaMiary != value)
                {
                    _selectedJednostkaMiary = value;
                    OnPropertyChanged(() => SelectedJednostkaMiary);
                }
            }
        }

        public StawkaVat SelectedStawkaVatSprzedazy
        {
            get => _selectedStawkaVatSprzedazy;
            set
            {
                if (_selectedStawkaVatSprzedazy != value)
                {
                    _selectedStawkaVatSprzedazy = value;
                    OnPropertyChanged(() => SelectedStawkaVatSprzedazy);
                }
            }
        }

        public StawkaVat SelectedStawkaVatZakupu
        {
            get => _selectedStawkaVatZakupu;
            set
            {
                if (_selectedStawkaVatZakupu != value)
                {
                    _selectedStawkaVatZakupu = value;
                    OnPropertyChanged(() => SelectedStawkaVatZakupu);
                }
            }
        }

        public bool WasSaved { get; private set; }

        #endregion

        #region Validation

        public string Error => null;

        public string this[string name]
        {
            get
            {
                return name switch
                {
                    nameof(Kod) => StringValidator.RequiredAndLength(Kod, "Kody", 2, 20),
                    nameof(Nazwa) => StringValidator.RequiredAndLength(Nazwa, "Nazwa", 2, 120),
                    nameof(CenaText) => Validator.PositiveDecimal(CenaText, "Cena"),
                    nameof(MarzaText) => BiznesValidator.Percent(MarzaText, "Marża"),
                    nameof(SelectedKategoriaTowaru) => Validator.Required(SelectedKategoriaTowaru, "Kategoria"),
                    nameof(SelectedJednostkaMiary) => Validator.Required(SelectedJednostkaMiary, "Jednostka miary"),
                    nameof(SelectedStawkaVatSprzedazy) => Validator.Required(SelectedStawkaVatSprzedazy, "VAT sprzedaży"),
                    nameof(SelectedStawkaVatZakupu) => Validator.Required(SelectedStawkaVatZakupu, "VAT zakupu"),
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
                nameof(Kod),
                nameof(Nazwa),
                nameof(CenaText),
                nameof(MarzaText),
                nameof(SelectedKategoriaTowaru),
                nameof(SelectedJednostkaMiary),
                nameof(SelectedStawkaVatSprzedazy),
                nameof(SelectedStawkaVatZakupu)
            };

        #endregion
    }
}
