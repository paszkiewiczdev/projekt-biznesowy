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
    public class WszystkieZamowieniaViewModel : WszystkieViewModel<ZamowienieSprzedazy>
    {
        private string _deleteId;

        public WszystkieZamowieniaViewModel()
        {
            DisplayName = "Zamówienia sprzedaży";
            SetSortOptions(new[] { "Id", "Numer", "Data" });

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

        protected override IEnumerable<ZamowienieSprzedazy> LoadData()
        {
            return fakturyEntities.ZamowienieSprzedazy
                .Include(z => z.Kontrahent)
                .Include(z => z.StatusZamowienia)
                .ToList();
        }

        protected override bool MatchesFilter(ZamowienieSprzedazy item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdZamowieniaSprzedazy.ToString().Contains(text)
                   || (item.Numer != null && item.Numer.ToLowerInvariant().Contains(text))
                   || (item.Kontrahent != null
                       && item.Kontrahent.NazwaPelna != null
                       && item.Kontrahent.NazwaPelna.ToLowerInvariant().Contains(text))
                   || (item.StatusZamowienia != null
                       && item.StatusZamowienia.Nazwa != null
                       && item.StatusZamowienia.Nazwa.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<ZamowienieSprzedazy> ApplySort(
            IEnumerable<ZamowienieSprzedazy> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Numer" => descending
                    ? query.OrderByDescending(z => z.Numer)
                    : query.OrderBy(z => z.Numer),

                "Data" => descending
                    ? query.OrderByDescending(z => z.DataZamowienia)
                    : query.OrderBy(z => z.DataZamowienia),

                _ => descending
                    ? query.OrderByDescending(z => z.IdZamowieniaSprzedazy)
                    : query.OrderBy(z => z.IdZamowieniaSprzedazy)
            };
        }

        private void OpenAddDialog()
        {
            var viewModel = new NoweZamowienieSprzedazyViewModel();

            var dialog = new NoweZamowienieSprzedazyDialog
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
                MessageBox.Show("Podaj id zamówienia do usunięcia.");
                return;
            }

            if (!int.TryParse(DeleteId.Trim(), out var id))
            {
                MessageBox.Show("Id zamówienia musi być liczbą.");
                return;
            }

            var zamowienie = fakturyEntities.ZamowienieSprzedazy
                .FirstOrDefault(z => z.IdZamowieniaSprzedazy == id);

            if (zamowienie == null)
            {
                MessageBox.Show("Nie znaleziono zamówienia o podanym id.");
                return;
            }

            var pozycje = fakturyEntities.PozycjaZamowieniaSprzedazy
                .Where(p => p.IdZamowieniaSprzedazy == zamowienie.IdZamowieniaSprzedazy)
                .ToList();

            if (pozycje.Any())
                fakturyEntities.PozycjaZamowieniaSprzedazy.RemoveRange(pozycje);

            fakturyEntities.ZamowienieSprzedazy.Remove(zamowienie);
            fakturyEntities.SaveChanges();

            load();
        }
    }
}
