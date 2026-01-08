using System.Collections.Generic;
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
            SetSortOptions(new[] { "Id", "Nazwa" });
            AddCommand = new BaseCommand(Dodaj, CanDodaj);
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

        public ICommand AddCommand { get; }

        protected override IEnumerable<KategoriaTowaru> LoadData()
        {
            return fakturyEntities.KategoriaTowaru.ToList();
        }

        protected override bool MatchesFilter(KategoriaTowaru item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdKategoriiTowaru.ToString().Contains(text)
                   || (item.Nazwa != null && item.Nazwa.ToLowerInvariant().Contains(text))
                   || (item.Opis != null && item.Opis.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<KategoriaTowaru> ApplySort(
            IEnumerable<KategoriaTowaru> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Nazwa" => descending
                    ? query.OrderByDescending(k => k.Nazwa)
                    : query.OrderBy(k => k.Nazwa),

                _ => descending
                    ? query.OrderByDescending(k => k.IdKategoriiTowaru)
                    : query.OrderBy(k => k.IdKategoriiTowaru)
            };
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
