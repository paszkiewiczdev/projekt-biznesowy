using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieJednostkiMiaryViewModel : WszystkieViewModel<JednostkaMiary>
    {
        private string _symbol;
        private string _nazwa;
        private bool _czyAktywna = true;

        public WszystkieJednostkiMiaryViewModel()
            : base()
        {
            DisplayName = "Jednostki miary";
            AddCommand = new BaseCommand(Dodaj, CanDodaj);
        }

        public string Symbol
        {
            get => _symbol;
            set
            {
                if (_symbol != value)
                {
                    _symbol = value;
                    OnPropertyChanged(() => Symbol);
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

        public bool CzyAktywna
        {
            get => _czyAktywna;
            set
            {
                if (_czyAktywna != value)
                {
                    _czyAktywna = value;
                    OnPropertyChanged(() => CzyAktywna);
                }
            }
        }

        public ICommand AddCommand { get; }

        public override void load()
        {
            List = new ObservableCollection<JednostkaMiary>(
                from j in fakturyEntities.JednostkaMiary
                select j
            );
        }

        private void Dodaj()
        {
            var nowa = new JednostkaMiary
            {
                Symbol = Symbol,
                Nazwa = Nazwa,
                CzyAktywna = CzyAktywna
            };

            fakturyEntities.JednostkaMiary.Add(nowa);
            fakturyEntities.SaveChanges();

            load();

            Symbol = string.Empty;
            Nazwa = string.Empty;
            CzyAktywna = true;
        }

        private bool CanDodaj()
        {
            return !string.IsNullOrWhiteSpace(Symbol)
                   && !string.IsNullOrWhiteSpace(Nazwa);
        }
    }
}
