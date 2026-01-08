using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieSposobyPlatnosciViewModel : WszystkieViewModel<SposobPlatnosci>
    {
        private string _nazwa;
        private string _opis;
        private bool _czyAktywny = true;

        public WszystkieSposobyPlatnosciViewModel()
            : base()
        {
            DisplayName = "Sposoby płatności";
            AddCommand = new BaseCommand(Dodaj, CanDodaj);
        }

        #region Właściwości formularza

        public string Nazwa
        {
            get => _nazwa;
            set
            {
                if (_nazwa != value)
                {
                    _nazwa = value;
                    OnPropertyChanged(() => Nazwa);
                }
            }
        }

        public string Opis
        {
            get => _opis;
            set
            {
                if (_opis != value)
                {
                    _opis = value;
                    OnPropertyChanged(() => Opis);
                }
            }
        }

        public bool CzyAktywny
        {
            get => _czyAktywny;
            set
            {
                if (_czyAktywny != value)
                {
                    _czyAktywny = value;
                    OnPropertyChanged(() => CzyAktywny);
                }
            }
        }

        #endregion

        #region Komendy

        public ICommand AddCommand { get; }

        #endregion

        public override void load()
        {
            List = new ObservableCollection<SposobPlatnosci>(
                from s in fakturyEntities.SposobPlatnosci
                select s
            );
        }

        private void Dodaj()
        {
            var nowy = new SposobPlatnosci
            {
                Nazwa = Nazwa,
                Opis = Opis,
                CzyAktywny = CzyAktywny
            };

            fakturyEntities.SposobPlatnosci.Add(nowy);
            fakturyEntities.SaveChanges();

            load(); // odświeżenie DataGrid

            // czyszczenie formularza
            Nazwa = string.Empty;
            Opis = string.Empty;
            CzyAktywny = true;
        }

        private bool CanDodaj()
        {
            return !string.IsNullOrWhiteSpace(Nazwa);
        }
    }
}
