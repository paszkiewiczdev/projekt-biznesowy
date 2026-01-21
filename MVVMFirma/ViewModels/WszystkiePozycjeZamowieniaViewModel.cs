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
    public class WszystkiePozycjeZamowieniaViewModel : WszystkieViewModel<PozycjaZamowieniaSprzedazy>
    {
        private string _deleteId;

        public WszystkiePozycjeZamowieniaViewModel()
        {
            DisplayName = "Pozycje zamówień sprzedaży";
            SetSortOptions(new[] { "Id", "Zamówienie", "Towar", "Ilość" });

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

        protected override IEnumerable<PozycjaZamowieniaSprzedazy> LoadData()
        {
            return fakturyEntities.PozycjaZamowieniaSprzedazy
                .Include(p => p.ZamowienieSprzedazy)
                .Include(p => p.ZamowienieSprzedazy.Kontrahent)
                .Include(p => p.Towar)
                .ToList();
        }

        protected override bool MatchesFilter(PozycjaZamowieniaSprzedazy item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdPozycjiZamowieniaSprzedazy.ToString().Contains(text)
                   || (item.ZamowienieSprzedazy != null
                       && item.ZamowienieSprzedazy.Numer != null
                       && item.ZamowienieSprzedazy.Numer.ToLowerInvariant().Contains(text))
                   || (item.Towar != null
                       && item.Towar.Nazwa != null
                       && item.Towar.Nazwa.ToLowerInvariant().Contains(text))
                   || (item.ZamowienieSprzedazy != null
                       && item.ZamowienieSprzedazy.Kontrahent != null
                       && item.ZamowienieSprzedazy.Kontrahent.NazwaPelna != null
                       && item.ZamowienieSprzedazy.Kontrahent.NazwaPelna.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<PozycjaZamowieniaSprzedazy> ApplySort(
            IEnumerable<PozycjaZamowieniaSprzedazy> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Zamówienie" => descending
                    ? query.OrderByDescending(p => p.ZamowienieSprzedazy != null ? p.ZamowienieSprzedazy.Numer : null)
                    : query.OrderBy(p => p.ZamowienieSprzedazy != null ? p.ZamowienieSprzedazy.Numer : null),

                "Towar" => descending
                    ? query.OrderByDescending(p => p.Towar != null ? p.Towar.Nazwa : null)
                    : query.OrderBy(p => p.Towar != null ? p.Towar.Nazwa : null),

                "Ilość" => descending
                    ? query.OrderByDescending(p => p.Ilosc)
                    : query.OrderBy(p => p.Ilosc),

                _ => descending
                    ? query.OrderByDescending(p => p.IdPozycjiZamowieniaSprzedazy)
                    : query.OrderBy(p => p.IdPozycjiZamowieniaSprzedazy)
            };
        }

        private void OpenAddDialog()
        {
            var viewModel = new NowaPozycjaZamowieniaSprzedazyViewModel();

            var dialog = new NowaPozycjaZamowieniaSprzedazyDialog
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
                MessageBox.Show("Podaj id pozycji do usunięcia.");
                return;
            }

            if (!int.TryParse(DeleteId.Trim(), out var id))
            {
                MessageBox.Show("Id pozycji musi być liczbą.");
                return;
            }

            var pozycja = fakturyEntities.PozycjaZamowieniaSprzedazy
                .FirstOrDefault(p => p.IdPozycjiZamowieniaSprzedazy == id);

            if (pozycja == null)
            {
                MessageBox.Show("Nie znaleziono pozycji o podanym id.");
                return;
            }

            fakturyEntities.PozycjaZamowieniaSprzedazy.Remove(pozycja);
            fakturyEntities.SaveChanges();

            load();
        }
    }
}
