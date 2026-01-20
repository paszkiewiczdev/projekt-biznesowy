using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;
using MVVMFirma.Views;

namespace MVVMFirma.ViewModels
{
    public class WszystkieTowaryViewModel : WszystkieViewModel<Towar>
    {
        public WszystkieTowaryViewModel()
            : base()
        {
            DisplayName = "Towary";
            SetSortOptions(new[] { "Id", "Kod", "Nazwa", "Cena" });
            AddCommand = new BaseCommand(OpenAddDialog);
            RefreshCommand = new BaseCommand(load);
        }

        public ICommand AddCommand { get; }

        public ICommand RefreshCommand { get; }

        protected override IEnumerable<Towar> LoadData()
        {
            return fakturyEntities.Towar.ToList();
        }

        protected override bool MatchesFilter(Towar item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdTowaru.ToString().Contains(text)
                   || (item.Kod != null && item.Kod.ToLowerInvariant().Contains(text))
                   || (item.Nazwa != null && item.Nazwa.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<Towar> ApplySort(
            IEnumerable<Towar> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Kod" => descending
                    ? query.OrderByDescending(t => t.Kod)
                    : query.OrderBy(t => t.Kod),

                "Nazwa" => descending
                    ? query.OrderByDescending(t => t.Nazwa)
                    : query.OrderBy(t => t.Nazwa),

                "Cena" => descending
                    ? query.OrderByDescending(t => t.Cena)
                    : query.OrderBy(t => t.Cena),

                _ => descending
                    ? query.OrderByDescending(t => t.IdTowaru)
                    : query.OrderBy(t => t.IdTowaru)
            };
        }

        private void OpenAddDialog()
        {
            var viewModel = new NowyTowarViewModel();
            var dialog = new NowyTowarDialog
            {
                DataContext = viewModel,
                Owner = Application.Current?.MainWindow
            };

            if (dialog.ShowDialog() == true)
                load();
        }
    }
}
