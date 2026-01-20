using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;
using MVVMFirma.Views;

namespace MVVMFirma.ViewModels
{
    public class WszystkiePozycjeFakturySprzedazyViewModel : WszystkieViewModel<PozycjaFakturySprzedazy>
    {
        private string _deleteNumber;

        public WszystkiePozycjeFakturySprzedazyViewModel()
        {
            DisplayName = "Pozycje faktur sprzedaży";
            SetSortOptions(new[] { "Id", "Faktura", "Towar", "Ilość" });
            AddCommand = new BaseCommand(OpenAddDialog);
            RefreshCommand = new BaseCommand(load);
            DeleteByNumberCommand = new BaseCommand(DeleteByNumber);
        }

        public ICommand AddCommand { get; }

        public ICommand RefreshCommand { get; }

        public ICommand DeleteByNumberCommand { get; }

        public string DeleteNumber
        {
            get => _deleteNumber;
            set
            {
                _deleteNumber = value;
                OnPropertyChanged(() => DeleteNumber);
            }
        }

        protected override IEnumerable<PozycjaFakturySprzedazy> LoadData()
        {
            return fakturyEntities.PozycjaFakturySprzedazy
                .Include(p => p.FakturaSprzedazy)
                .Include(p => p.FakturaSprzedazy.Kontrahent)
                .Include(p => p.Towar)
                .Include(p => p.StawkaVat)
                .ToList();
        }

        protected override bool MatchesFilter(PozycjaFakturySprzedazy item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdPozycjiFakturySprzedazy.ToString().Contains(text)
                   || (item.FakturaSprzedazy != null
                       && item.FakturaSprzedazy.Numer != null
                       && item.FakturaSprzedazy.Numer.ToLowerInvariant().Contains(text))
                   || (item.Towar != null
                       && item.Towar.Nazwa != null
                       && item.Towar.Nazwa.ToLowerInvariant().Contains(text))
                   || (item.FakturaSprzedazy != null
                       && item.FakturaSprzedazy.Kontrahent != null
                       && item.FakturaSprzedazy.Kontrahent.NazwaPelna != null
                       && item.FakturaSprzedazy.Kontrahent.NazwaPelna.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<PozycjaFakturySprzedazy> ApplySort(
            IEnumerable<PozycjaFakturySprzedazy> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Faktura" => descending
                    ? query.OrderByDescending(p => p.FakturaSprzedazy != null ? p.FakturaSprzedazy.Numer : null)
                    : query.OrderBy(p => p.FakturaSprzedazy != null ? p.FakturaSprzedazy.Numer : null),

                "Towar" => descending
                    ? query.OrderByDescending(p => p.Towar != null ? p.Towar.Nazwa : null)
                    : query.OrderBy(p => p.Towar != null ? p.Towar.Nazwa : null),

                "Ilość" => descending
                    ? query.OrderByDescending(p => p.Ilosc)
                    : query.OrderBy(p => p.Ilosc),

                _ => descending
                    ? query.OrderByDescending(p => p.IdPozycjiFakturySprzedazy)
                    : query.OrderBy(p => p.IdPozycjiFakturySprzedazy)
            };
        }

        private void OpenAddDialog()
        {
            var viewModel = new NowaPozycjaFakturySprzedazyViewModel();
            var dialog = new NowaPozycjaFakturySprzedazyDialog
            {
                DataContext = viewModel,
                Owner = Application.Current?.MainWindow
            };

            if (dialog.ShowDialog() == true)
                load();
        }

        private void DeleteByNumber()
        {
            if (string.IsNullOrWhiteSpace(DeleteNumber))
            {
                ShowMessageBox("Podaj numer pozycji do usunięcia.");
                return;
            }

            if (!int.TryParse(DeleteNumber.Trim(), out var id))
            {
                ShowMessageBox("Numer pozycji musi być liczbą.");
                return;
            }

            var pozycja = fakturyEntities.PozycjaFakturySprzedazy
                .FirstOrDefault(item => item.IdPozycjiFakturySprzedazy == id);

            if (pozycja == null)
            {
                ShowMessageBox("Nie znaleziono pozycji o podanym numerze.");
                return;
            }

            fakturyEntities.PozycjaFakturySprzedazy.Remove(pozycja);
            fakturyEntities.SaveChanges();
            load();
        }
    }
}
