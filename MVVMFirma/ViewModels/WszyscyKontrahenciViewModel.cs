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
    public class WszyscyKontrahenciViewModel : WszystkieViewModel<Kontrahent>
    {
        private string _deleteId;

        public WszyscyKontrahenciViewModel()
        {
            DisplayName = "Kontrahenci";
            SetSortOptions(new[] { "Id", "NIP", "Nazwa pełna", "Nazwa skrócona" });
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

        protected override IEnumerable<Kontrahent> LoadData()
        {
            return fakturyEntities.Kontrahent
                .Include(k => k.TypKontrahenta)
                .ToList();
        }

        protected override bool MatchesFilter(Kontrahent item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdKontrahenta.ToString().Contains(text)
                   || (item.NIP != null && item.NIP.ToLowerInvariant().Contains(text))
                   || (item.NazwaPelna != null && item.NazwaPelna.ToLowerInvariant().Contains(text))
                   || (item.NazwaSkrocona != null && item.NazwaSkrocona.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<Kontrahent> ApplySort(IEnumerable<Kontrahent> query, string sortField, bool descending)
        {
            return sortField switch
            {
                "NIP" => descending ? query.OrderByDescending(k => k.NIP) : query.OrderBy(k => k.NIP),
                "Nazwa pełna" => descending ? query.OrderByDescending(k => k.NazwaPelna) : query.OrderBy(k => k.NazwaPelna),
                "Nazwa skrócona" => descending ? query.OrderByDescending(k => k.NazwaSkrocona) : query.OrderBy(k => k.NazwaSkrocona),
                _ => descending ? query.OrderByDescending(k => k.IdKontrahenta) : query.OrderBy(k => k.IdKontrahenta)
            };
        }

        private void OpenAddDialog()
        {
            var viewModel = new NowyKontrahentViewModel();
            var dialog = new NowyKontrahentDialog
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
                ShowMessageBox("Podaj id kontrahenta do usunięcia.");
                return;
            }

            if (!int.TryParse(DeleteId.Trim(), out var id))
            {
                ShowMessageBox("Id kontrahenta musi być liczbą.");
                return;
            }

            var kontrahent = fakturyEntities.Kontrahent
                .FirstOrDefault(k => k.IdKontrahenta == id);

            if (kontrahent == null)
            {
                ShowMessageBox("Nie znaleziono kontrahenta o podanym id.");
                return;
            }

            var adresy = fakturyEntities.AdresKontrahenta
                .Where(a => a.IdKontrahenta == id)
                .ToList();

            var faktury = fakturyEntities.FakturaSprzedazy
                .Where(f => f.IdKontrahenta == id)
                .ToList();

            if (faktury.Any())
            {
                var fakturyIds = faktury.Select(f => f.IdFakturySprzedazy).ToList();
                var pozycjeFaktur = fakturyEntities.PozycjaFakturySprzedazy
                    .Where(p => fakturyIds.Contains(p.IdFakturySprzedazy))
                    .ToList();

                if (pozycjeFaktur.Any())
                    fakturyEntities.PozycjaFakturySprzedazy.RemoveRange(pozycjeFaktur);

                fakturyEntities.FakturaSprzedazy.RemoveRange(faktury);
            }

            var zamowienia = fakturyEntities.ZamowienieSprzedazy
                .Where(z => z.IdKontrahenta == id)
                .ToList();

            if (zamowienia.Any())
            {
                var zamowieniaIds = zamowienia.Select(z => z.IdZamowieniaSprzedazy).ToList();
                var pozycjeZamowien = fakturyEntities.PozycjaZamowieniaSprzedazy
                    .Where(p => zamowieniaIds.Contains(p.IdZamowieniaSprzedazy))
                    .ToList();

                if (pozycjeZamowien.Any())
                    fakturyEntities.PozycjaZamowieniaSprzedazy.RemoveRange(pozycjeZamowien);

                fakturyEntities.ZamowienieSprzedazy.RemoveRange(zamowienia);
            }

            if (adresy.Any())
                fakturyEntities.AdresKontrahenta.RemoveRange(adresy);

            fakturyEntities.Kontrahent.Remove(kontrahent);
            fakturyEntities.SaveChanges();
            load();
        }
    }
}
