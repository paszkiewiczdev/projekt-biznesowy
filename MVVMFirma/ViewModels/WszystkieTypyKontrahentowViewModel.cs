using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieTypyKontrahentowViewModel : WszystkieViewModel<TypKontrahenta>
    {
        private string _nazwa;
        private bool _czyAktywny = true;

        public WszystkieTypyKontrahentowViewModel()
            : base()
        {
            base.DisplayName = "Typy kontrahentów";
            AddCommand = new BaseCommand(Dodaj, CanDodaj);
        }

        #region Właściwości bindowane

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
            List = new ObservableCollection<TypKontrahenta>(
                from t in fakturyEntities.TypKontrahenta
                select t
            );
        }

        private void Dodaj()
        {
            var nowy = new TypKontrahenta
            {
                Nazwa = Nazwa,
                CzyAktywny = CzyAktywny
            };

            fakturyEntities.TypKontrahenta.Add(nowy);
            fakturyEntities.SaveChanges();
            load();

            Nazwa = string.Empty;
            CzyAktywny = true;
        }

        private bool CanDodaj()
        {
            return !string.IsNullOrWhiteSpace(Nazwa);
        }
    }
}