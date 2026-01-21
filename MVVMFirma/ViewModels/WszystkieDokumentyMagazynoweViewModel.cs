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
    public class WszystkieDokumentyMagazynoweViewModel : WszystkieViewModel<DokumentMagazynowy>
    {
        private string _deleteId;

        public WszystkieDokumentyMagazynoweViewModel()
        {
            DisplayName = "Dokumenty magazynowe";
            SetSortOptions(new[] { "Id", "Numer", "Data", "Magazyn" });

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

        protected override IEnumerable<DokumentMagazynowy> LoadData()
        {
            return fakturyEntities.DokumentMagazynowy
                .Include(d => d.Magazyn)
                .Include(d => d.TypDokumentuMagazynowego)
                .ToList();
        }

        protected override bool MatchesFilter(DokumentMagazynowy item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdDokumentuMagazynowego.ToString().Contains(text)
                   || (item.Numer != null && item.Numer.ToLowerInvariant().Contains(text))
                   || (item.Magazyn != null
                       && item.Magazyn.Nazwa != null
                       && item.Magazyn.Nazwa.ToLowerInvariant().Contains(text))
                   || (item.TypDokumentuMagazynowego != null
                       && item.TypDokumentuMagazynowego.Nazwa != null
                       && item.TypDokumentuMagazynowego.Nazwa.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<DokumentMagazynowy> ApplySort(IEnumerable<DokumentMagazynowy> query, string sortField, bool descending)
        {
            return sortField switch
            {
                "Numer" => descending
                    ? query.OrderByDescending(d => d.Numer)
                    : query.OrderBy(d => d.Numer),

                "Data" => descending
                    ? query.OrderByDescending(d => d.DataDokumentu)
                    : query.OrderBy(d => d.DataDokumentu),

                "Magazyn" => descending
                    ? query.OrderByDescending(d => d.Magazyn != null ? d.Magazyn.Nazwa : null)
                    : query.OrderBy(d => d.Magazyn != null ? d.Magazyn.Nazwa : null),

                _ => descending
                    ? query.OrderByDescending(d => d.IdDokumentuMagazynowego)
                    : query.OrderBy(d => d.IdDokumentuMagazynowego)
            };
        }

        private void OpenAddDialog()
        {
            var viewModel = new NowyDokumentMagazynowyViewModel();
            var dialog = new NowyDokumentMagazynowyDialog
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
                MessageBox.Show("Podaj ID dokumentu do usunięcia.");
                return;
            }

            if (!int.TryParse(DeleteId.Trim(), out var id))
            {
                MessageBox.Show("ID dokumentu musi być liczbą.");
                return;
            }

            var dokument = fakturyEntities.DokumentMagazynowy
                .FirstOrDefault(item => item.IdDokumentuMagazynowego == id);

            if (dokument == null)
            {
                MessageBox.Show("Nie znaleziono dokumentu o podanym ID.");
                return;
            }

            var pozycjeDoUsuniecia = fakturyEntities.PozycjaDokumentuMagazynowego
                .Where(p => p.IdDokumentuMagazynowego == dokument.IdDokumentuMagazynowego)
                .ToList();

            if (pozycjeDoUsuniecia.Any())
                fakturyEntities.PozycjaDokumentuMagazynowego.RemoveRange(pozycjeDoUsuniecia);

            fakturyEntities.DokumentMagazynowy.Remove(dokument);
            fakturyEntities.SaveChanges();
            load();
        }
    }
}
