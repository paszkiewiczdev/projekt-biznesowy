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
    public class WszystkieAdresyKontrahentowViewModel : WszystkieViewModel<AdresKontrahenta>
    {
        private string _deleteId;

        public WszystkieAdresyKontrahentowViewModel()
        {
            DisplayName = "Adresy kontrahentów";
            SetSortOptions(new[] { "Id", "Kontrahent", "Miasto" });
            AddCommand = new BaseCommand(OpenAddDialog);
            RefreshCommand = new BaseCommand(load);
            DeleteByIdCommand = new BaseCommand(DeleteById);
        }

        public ICommand AddCommand { get; }

        public ICommand RefreshCommand { get; }

        public ICommand DeleteByIdCommand { get; }

        public string DeleteId
        {
            get => _deleteId;
            set
            {
                _deleteId = value;
                OnPropertyChanged(() => DeleteId);
            }
        }

        protected override IEnumerable<AdresKontrahenta> LoadData()
        {
            return fakturyEntities.AdresKontrahenta
                .Include(a => a.Kontrahent)
                .ToList();
        }

        protected override bool MatchesFilter(AdresKontrahenta item, string filterText)
        {
            var text = (filterText ?? string.Empty).ToLowerInvariant();

            return item.IdAdresu.ToString().Contains(text)
                   || (item.Kontrahent?.NazwaPelna?.ToLowerInvariant().Contains(text) ?? false)
                   || (item.Miasto?.ToLowerInvariant().Contains(text) ?? false)
                   || (item.Ulica?.ToLowerInvariant().Contains(text) ?? false);
        }

        protected override IOrderedEnumerable<AdresKontrahenta> ApplySort(IEnumerable<AdresKontrahenta> query, string sortField, bool descending)
        {
            if (sortField == "Kontrahent")
            {
                return descending
                    ? query.OrderByDescending(a => a.Kontrahent?.NazwaPelna)
                    : query.OrderBy(a => a.Kontrahent?.NazwaPelna);
            }

            if (sortField == "Miasto")
            {
                return descending
                    ? query.OrderByDescending(a => a.Miasto)
                    : query.OrderBy(a => a.Miasto);
            }

            return descending
                ? query.OrderByDescending(a => a.IdAdresu)
                : query.OrderBy(a => a.IdAdresu);
        }

        private void OpenAddDialog()
        {
            var viewModel = new NowyAdresKontrahentaViewModel();
            var dialog = new NowyAdresKontrahentaDialog
            {
                DataContext = viewModel,
                Owner = Application.Current?.MainWindow
            };

            if (dialog.ShowDialog() == true)
                load();
        }

        private void DeleteById()
        {
            if (string.IsNullOrWhiteSpace(DeleteId))
            {
                ShowMessageBox("Podaj id adresu do usunięcia.");
                return;
            }

            if (!int.TryParse(DeleteId.Trim(), out var id))
            {
                ShowMessageBox("Id adresu musi być liczbą.");
                return;
            }

            var adres = fakturyEntities.AdresKontrahenta
                .FirstOrDefault(a => a.IdAdresu == id);

            if (adres == null)
            {
                ShowMessageBox("Nie znaleziono adresu o podanym id.");
                return;
            }

            var kontrahentId = adres.IdKontrahenta;
            var wasDefault = adres.CzyDomyslny;

            fakturyEntities.AdresKontrahenta.Remove(adres);

            if (wasDefault)
            {
                var nextDefault = fakturyEntities.AdresKontrahenta
                    .FirstOrDefault(a => a.IdKontrahenta == kontrahentId && a.IdAdresu != id);

                if (nextDefault != null)
                    nextDefault.CzyDomyslny = true;
            }

            fakturyEntities.SaveChanges();
            load();
        }
    }
}
