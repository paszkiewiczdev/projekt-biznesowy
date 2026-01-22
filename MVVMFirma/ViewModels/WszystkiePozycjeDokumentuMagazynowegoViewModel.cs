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
    public class WszystkiePozycjeDokumentuMagazynowegoViewModel
        : WszystkieViewModel<PozycjaDokumentuMagazynowego>
    {
        private string _deleteNumber;

        public WszystkiePozycjeDokumentuMagazynowegoViewModel()
        {
            DisplayName = "Pozycje dokumentów magazynowych";
            SetSortOptions(new[] { "Id", "Dokument", "Towar", "Ilość" });

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

        protected override IEnumerable<PozycjaDokumentuMagazynowego> LoadData()
        {
            return fakturyEntities.PozycjaDokumentuMagazynowego
                .Include(p => p.DokumentMagazynowy)
                .Include(p => p.Towar)
                .ToList();
        }

        protected override bool MatchesFilter(PozycjaDokumentuMagazynowego item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdPozycjiDokumentuMagazynowego.ToString().Contains(text)
                   || (item.DokumentMagazynowy != null
                       && item.DokumentMagazynowy.Numer != null
                       && item.DokumentMagazynowy.Numer.ToLowerInvariant().Contains(text))
                   || (item.Towar != null
                       && item.Towar.Nazwa != null
                       && item.Towar.Nazwa.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<PozycjaDokumentuMagazynowego> ApplySort(
            IEnumerable<PozycjaDokumentuMagazynowego> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Dokument" => descending
                    ? query.OrderByDescending(p => p.DokumentMagazynowy != null ? p.DokumentMagazynowy.Numer : null)
                    : query.OrderBy(p => p.DokumentMagazynowy != null ? p.DokumentMagazynowy.Numer : null),

                "Towar" => descending
                    ? query.OrderByDescending(p => p.Towar != null ? p.Towar.Nazwa : null)
                    : query.OrderBy(p => p.Towar != null ? p.Towar.Nazwa : null),

                "Ilość" => descending
                    ? query.OrderByDescending(p => p.Ilosc)
                    : query.OrderBy(p => p.Ilosc),

                _ => descending
                    ? query.OrderByDescending(p => p.IdPozycjiDokumentuMagazynowego)
                    : query.OrderBy(p => p.IdPozycjiDokumentuMagazynowego)
            };
        }

        private void OpenAddDialog()
        {
            var viewModel = new NowaPozycjaDokumentuMagazynowegoViewModel();
            var dialog = new NowaPozycjaDokumentuMagazynowegoDialog
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
                MessageBox.Show("Podaj numer pozycji do usunięcia.");
                return;
            }

            if (!int.TryParse(DeleteNumber.Trim(), out var id))
            {
                MessageBox.Show("Numer pozycji musi być liczbą.");
                return;
            }

            var pozycja = fakturyEntities.PozycjaDokumentuMagazynowego
                .FirstOrDefault(item => item.IdPozycjiDokumentuMagazynowego == id);

            if (pozycja == null)
            {
                MessageBox.Show("Nie znaleziono pozycji o podanym numerze.");
                return;
            }

            fakturyEntities.PozycjaDokumentuMagazynowego.Remove(pozycja);
            fakturyEntities.SaveChanges();
            load();
        }
    }
}
