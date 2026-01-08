using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieMagazynyViewModel : WszystkieViewModel<Magazyn>
    {
        private string _kod;
        private string _nazwa;
        private string _opis;
        private bool _czyAktywny = true;

        public WszystkieMagazynyViewModel()
            : base()
        {
            base.DisplayName = "Magazyny";
            AddCommand = new BaseCommand(Dodaj, CanDodaj);
        }

        #region Właściwości bindowane

        public string Kod
        {
            get => _kod;
            set
            {
                if (_kod != value)
                {
                    _kod = value;
                    OnPropertyChanged(() => Kod);
                }
            }
        }

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
            List = new ObservableCollection<Magazyn>(
                from m in fakturyEntities.Magazyn
                select m
            );
        }

        private void Dodaj()
        {
            var nowy = new Magazyn
            {
                Kod = Kod,
                Nazwa = Nazwa,
                Opis = Opis,
                CzyAktywny = CzyAktywny
            };

            fakturyEntities.Magazyn.Add(nowy);
            fakturyEntities.SaveChanges();
            load();

            Kod = string.Empty;
            Nazwa = string.Empty;
            Opis = string.Empty;
            CzyAktywny = true;
        }

        private bool CanDodaj()
        {
            return !string.IsNullOrWhiteSpace(Kod)
                && !string.IsNullOrWhiteSpace(Nazwa);
        }
    }
}