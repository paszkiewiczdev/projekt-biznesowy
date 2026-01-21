using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class NowyDokumentMagazynowyViewModel : JedenViewModel<DokumentMagazynowy>, IDataErrorInfo
    {
        public NowyDokumentMagazynowyViewModel()
        {
            DisplayName = "Nowy dokument magazynowy";

            Item = new DokumentMagazynowy();
            Magazyny = new ObservableCollection<Magazyn>(fakturyEntities.Magazyn.ToList());
            TypyDokumentow = new ObservableCollection<TypDokumentuMagazynowego>(
                fakturyEntities.TypDokumentuMagazynowego.ToList());
            Towary = new ObservableCollection<Towar>(fakturyEntities.Towar.ToList());

            Pozycje = new ObservableCollection<PozycjaDokumentuMagazynowegoDraft>();

            AddPozycjaCommand = new BaseCommand(AddPozycja);
            RemovePozycjaCommand = new BaseCommand(RemovePozycja);
        }

        public override void Save()
        {
            if (!IsValid())
                return;

            if (Pozycje.Count == 0)
            {
                ShowMessageBox("Dodaj przynajmniej jedną pozycję dokumentu.");
                return;
            }

            if (!DataDokumentu.HasValue)
                return;

            var dokument = new DokumentMagazynowy
            {
                Numer = Numer?.Trim(),
                DataDokumentu = DataDokumentu.Value,
                IdMagazynu = SelectedMagazyn.IdMagazynu,
                IdTypuDokumentu = SelectedTypDokumentu.IdTypuDokumentu,
                Uwagi = string.IsNullOrWhiteSpace(Uwagi) ? null : Uwagi.Trim()
            };

            foreach (var pozycja in Pozycje)
            {
                var nowaPozycja = new PozycjaDokumentuMagazynowego
                {
                    IdTowaru = pozycja.Towar.IdTowaru,
                    Ilosc = pozycja.Ilosc
                };

                dokument.PozycjaDokumentuMagazynowego.Add(nowaPozycja);
            }

            fakturyEntities.DokumentMagazynowy.Add(dokument);
            fakturyEntities.SaveChanges();

            WasSaved = true;
            OnRequestClose();
        }

        private string _numer;
        private DateTime? _dataDokumentu;
        private string _uwagi;
        private Magazyn _selectedMagazyn;
        private TypDokumentuMagazynowego _selectedTypDokumentu;
        private Towar _selectedTowar;
        private string _iloscText;
        private PozycjaDokumentuMagazynowegoDraft _selectedPozycja;

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

        public DateTime? DataDokumentu
        {
            get => _dataDokumentu;
            set
            {
                if (_dataDokumentu != value)
                {
                    _dataDokumentu = value;
                    OnPropertyChanged(() => DataDokumentu);
                }
            }
        }

        public string Uwagi
        {
            get => _uwagi;
            set
            {
                if (_uwagi != value)
                {
                    _uwagi = value;
                    OnPropertyChanged(() => Uwagi);
                }
            }
        }

        public ObservableCollection<Magazyn> Magazyny { get; }

        public ObservableCollection<TypDokumentuMagazynowego> TypyDokumentow { get; }

        public ObservableCollection<Towar> Towary { get; }

        public ObservableCollection<PozycjaDokumentuMagazynowegoDraft> Pozycje { get; }

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

        public TypDokumentuMagazynowego SelectedTypDokumentu
        {
            get => _selectedTypDokumentu;
            set
            {
                if (_selectedTypDokumentu != value)
                {
                    _selectedTypDokumentu = value;
                    OnPropertyChanged(() => SelectedTypDokumentu);
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

        public PozycjaDokumentuMagazynowegoDraft SelectedPozycja
        {
            get => _selectedPozycja;
            set
            {
                if (_selectedPozycja != value)
                {
                    _selectedPozycja = value;
                    OnPropertyChanged(() => SelectedPozycja);
                }
            }
        }

        public ICommand AddPozycjaCommand { get; }

        public ICommand RemovePozycjaCommand { get; }

        public bool WasSaved { get; private set; }

        public string Error => null;

        public string this[string name]
        {
            get
            {
                return name switch
                {
                    nameof(Numer) => StringValidator.RequiredAndLength(Numer, "Numer", 2, 50),
                    nameof(DataDokumentu) => Validator.Required(DataDokumentu, "Data dokumentu"),
                    nameof(SelectedMagazyn) => Validator.Required(SelectedMagazyn, "Magazyn"),
                    nameof(SelectedTypDokumentu) => Validator.Required(SelectedTypDokumentu, "Typ dokumentu"),
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
                nameof(Numer),
                nameof(DataDokumentu),
                nameof(SelectedMagazyn),
                nameof(SelectedTypDokumentu)
            };

        private void AddPozycja()
        {
            var towarError = this[nameof(SelectedTowar)];
            if (!string.IsNullOrEmpty(towarError))
            {
                ShowMessageBox(towarError);
                return;
            }

            var iloscError = this[nameof(IloscText)];
            if (!string.IsNullOrEmpty(iloscError))
            {
                ShowMessageBox(iloscError);
                return;
            }

            if (!decimal.TryParse(IloscText, NumberStyles.Number, CultureInfo.CurrentCulture, out var ilosc))
                return;

            Pozycje.Add(new PozycjaDokumentuMagazynowegoDraft(SelectedTowar, ilosc));

            SelectedTowar = null;
            IloscText = string.Empty;
        }

        private void RemovePozycja()
        {
            if (SelectedPozycja == null)
            {
                ShowMessageBox("Wybierz pozycję do usunięcia.");
                return;
            }

            Pozycje.Remove(SelectedPozycja);
            SelectedPozycja = null;
        }
    }

    public class PozycjaDokumentuMagazynowegoDraft
    {
        public PozycjaDokumentuMagazynowegoDraft(Towar towar, decimal ilosc)
        {
            Towar = towar;
            Ilosc = ilosc;
        }

        public Towar Towar { get; }

        public decimal Ilosc { get; }
    }
}