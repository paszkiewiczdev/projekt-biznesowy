using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using MVVMFirma.Helper;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class NoweZamowienieSprzedazyViewModel : JedenViewModel<ZamowienieSprzedazy>, IDataErrorInfo
    {
        public NoweZamowienieSprzedazyViewModel()
        {
            DisplayName = "Nowe zamówienie sprzedaży";

            Item = new ZamowienieSprzedazy();
            Kontrahenci = new ObservableCollection<Kontrahent>(fakturyEntities.Kontrahent.ToList());
            StatusyZamowien = new ObservableCollection<StatusZamowienia>(fakturyEntities.StatusZamowienia.ToList());
        }

        public override void Save()
        {
            if (!IsValid())
                return;

            if (!DataZamowienia.HasValue || SelectedKontrahent == null || SelectedStatusZamowienia == null)
                return;

            var zamowienie = new ZamowienieSprzedazy
            {
                Numer = Numer?.Trim(),
                DataZamowienia = DataZamowienia.Value,
                IdKontrahenta = SelectedKontrahent.IdKontrahenta,
                IdStatusuZamowienia = SelectedStatusZamowienia.IdStatusuZamowienia,
                Uwagi = string.IsNullOrWhiteSpace(Uwagi) ? null : Uwagi.Trim()
            };

            fakturyEntities.ZamowienieSprzedazy.Add(zamowienie);
            fakturyEntities.SaveChanges();

            WasSaved = true;
            OnRequestClose();
        }

        private string _numer;
        private DateTime? _dataZamowienia;
        private Kontrahent _selectedKontrahent;
        private StatusZamowienia _selectedStatusZamowienia;
        private string _uwagi;

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

        public DateTime? DataZamowienia
        {
            get => _dataZamowienia;
            set
            {
                if (_dataZamowienia != value)
                {
                    _dataZamowienia = value;
                    OnPropertyChanged(() => DataZamowienia);
                }
            }
        }

        public ObservableCollection<Kontrahent> Kontrahenci { get; }

        public ObservableCollection<StatusZamowienia> StatusyZamowien { get; }

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

        public StatusZamowienia SelectedStatusZamowienia
        {
            get => _selectedStatusZamowienia;
            set
            {
                if (_selectedStatusZamowienia != value)
                {
                    _selectedStatusZamowienia = value;
                    OnPropertyChanged(() => SelectedStatusZamowienia);
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

        public bool WasSaved { get; private set; }

        public string Error => null;

        public string this[string name]
        {
            get
            {
                return name switch
                {
                    nameof(Numer) => StringValidator.RequiredAndLength(Numer, "Numer", 2, 50),
                    nameof(DataZamowienia) => Validator.Required(DataZamowienia, "Data zamówienia"),
                    nameof(SelectedKontrahent) => Validator.Required(SelectedKontrahent, "Kontrahent"),
                    nameof(SelectedStatusZamowienia) => Validator.Required(SelectedStatusZamowienia, "Status"),
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
                nameof(DataZamowienia),
                nameof(SelectedKontrahent),
                nameof(SelectedStatusZamowienia)
            };
    }
}