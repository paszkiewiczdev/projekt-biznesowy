using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using MVVMFirma.Helper;
using System.Diagnostics;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;

namespace MVVMFirma.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Fields
        private ReadOnlyCollection<CommandViewModel> _Commands;
        private ObservableCollection<WorkspaceViewModel> _Workspaces;
        #endregion

        #region Commands
        public ReadOnlyCollection<CommandViewModel> Commands
        {
            get
            {
                if (_Commands == null)
                {
                    List<CommandViewModel> cmds = this.CreateCommands();
                    _Commands = new ReadOnlyCollection<CommandViewModel>(cmds);
                }
                return _Commands;
            }
        }

        private List<CommandViewModel> CreateCommands()
        {
            return new List<CommandViewModel>
            {
                
                new CommandViewModel(
                    "Towar",
                    new BaseCommand(() => this.CreateView(new NowyTowarViewModel()))),

                new CommandViewModel(
                    "Faktura",
                    new BaseCommand(() => this.CreateView(new NowaFakturaViewModel()))),

                new CommandViewModel(
                    "Kontrahent",
                    new BaseCommand(() => this.CreateView(new NowyKontrahentViewModel()))),

                
                new CommandViewModel(
                    "Sposób płatności",
                    new BaseCommand(() => this.CreateView(new NowySposobPlatnosciViewModel()))),

                
                new CommandViewModel(
                    "Towary",
                    new BaseCommand(() => this.ShowAllTowar())),

                new CommandViewModel(
                    "Faktury",
                    new BaseCommand(() => this.ShowAllFaktury())),

                // Pozycje faktur sprzedaży
                new CommandViewModel(
                    "Pozycje faktur",
                    new BaseCommand(() => this.ShowAllPozycjeFakturySprzedazy())),

                // Zamówienia sprzedaży
                new CommandViewModel(
                    "Zamówienia sprzedaży",
                    new BaseCommand(() => this.ShowAllZamowienia())),

                // Pozycje zamówień sprzedaży
                new CommandViewModel(
                    "Pozycje zamówień",
                    new BaseCommand(() => this.ShowAllPozycjeZamowienia())),

                // Dokumenty magazynowe
                new CommandViewModel(
                    "Dokumenty magazynowe",
                    new BaseCommand(() => this.ShowAllDokumentyMagazynowe())),

                // Pozycje dokumentów magazynowych
                new CommandViewModel(
                    "Pozycje dok. magazynowych",
                    new BaseCommand(() => this.ShowAllPozycjeDokumentuMagazynowego())),

                // Kontrahenci + adresy
                new CommandViewModel(
                    "Kontrahenci",
                    new BaseCommand(() => this.ShowAllKontrahenci())),

                new CommandViewModel(
                    "Adresy kontrahentów",
                    new BaseCommand(() => this.ShowAllAdresyKontrahentow())),

                // Stany magazynowe
                new CommandViewModel(
                    "Stany magazynowe",
                    new BaseCommand(() => this.ShowAllStanyMagazynowe())),

                // słowniki bez kluczy obcych
                new CommandViewModel(
                    "Sposoby płatności",
                    new BaseCommand(() => this.ShowAllSposobyPlatnosci())),

                new CommandViewModel(
                    "Waluty",
                    new BaseCommand(() => this.ShowAllWaluty())),

                new CommandViewModel(
                    "Stawki VAT",
                    new BaseCommand(() => this.ShowAllStawkiVat())),

                new CommandViewModel(
                    "Jednostki miary",
                    new BaseCommand(() => this.ShowAllJednostkiMiary())),

                new CommandViewModel(
                    "Kategorie towaru",
                    new BaseCommand(() => this.ShowAllKategorieTowaru())),

                new CommandViewModel(
                    "Statusy faktur",
                    new BaseCommand(() => this.ShowAllStatusyFaktur())),

                new CommandViewModel(
                    "Statusy zamówień",
                    new BaseCommand(() => this.ShowAllStatusyZamowien())),

                new CommandViewModel(
                    "Typy dok. magazynowych",
                    new BaseCommand(() => this.ShowAllTypyDokumentowMagazynowych())),

                new CommandViewModel(
                    "Magazyny",
                    new BaseCommand(() => this.ShowAllMagazyny())),

                new CommandViewModel(
                    "Typy kontrahentów",
                    new BaseCommand(() => this.ShowAllTypyKontrahentow()))
            };
        }
        #endregion

        #region Workspaces
        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_Workspaces == null)
                {
                    _Workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _Workspaces.CollectionChanged += this.OnWorkspacesChanged;
                }
                return _Workspaces;
            }
        }

        private void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += this.OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
        }

        private void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel workspace = sender as WorkspaceViewModel;
            this.Workspaces.Remove(workspace);
        }
        #endregion 

        #region Private Helpers

        private void CreateView(WorkspaceViewModel workspace)
        {
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }

        // faktury
        private void ShowAllFaktury()
        {
            WszystkieFakturyViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is WszystkieFakturyViewModel)
                as WszystkieFakturyViewModel;

            if (workspace == null)
            {
                workspace = new WszystkieFakturyViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        private void ShowAllPozycjeFakturySprzedazy()
        {
            WszystkiePozycjeFakturySprzedazyViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is WszystkiePozycjeFakturySprzedazyViewModel)
                as WszystkiePozycjeFakturySprzedazyViewModel;

            if (workspace == null)
            {
                workspace = new WszystkiePozycjeFakturySprzedazyViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        // kontrahenci
        private void ShowAllKontrahenci()
        {
            WszyscyKontrahenciViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is WszyscyKontrahenciViewModel)
                as WszyscyKontrahenciViewModel;

            if (workspace == null)
            {
                workspace = new WszyscyKontrahenciViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        private void ShowAllAdresyKontrahentow()
        {
            WszystkieAdresyKontrahentowViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is WszystkieAdresyKontrahentowViewModel)
                as WszystkieAdresyKontrahentowViewModel;

            if (workspace == null)
            {
                workspace = new WszystkieAdresyKontrahentowViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        // towary
        private void ShowAllTowar()
        {
            WszystkieTowaryViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is WszystkieTowaryViewModel)
                as WszystkieTowaryViewModel;

            if (workspace == null)
            {
                workspace = new WszystkieTowaryViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        // zamowienia
        private void ShowAllZamowienia()
        {
            WszystkieZamowieniaViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is WszystkieZamowieniaViewModel)
                as WszystkieZamowieniaViewModel;

            if (workspace == null)
            {
                workspace = new WszystkieZamowieniaViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        private void ShowAllPozycjeZamowienia()
        {
            WszystkiePozycjeZamowieniaViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is WszystkiePozycjeZamowieniaViewModel)
                as WszystkiePozycjeZamowieniaViewModel;

            if (workspace == null)
            {
                workspace = new WszystkiePozycjeZamowieniaViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        // dok. magazynowe
        private void ShowAllDokumentyMagazynowe()
        {
            WszystkieDokumentyMagazynoweViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is WszystkieDokumentyMagazynoweViewModel)
                as WszystkieDokumentyMagazynoweViewModel;

            if (workspace == null)
            {
                workspace = new WszystkieDokumentyMagazynoweViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        private void ShowAllPozycjeDokumentuMagazynowego()
        {
            WszystkiePozycjeDokumentuMagazynowegoViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is WszystkiePozycjeDokumentuMagazynowegoViewModel)
                as WszystkiePozycjeDokumentuMagazynowegoViewModel;

            if (workspace == null)
            {
                workspace = new WszystkiePozycjeDokumentuMagazynowegoViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        // stany magazynowe
        private void ShowAllStanyMagazynowe()
        {
            WszystkieStanyMagazynoweViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is WszystkieStanyMagazynoweViewModel)
                as WszystkieStanyMagazynoweViewModel;

            if (workspace == null)
            {
                workspace = new WszystkieStanyMagazynoweViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        // tabele bez kl. obcych
        private void ShowAllSposobyPlatnosci()
        {
            WszystkieSposobyPlatnosciViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is WszystkieSposobyPlatnosciViewModel)
                as WszystkieSposobyPlatnosciViewModel;

            if (workspace == null)
            {
                workspace = new WszystkieSposobyPlatnosciViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        private void ShowAllWaluty()
        {
            WszystkieWalutyViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is WszystkieWalutyViewModel)
                as WszystkieWalutyViewModel;

            if (workspace == null)
            {
                workspace = new WszystkieWalutyViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        private void ShowAllStawkiVat()
        {
            WszystkieStawkiVatViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is WszystkieStawkiVatViewModel)
                as WszystkieStawkiVatViewModel;

            if (workspace == null)
            {
                workspace = new WszystkieStawkiVatViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        private void ShowAllJednostkiMiary()
        {
            WszystkieJednostkiMiaryViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is WszystkieJednostkiMiaryViewModel)
                as WszystkieJednostkiMiaryViewModel;

            if (workspace == null)
            {
                workspace = new WszystkieJednostkiMiaryViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        private void ShowAllKategorieTowaru()
        {
            WszystkieKategorieTowaruViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is WszystkieKategorieTowaruViewModel)
                as WszystkieKategorieTowaruViewModel;

            if (workspace == null)
            {
                workspace = new WszystkieKategorieTowaruViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        private void ShowAllStatusyFaktur()
        {
            WszystkieStatusyFakturViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is WszystkieStatusyFakturViewModel)
                as WszystkieStatusyFakturViewModel;

            if (workspace == null)
            {
                workspace = new WszystkieStatusyFakturViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        private void ShowAllStatusyZamowien()
        {
            WszystkieStatusyZamowienViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is WszystkieStatusyZamowienViewModel)
                as WszystkieStatusyZamowienViewModel;

            if (workspace == null)
            {
                workspace = new WszystkieStatusyZamowienViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        private void ShowAllTypyDokumentowMagazynowych()
        {
            WszystkieTypyDokumentuMagazynowegoViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is WszystkieTypyDokumentuMagazynowegoViewModel)
                as WszystkieTypyDokumentuMagazynowegoViewModel;

            if (workspace == null)
            {
                workspace = new WszystkieTypyDokumentuMagazynowegoViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        private void ShowAllMagazyny()
        {
            WszystkieMagazynyViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is WszystkieMagazynyViewModel)
                as WszystkieMagazynyViewModel;

            if (workspace == null)
            {
                workspace = new WszystkieMagazynyViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        private void ShowAllTypyKontrahentow()
        {
            WszystkieTypyKontrahentowViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is WszystkieTypyKontrahentowViewModel)
                as WszystkieTypyKontrahentowViewModel;

            if (workspace == null)
            {
                workspace = new WszystkieTypyKontrahentowViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }
        #endregion
    }
}
