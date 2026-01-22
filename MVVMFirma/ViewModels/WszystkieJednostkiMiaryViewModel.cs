using System.Collections.Generic;
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
            SetSortOptions(new[] { "Id", "Symbol", "Nazwa" });
            AddCommand = new BaseCommand(Dodaj, CanDodaj);
            RefreshCommand = new BaseCommand(load);
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
        public ICommand RefreshCommand { get; }


        protected override IEnumerable<JednostkaMiary> LoadData()
        {
            return fakturyEntities.JednostkaMiary.ToList();
        }

        protected override bool MatchesFilter(JednostkaMiary item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdJednostkiMiary.ToString().Contains(text)
                   || (item.Symbol != null && item.Symbol.ToLowerInvariant().Contains(text))
                   || (item.Nazwa != null && item.Nazwa.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<JednostkaMiary> ApplySort(
            IEnumerable<JednostkaMiary> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Symbol" => descending
                    ? query.OrderByDescending(j => j.Symbol)
                    : query.OrderBy(j => j.Symbol),

                "Nazwa" => descending
                    ? query.OrderByDescending(j => j.Nazwa)
                    : query.OrderBy(j => j.Nazwa),

                _ => descending
                    ? query.OrderByDescending(j => j.IdJednostkiMiary)
                    : query.OrderBy(j => j.IdJednostkiMiary)
            };
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
