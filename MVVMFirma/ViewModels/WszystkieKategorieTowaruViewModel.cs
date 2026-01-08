using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieKategorieTowaruViewModel : WszystkieViewModel<KategoriaTowaru>
    {
        private string _nazwa;
        private string _opis;
        private bool _czyAktywny = true;

        public WszystkieKategorieTowaruViewModel()
            : base()
        {
            DisplayName = "Kategorie towaru";
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
            List = new ObservableCollection<KategoriaTowaru>(
                from k in fakturyEntities.KategoriaTowaru
                select k
            );
        }

        private void Dodaj()
        {
            var nowa = new KategoriaTowaru
            {
                Nazwa = Nazwa,
                Opis = Opis,
                CzyAktywny = CzyAktywny
            };

            fakturyEntities.KategoriaTowaru.Add(nowa);
            fakturyEntities.SaveChanges();

            load();

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
