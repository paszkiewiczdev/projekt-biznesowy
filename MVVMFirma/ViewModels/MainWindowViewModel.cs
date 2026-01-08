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

                new CommandViewModel(
                    "Pozycje faktur",
                    new BaseCommand(() => this.ShowAllPozycjeFakturySprzedazy())),

                new CommandViewModel(
                    "Zamówienia sprzedaży",
                    new BaseCommand(() => this.ShowAllZamowienia())),

                new CommandViewModel(
                    "Pozycje zamówień",
                    new BaseCommand(() => this.ShowAllPozycjeZamowienia())),

                new CommandViewModel(
                    "Dokumenty magazynowe",
                    new BaseCommand(() => this.ShowAllDokumentyMagazynowe())),

                new CommandViewModel(
                    "Pozycje dok. magazynowych",
                    new BaseCommand(() => this.ShowAllPozycjeDokumentuMagazynowego())),

                new CommandViewModel(
                    "Kontrahenci",
                    new BaseCommand(() => this.ShowAllKontrahenci())),

                new CommandViewModel(
                    "Adresy kontrahentów",
                    new BaseCommand(() => this.ShowAllAdresyKontrahentow())),

                new CommandViewModel(
                    "Stany magazynowe",
                    new BaseCommand(() => this.ShowAllStanyMagazynowe())),

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
                    new BaseCommand(() => this.ShowAllTypyKontrahentow())),

                // ===== RAPORTY – LOGIKA BIZNESOWA =====

                new CommandViewModel(
                    "Raport: Sprzedaż miesięczna",
                    new BaseCommand(() => this.ShowAllRaportSprzedazMiesieczna())),

                new CommandViewModel(
                    "Raport: Top towary",
                    new BaseCommand(() => this.ShowAllRaportTopTowary())),

                new CommandViewModel(
                    "Raport: Stan magazynowy",
                    new BaseCommand(() => this.ShowAllRaportStanMagazynowy()))
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
            if (e.NewItems != null)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += this.OnWorkspaceRequestClose;

            if (e.OldItems != null)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
        }

        private void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel workspace = sender as WorkspaceViewModel;
            this.Workspaces.Remove(workspace);
        }
        #endregion

        #region Helpers – standardowe widoki
        private void CreateView(WorkspaceViewModel workspace)
        {
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }

        private void ShowAllFaktury() => Open<WszystkieFakturyViewModel>();
        private void ShowAllPozycjeFakturySprzedazy() => Open<WszystkiePozycjeFakturySprzedazyViewModel>();
        private void ShowAllKontrahenci() => Open<WszyscyKontrahenciViewModel>();
        private void ShowAllAdresyKontrahentow() => Open<WszystkieAdresyKontrahentowViewModel>();
        private void ShowAllTowar() => Open<WszystkieTowaryViewModel>();
        private void ShowAllZamowienia() => Open<WszystkieZamowieniaViewModel>();
        private void ShowAllPozycjeZamowienia() => Open<WszystkiePozycjeZamowieniaViewModel>();
        private void ShowAllDokumentyMagazynowe() => Open<WszystkieDokumentyMagazynoweViewModel>();
        private void ShowAllPozycjeDokumentuMagazynowego() => Open<WszystkiePozycjeDokumentuMagazynowegoViewModel>();
        private void ShowAllStanyMagazynowe() => Open<WszystkieStanyMagazynoweViewModel>();
        private void ShowAllSposobyPlatnosci() => Open<WszystkieSposobyPlatnosciViewModel>();
        private void ShowAllWaluty() => Open<WszystkieWalutyViewModel>();
        private void ShowAllStawkiVat() => Open<WszystkieStawkiVatViewModel>();
        private void ShowAllJednostkiMiary() => Open<WszystkieJednostkiMiaryViewModel>();
        private void ShowAllKategorieTowaru() => Open<WszystkieKategorieTowaruViewModel>();
        private void ShowAllStatusyFaktur() => Open<WszystkieStatusyFakturViewModel>();
        private void ShowAllStatusyZamowien() => Open<WszystkieStatusyZamowienViewModel>();
        private void ShowAllTypyDokumentowMagazynowych() => Open<WszystkieTypyDokumentuMagazynowegoViewModel>();
        private void ShowAllMagazyny() => Open<WszystkieMagazynyViewModel>();
        private void ShowAllTypyKontrahentow() => Open<WszystkieTypyKontrahentowViewModel>();

        // ===== RAPORTY =====
        private void ShowAllRaportSprzedazMiesieczna() => Open<WszystkieRaportSprzedazMiesiecznaViewModel>();
        private void ShowAllRaportTopTowary() => Open<WszystkieRaportTopTowaryViewModel>();
        private void ShowAllRaportStanMagazynowy() => Open<WszystkieRaportStanMagazynowyViewModel>();

        private void Open<T>() where T : WorkspaceViewModel, new()
        {
            var workspace = this.Workspaces.FirstOrDefault(vm => vm is T) as T;
            if (workspace == null)
            {
                workspace = new T();
                this.Workspaces.Add(workspace);
            }
            this.SetActiveWorkspace(workspace);
        }

        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            collectionView?.MoveCurrentTo(workspace);
        }
        #endregion
    }
}
