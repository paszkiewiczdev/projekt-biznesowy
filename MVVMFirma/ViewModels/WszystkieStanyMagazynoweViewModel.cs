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
    public class WszystkieStanyMagazynoweViewModel : WszystkieViewModel<StanMagazynowy>
    {
        private string _deleteId;

        public WszystkieStanyMagazynoweViewModel()
        {
            DisplayName = "Stany magazynowe";
            SetSortOptions(new[] { "Id", "Magazyn", "Towar", "Ilość" });

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

        protected override IEnumerable<StanMagazynowy> LoadData()
        {
            return fakturyEntities.StanMagazynowy
                .Include(s => s.Magazyn)
                .Include(s => s.Towar)
                .ToList();
        }

        protected override bool MatchesFilter(StanMagazynowy item, string filterText)
        {
            var text = (filterText ?? string.Empty).ToLowerInvariant();

            return item.IdStanuMagazynowego.ToString().Contains(text)
                   || (item.Magazyn?.Nazwa?.ToLowerInvariant().Contains(text) ?? false)
                   || (item.Towar?.Nazwa?.ToLowerInvariant().Contains(text) ?? false);
        }

        protected override IOrderedEnumerable<StanMagazynowy> ApplySort(
            IEnumerable<StanMagazynowy> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Magazyn" => descending
                    ? query.OrderByDescending(s => s.Magazyn?.Nazwa)
                    : query.OrderBy(s => s.Magazyn?.Nazwa),

                "Towar" => descending
                    ? query.OrderByDescending(s => s.Towar?.Nazwa)
                    : query.OrderBy(s => s.Towar?.Nazwa),

                "Ilość" => descending
                    ? query.OrderByDescending(s => s.Ilosc)
                    : query.OrderBy(s => s.Ilosc),

                _ => descending
                    ? query.OrderByDescending(s => s.IdStanuMagazynowego)
                    : query.OrderBy(s => s.IdStanuMagazynowego)
            };
        }

        private void OpenAddDialog()
        {
            var viewModel = new NowyStanMagazynowyViewModel();
            var dialog = new NowyStanMagazynowyDialog
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
                MessageBox.Show("Podaj ID stanu magazynowego do usunięcia.");
                return;
            }

            if (!int.TryParse(DeleteId.Trim(), out var id))
            {
                MessageBox.Show("ID stanu magazynowego musi być liczbą.");
                return;
            }

            var stan = fakturyEntities.StanMagazynowy
                .FirstOrDefault(item => item.IdStanuMagazynowego == id);

            if (stan == null)
            {
                MessageBox.Show("Nie znaleziono stanu magazynowego o podanym ID.");
                return;
            }

            fakturyEntities.StanMagazynowy.Remove(stan);
            fakturyEntities.SaveChanges();
            load();
        }
    }
}
