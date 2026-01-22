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
    public class NowaPozycjaDokumentuMagazynowegoViewModel
        : JedenViewModel<PozycjaDokumentuMagazynowego>, IDataErrorInfo
    {
        public NowaPozycjaDokumentuMagazynowegoViewModel()
        {
            DisplayName = "Nowa pozycja dokumentu magazynowego";

            Item = new PozycjaDokumentuMagazynowego();
            Dokumenty = new ObservableCollection<DokumentMagazynowy>(
                fakturyEntities.DokumentMagazynowy.ToList());
            Towary = new ObservableCollection<Towar>(fakturyEntities.Towar.ToList());
        }

        public override void Save()
        {
            if (!IsValid())
                return;

            if (!decimal.TryParse(IloscText, NumberStyles.Number, CultureInfo.CurrentCulture, out var ilosc))
                return;

            if (SelectedDokument == null || SelectedTowar == null)
                return;

            var pozycja = new PozycjaDokumentuMagazynowego
            {
                IdDokumentuMagazynowego = SelectedDokument.IdDokumentuMagazynowego,
                IdTowaru = SelectedTowar.IdTowaru,
                Ilosc = ilosc
            };

            fakturyEntities.PozycjaDokumentuMagazynowego.Add(pozycja);
            fakturyEntities.SaveChanges();

            WasSaved = true;
            OnRequestClose();
        }

        private DokumentMagazynowy _selectedDokument;
        private Towar _selectedTowar;
        private string _iloscText;

        public ObservableCollection<DokumentMagazynowy> Dokumenty { get; }

        public ObservableCollection<Towar> Towary { get; }

        public DokumentMagazynowy SelectedDokument
        {
            get => _selectedDokument;
            set
            {
                if (_selectedDokument != value)
                {
                    _selectedDokument = value;
                    OnPropertyChanged(() => SelectedDokument);
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

        public bool WasSaved { get; private set; }

        public string Error => null;

        public string this[string name]
        {
            get
            {
                return name switch
                {
                    nameof(SelectedDokument) => Validator.Required(SelectedDokument, "Dokument magazynowy"),
                    nameof(SelectedTowar) => Validator.Required(SelectedTowar, "Towar"),
                    nameof(IloscText) => Validator.PositiveDecimal(IloscText, "Ilość"),
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
                nameof(SelectedDokument),
                nameof(SelectedTowar),
                nameof(IloscText)
            };
    }
}