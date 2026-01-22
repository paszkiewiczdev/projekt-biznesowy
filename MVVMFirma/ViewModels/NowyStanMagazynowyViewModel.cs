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
    public class NowyStanMagazynowyViewModel : JedenViewModel<StanMagazynowy>, IDataErrorInfo
    {
        public NowyStanMagazynowyViewModel()
        {
            DisplayName = "Nowy stan magazynowy";

            Item = new StanMagazynowy();
            Magazyny = new ObservableCollection<Magazyn>(fakturyEntities.Magazyn.ToList());
            Towary = new ObservableCollection<Towar>(fakturyEntities.Towar.ToList());
        }

        public override void Save()
        {
            if (!IsValid())
                return;

            if (!decimal.TryParse(IloscText, NumberStyles.Number, CultureInfo.CurrentCulture, out var ilosc))
                return;

            if (SelectedMagazyn == null || SelectedTowar == null)
                return;

            var stan = fakturyEntities.StanMagazynowy.FirstOrDefault(s =>
                s.IdMagazynu == SelectedMagazyn.IdMagazynu
                && s.IdTowaru == SelectedTowar.IdTowaru);

            if (stan == null)
            {
                var nowy = new StanMagazynowy
                {
                    IdMagazynu = SelectedMagazyn.IdMagazynu,
                    IdTowaru = SelectedTowar.IdTowaru,
                    Ilosc = ilosc
                };

                fakturyEntities.StanMagazynowy.Add(nowy);
            }
            else
            {
                stan.Ilosc += ilosc;
            }

            fakturyEntities.SaveChanges();

            WasSaved = true;
            OnRequestClose();
        }

        private Magazyn _selectedMagazyn;
        private Towar _selectedTowar;
        private string _iloscText;

        public ObservableCollection<Magazyn> Magazyny { get; }

        public ObservableCollection<Towar> Towary { get; }

        public Magazyn SelectedMagazyn
        {
            get => _selectedMagazyn;
            set
            {
                if (_selectedMagazyn != value)
                {
                    _selectedMagazyn = value;
                    OnPropertyChanged(() => SelectedMagazyn);
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
                    nameof(SelectedMagazyn) => Validator.Required(SelectedMagazyn, "Magazyn"),
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
                nameof(SelectedMagazyn),
                nameof(SelectedTowar),
                nameof(IloscText)
            };
    }
}
