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
    public class WszystkieFakturyViewModel : WszystkieViewModel<FakturaSprzedazy>
    {
        private string _DeleteNumber;

        public WszystkieFakturyViewModel()
        {
            DisplayName = "Faktury";
            SetSortOptions(new[] { "Id", "Numer", "Data wystawienia" });
            AddCommand = new BaseCommand(OpenAddDialog);
            RefreshCommand = new BaseCommand(load);
            DeleteByNumberCommand = new BaseCommand(DeleteByNumber);
        }

        public ICommand AddCommand { get; }

        public ICommand RefreshCommand { get; }

        public ICommand DeleteByNumberCommand { get; }

        public string DeleteNumber
        {
            get => _DeleteNumber;
            set
            {
                _DeleteNumber = value;
                OnPropertyChanged(() => DeleteNumber);
            }
        }

        protected override IEnumerable<FakturaSprzedazy> LoadData()
        {
            return fakturyEntities.FakturaSprzedazy.ToList();
        }
        public override void load()
        {
            if (fakturyEntities != null)
                fakturyEntities.Dispose();

            fakturyEntities = new FakturyEntities();
            base.load();
        }

        protected override bool MatchesFilter(FakturaSprzedazy item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdFakturySprzedazy.ToString().Contains(text)
                   || (item.Numer != null && item.Numer.ToLowerInvariant().Contains(text))
                   || (item.Kontrahent != null
                       && item.Kontrahent.NazwaPelna != null
                       && item.Kontrahent.NazwaPelna.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<FakturaSprzedazy> ApplySort(
            IEnumerable<FakturaSprzedazy> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Numer" => descending
                    ? query.OrderByDescending(f => f.Numer)
                    : query.OrderBy(f => f.Numer),

                "Data wystawienia" => descending
                    ? query.OrderByDescending(f => f.DataWystawienia)
                    : query.OrderBy(f => f.DataWystawienia),

                _ => descending
                    ? query.OrderByDescending(f => f.IdFakturySprzedazy)
                    : query.OrderBy(f => f.IdFakturySprzedazy)
            };
        }

        private void OpenAddDialog()
        {
            var viewModel = new NowaFakturaViewModel();
            var dialog = new NowaFakturaDialog
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
                ShowMessageBox("Podaj numer faktury do usunięcia.");
                return;
            }

            var number = DeleteNumber.Trim().ToLower();
            var faktura = fakturyEntities.FakturaSprzedazy.FirstOrDefault(item =>
                item.Numer != null
                && item.Numer.Trim().ToLower() == number);

            if (faktura == null)
            {
                ShowMessageBox("Nie znaleziono faktury o podanym numerze.");
                return;
            }

            var pozycjeDoUsuniecia = fakturyEntities.PozycjaFakturySprzedazy
                .Where(p => p.IdFakturySprzedazy == faktura.IdFakturySprzedazy)
                .ToList();

            if (pozycjeDoUsuniecia.Any())
                fakturyEntities.PozycjaFakturySprzedazy.RemoveRange(pozycjeDoUsuniecia);

            fakturyEntities.FakturaSprzedazy.Remove(faktura);
            fakturyEntities.SaveChanges();
            load();
        }
    }
}
