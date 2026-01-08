using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieTypyDokumentuMagazynowegoViewModel
        : WszystkieViewModel<TypDokumentuMagazynowego>
    {
        private string _kod;
        private string _nazwa;
        private bool _czyAktywny = true;

        public WszystkieTypyDokumentuMagazynowegoViewModel()
            : base()
        {
            base.DisplayName = "Typy dokumentów magazynowych";
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
            List = new ObservableCollection<TypDokumentuMagazynowego>(
                from t in fakturyEntities.TypDokumentuMagazynowego
                select t
            );
        }

        private void Dodaj()
        {
            var nowy = new TypDokumentuMagazynowego
            {
                Kod = Kod,
                Nazwa = Nazwa,
                CzyAktywny = CzyAktywny
            };

            fakturyEntities.TypDokumentuMagazynowego.Add(nowy);
            fakturyEntities.SaveChanges();
            load();

            Kod = string.Empty;
            Nazwa = string.Empty;
            CzyAktywny = true;
        }

        private bool CanDodaj()
        {
            return !string.IsNullOrWhiteSpace(Kod)
                && !string.IsNullOrWhiteSpace(Nazwa);
        }
    }
}